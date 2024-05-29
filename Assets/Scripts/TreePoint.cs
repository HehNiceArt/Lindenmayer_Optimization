using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//Andrei Quirante
public class TreePoint : MonoBehaviour
{
    public GameObject Self;

    [SerializeField] private Vector3 _firstScale = Vector3.one;
    public GameObject[] TreeIterationParents;
    [Space(10)]
    [Range(0f, 10f)]
    [SerializeField] private float _time = 1f;

    SphereCollider _sphereCollider;
    public static TreePoint Instance { get; private set; }

    TreeScaleCalculation _calculation;
    private void Awake()
    {
        _calculation = GetComponent<TreeScaleCalculation>();
        Instance = GetComponent<TreePoint>();
        _sphereCollider = GetComponent<SphereCollider>();
        for (int i = 1; i < TreeIterationParents.Length; i++)
        {
            TreeIterationParents[i].SetActive(false);
        }
    }
    IEnumerator DecreaseScaleOverTime(Vector3 _reduceScale)
    {
        float _elapsedTime = 0f;
        Vector3 _minScale = new Vector3(0.5f, 0.5f, 0.5f);
        while (_elapsedTime < _time)
        {
            transform.localScale -= _reduceScale * (Time.deltaTime * _time);
            if (transform.localScale.x < _minScale.x) { transform.localScale = _minScale; }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
