using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

//Andrei Quirante
public class TreeScaleCalculation : MonoBehaviour
{
    public List<Vector3> TreeItemIteration = new List<Vector3>();
    public float _growthRate = 1f;
    public bool IsGrowing = false;
    public int TreeItemValue;
    TreePoint _treePoint;

    public bool _isFirstIteration;
    public bool _isSecondIteration;
    public bool _isThirdIteration;
    public bool _isFourthIteration;
    public bool _isFifthIteration;
    public static TreeScaleCalculation Instance { get; private set; }
    Vector3 _treeScale = Vector3.one;
    SphereCollider _sphereCollider;
    private void Awake()
    {
        Instance = GetComponent<TreeScaleCalculation>();
        _treePoint = GetComponent<TreePoint>();
        _sphereCollider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        CheckScale();
    }
    void CheckScale()
    {
        float _scaleX = transform.localScale.x;

        if (_scaleX >= TreeItemIteration[0].x && _scaleX <= TreeItemIteration[1].x && !_isFirstIteration)
        {
            _isFirstIteration = true;
            CheckIteration(1);
        }
        if (_scaleX >= TreeItemIteration[1].x && _scaleX <= TreeItemIteration[2].x && !_isSecondIteration)
        {
            _isSecondIteration = true;
            CheckIteration(2);
        }
        if (_scaleX >= TreeItemIteration[2].x && _scaleX <= TreeItemIteration[3].x && !_isThirdIteration)
        {
            _isThirdIteration = true;
            CheckIteration(3);
        }
        if (_scaleX >= TreeItemIteration[3].x && _scaleX <= TreeItemIteration[4].x && !_isFourthIteration)
        {
            _isFourthIteration = true;
            CheckIteration(4);
        }
        if (_scaleX >= TreeItemIteration[4].x && !_isFifthIteration)
        {
            _isFifthIteration = true;
            CheckIteration(5);
        }
    }
    void CheckIteration(int _iteration)
    {
        if (_iteration == 1) { Iteration(_iteration); }
        else if (_iteration == 2) { Iteration(_iteration); }
        else if (_iteration == 3) { Iteration(_iteration); }
        else if (_iteration == 4) { Iteration(_iteration); }
        else if (_iteration == 5) { Iteration(_iteration); }
    }
    void Iteration(int _local)
    {
        IterateTreeScale(_local);
    }
    void IterateTreeScale(int _index)
    {
        Vector3 _currentScale = transform.localScale;
        var _queue = new Queue<Vector3>();
        _queue.Enqueue(_currentScale);
        transform.localScale = _treeScale;

        TreeLSystem3D.Instance.Generate(_index);
        TreeLSystem3D.Instance._iteration = _index;

        transform.localScale = _queue.Dequeue();
    }

    IEnumerator IncreaseScaleOverTime(Vector3 _calcScale, Vector3 _currentScale)
    {
        Vector3 _desiredScale = _currentScale + _calcScale;
        while (_currentScale.x <= _desiredScale.x)
        {
            _currentScale += _calcScale * Time.deltaTime * _growthRate;
            transform.localScale = _currentScale;
            //_treePoint._detectionRange += TreeItemValue * 0.01f;

            IsGrowing = false;
            yield return null;
        }
    }
}
