using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TransformInfo
{
    public Vector3 Position;
    public Quaternion Rotation;
}
public class TreeRuleParsing : MonoBehaviour
{
    [SerializeField] private int _iteration = 4;

    [SerializeField] private float _width = 1f;
    [SerializeField] private float _length = 2f;
    [SerializeField] private float _angle = 30f;
    // [SerializeField] private float _roll = 30f; // (\ roll left) (/ roll right)

    [Header("Tree Parts")]
    [SerializeField] private GameObject _branch;
    [SerializeField] private GameObject _leaf;
    [SerializeField] private GameObject _flower;

    private const string _axiom = "X";

    private Stack<TransformInfo> _transformStack;
    private Dictionary<char, string> _rules;
    private string _currentString = string.Empty;

    private void Start()
    {
        _transformStack = new Stack<TransformInfo>();

        _rules = new Dictionary<char, string>
        {
            //this should be the one to be edited
            {'X', "F" },
            {'F', "F[-F]F[+F][F]" }
        };
        Generate();
    }

    void Generate()
    {
        _currentString = _axiom;

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < _iteration; i++)
        {
            foreach (char c in _currentString)
            {
                stringBuilder.Append(_rules.ContainsKey(c) ? _rules[c] : c.ToString());
            }

            _currentString = stringBuilder.ToString();
            stringBuilder = new StringBuilder();
        }

        for (int i = 0; i < _currentString.Length; i++)
        {
            //Rules
            switch (_currentString[i])
            {
                //move forward at distance and draw a line
                case 'F':
                    Vector3 _initialPosition = transform.position;
                    transform.Translate(Vector3.up * _length);

                    GameObject _branchSegment = Instantiate(_branch);
                    _branchSegment.GetComponent<LineRenderer>().SetPosition(0, _initialPosition);
                    _branchSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    _branchSegment.GetComponent<LineRenderer>().startWidth = _width;
                    _branchSegment.GetComponent<LineRenderer>().endWidth = _width;
                    break;
                //move forward at distance without drawing a line
                case 'X':
                    break;
                //turn right
                case '+':
                    transform.Rotate(Vector3.back * _angle);
                    break;
                //turn left
                case '-':
                    transform.Rotate(Vector3.forward * _angle);
                    break;
                //turn save current transform info
                case '[':
                    _transformStack.Push(new TransformInfo()
                    {
                        Position = transform.position,
                        Rotation = transform.rotation,
                    });
                    break;
                //return to previously saved transform info
                case ']':
                    TransformInfo ti = _transformStack.Pop();
                    transform.position = ti.Position;
                    transform.rotation = ti.Rotation;
                    break;
                default:
                    throw new InvalidOperationException("Invalid L-system operation");
            }
        }

    }
}
