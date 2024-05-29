using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;

//Andrei Quirante
public class TransformInfo3D
{
    public Vector3 Position3D;
    public Quaternion Rotation3D;
}
public class TreeLSystem3D : MonoBehaviour
{
    [Range(0, 5)] public int _iteration = 0;

    [Header("Tree Values")]
    [SerializeField][Range(0, 2)] private float _maxLength;
    [SerializeField][Range(0, 1)] private float _diameter;
    [SerializeField][Range(0, 5)] private float _leafVariance;
    [SerializeField] private float _angle;
    public float LeafProbability = 0.5f;

    [Header("Tree Parents")]
    [SerializeField] private GameObject _treeParentBranch;
    [SerializeField] private GameObject _treeParentLeaf;

    [Header("Tree Parts")]
    public GameObject _treeBranch;
    public GameObject _treeLeaf;

    [Header("Rules")]
    [SerializeField] private string _initialState;
    [SerializeField] private string _productionRule;

    [Space(10f)]
    public TreeRotations _treeRotations;
    private const string _axiom = "X";
    private int poolBranchSize = 500;
    private int poolLeafSize = 100;

    private Dictionary<char, string> _rules;
    private Stack<TransformInfo3D> _transformStack;
    private string _currentString = string.Empty;
    public static TreeLSystem3D Instance { get; private set; }
    private void Awake()
    {
        Instance = GetComponent<TreeLSystem3D>();
    }
    private void Start()
    {
        _transformStack = new Stack<TransformInfo3D>();

        Renderer _treeBranchColor = _treeBranch.GetComponent<Renderer>();
        Renderer _treeLeafColor = _treeLeaf.GetComponent<Renderer>();

        if (_treeBranchColor != null && _treeBranchColor.sharedMaterial != null)
        {
            _treeBranchColor.sharedMaterial.enableInstancing = true;
        }
        if (_treeLeafColor != null && _treeLeafColor.sharedMaterial != null)
        {
            _treeLeafColor.sharedMaterial.enableInstancing = true;
        }

        _rules = new Dictionary<char, string>
        {
            {'X', _initialState },
            {'F', _productionRule }
        };

        InitializeObjectPool(poolBranchSize, poolLeafSize);
        Task.Run(() => Generate(_iteration));
    }

    private StringBuilder _stringBuilder = new StringBuilder();
    private Queue<GameObject> branchPool = new Queue<GameObject>();
    private Queue<GameObject> leafPool = new Queue<GameObject>();

    void InitializeObjectPool(int branchSize, int leafSize)
    {
        // Populate branch and leaf object pools
        for (int i = 0; i < branchSize; i++)
        {
            GameObject branch = Instantiate(_treeBranch, Vector3.zero, Quaternion.identity);
            branch.SetActive(false);
            branchPool.Enqueue(branch);

        }
        for (int j = 0; j < leafSize; j++)
        {
            GameObject leaf = Instantiate(_treeLeaf, Vector3.zero, Quaternion.identity);
            leaf.SetActive(false);
            leafPool.Enqueue(leaf);
        }
    }

    GameObject GetBranchFromPool()
    {
        if (branchPool.Count == 0)
        {
            GameObject branch = Instantiate(_treeBranch, Vector3.zero, Quaternion.identity) as GameObject;
            branch.SetActive(false);
            branch.transform.parent = _treeParentBranch.transform;
            return branch;
        }

        GameObject pooledBranch = branchPool.Dequeue();
        pooledBranch.SetActive(true);
        pooledBranch.transform.parent = _treeParentBranch.transform;
        return pooledBranch;
    }

    void ReturnBranchToPool(GameObject branch)
    {
        branch.SetActive(false);
        branchPool.Enqueue(branch);
    }

    GameObject GetLeafFromPool()
    {
        if (leafPool.Count == 0)
        {
            GameObject leaf = Instantiate(_treeLeaf, Vector3.zero, Quaternion.identity) as GameObject;
            leaf.transform.parent = _treeLeaf.transform;
            leaf.SetActive(false);
            return leaf;
        }

        GameObject pooledLeaf = leafPool.Dequeue();
        pooledLeaf.SetActive(true);
        pooledLeaf.transform.parent = _treeParentLeaf.transform;
        return pooledLeaf;
    }

    void ReturnLeafToPool(GameObject leaf)
    {
        leaf.SetActive(false);
        leafPool.Enqueue(leaf);
    }

    public void Generate(int _updateIteration)
    {
        _currentString = _axiom;

        for (int i = 0; i < _updateIteration; i++)
        {
            _stringBuilder.Clear();

            Parallel.For(0, _currentString.Length, j =>
            {
                char c = _currentString[j];
                _stringBuilder.Append(_rules.ContainsKey(c) ? _rules[c] : c.ToString());
            });

            _currentString = _stringBuilder.ToString();
        }

        for (int i = 0; i < _currentString.Length; i++)
        {
            //Symbols and their functions
            switch (_currentString[i])
            {
                case 'F':   // Move forward a step of length d. A line segment between points (X,Y,Z) and (X', Y', Z') is drawn;
                    CreateBranch();
                    break;
                case 'X':
                    break;
                case '+':       // Turn left by angle Delta, Using rotation matrix R_U(Delta).
                    TurnLeft();
                    break;
                case '-':       // Turn right by angle Delta, Using rotation matrix R_U(-Delta).
                    TurnRight();
                    break;
                case '^':       // Pitch by angle Delta, Using rotation matrix R_L(-Delta).
                    PitchUp();
                    break;
                case '&':
                    PitchDown();
                    break;
                case 'l':
                    RollLeft();
                    break;
                case '/':
                    RollRight();
                    break;
                case 'a':
                    Leaf();
                    break;
                case '[':       // Push the current state 
                    _transformStack.Push(new TransformInfo3D()
                    {
                        Position3D = transform.position,
                        Rotation3D = transform.rotation,
                    });
                    break;
                case ']':       // Pop the current state
                    if (_transformStack.Count > 0)
                    {
                        TransformInfo3D ti = _transformStack.Pop();
                        transform.position = ti.Position3D;
                        transform.rotation = ti.Rotation3D;
                    }
                    else
                    {
                        Debug.LogWarning("Trying to pop from an empty stack.");
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid L-system operaion");
            }
        }
    }
    Quaternion _initialRotation;
    GameObject _branch;
    void CreateBranch() //Create the tree branch
    {
        _branch = GetBranchFromPool();                           //Instantiate the branch at (0,0,0)
        _initialRotation = Quaternion.identity;                                                               //Gets the initial Rotation of the Branch

        _branch.transform.localScale = new Vector3(_diameter, _maxLength, _diameter);

        _branch.transform.position = transform.position;

        transform.Translate(Vector3.up * _maxLength);

        _maxLength -= GetRandomFloat(0, 0.5f); // Optimize random number generation for _maxLength
        _diameter -= 0.01f;

        if (_maxLength <= 0)
        {
            _maxLength = 2;
        }
        if (_diameter <= 0.1f)
        {
            _diameter = 0.5f;
        }
    }
    void Leaf()
    {
        float randomValue = GetRandomFloat(0, 1); // Optimize random number generation for randomValue
        float rand = GetRandomFloat(1, _leafVariance);

        float randomRange = GetRandomFloat(-90, 90);
        if (randomValue < LeafProbability)
        {
            GameObject _leaf = GetLeafFromPool();
            _leaf.transform.position = transform.position;
            _leaf.transform.rotation = Quaternion.Euler(randomRange, randomRange, randomRange);
            _leaf.transform.localScale = new Vector3(rand, rand, rand);
        }
    }

    void TurnLeft()                     // +
    {
        Quaternion _currentRotation = transform.rotation;
        Matrix4x4 _rotationUp = Matrix4x4.TRS(Vector3.zero, _currentRotation, Vector3.one) * _treeRotations.RotationUp(_angle);
        Quaternion _rotation = Quaternion.LookRotation(_rotationUp.GetColumn(2), _rotationUp.GetColumn(1));

        transform.rotation = _rotation;
    }
    void TurnRight()                    // -        
    {
        Quaternion _currentRotation = transform.rotation;
        Matrix4x4 _rotationDown = Matrix4x4.TRS(Vector3.zero, _currentRotation, Vector3.one) * _treeRotations.RotationUp(-_angle);
        Quaternion _rotation = Quaternion.LookRotation(_rotationDown.GetColumn(2), _rotationDown.GetColumn(1));

        transform.rotation = _rotation;
    }
    void PitchUp()                      // ^
    {
        Quaternion _currentRotation = transform.rotation;
        Matrix4x4 _pitchUp = Matrix4x4.TRS(Vector3.zero, _currentRotation, Vector3.one) * _treeRotations.RotationPitch(_angle);
        Quaternion _rotation = Quaternion.LookRotation(_pitchUp.GetColumn(0), _pitchUp.GetColumn(1));

        transform.rotation = _rotation;
    }
    void PitchDown()                    // &
    {
        Quaternion _currentRotation = transform.rotation;
        Matrix4x4 _pitchDown = Matrix4x4.TRS(Vector3.zero, _currentRotation, Vector3.one) * _treeRotations.RotationPitch(-_angle);
        Quaternion _rotation = Quaternion.LookRotation(_pitchDown.GetColumn(0), _pitchDown.GetColumn(1));

        transform.rotation = _rotation;
    }
    void RollLeft()                     // l
    {
        Quaternion _currentRotation = transform.rotation;
        Matrix4x4 _rollLeft = Matrix4x4.TRS(Vector3.zero, _currentRotation, Vector3.one) * _treeRotations.RotationHeading(_angle);
        Quaternion _rotation = Quaternion.LookRotation(_rollLeft.GetColumn(2), _rollLeft.GetColumn(1));

        transform.rotation = _rotation;
    }
    void RollRight()                    // /
    {
        Quaternion _currentRotation = transform.rotation;
        Matrix4x4 _rollRight = Matrix4x4.TRS(Vector3.zero, _currentRotation, Vector3.one) * _treeRotations.RotationHeading(-_angle);
        Quaternion _rotation = Quaternion.LookRotation(_rollRight.GetColumn(2), _rollRight.GetColumn(1));

        transform.rotation = _rotation;
    }

    private System.Random random = new System.Random(); // Initialize random number generator

    float GetRandomFloat(float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }

}
