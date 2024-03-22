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
    public bool DetectEnemies = false;
    [Space(10)]
    public float _detectionRange;

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
    public void EnemyAttackTree(int _damage)
    {
        if (DetectEnemies)
        {
            float _dmgCalculation = _damage * 0.01f;
            Vector3 _dmgScale = (new Vector3(_dmgCalculation, _dmgCalculation, _dmgCalculation));
            Debug.Log($"Dmg Scale {_dmgScale}");
            StartCoroutine(DecreaseScaleOverTime(_dmgScale));
        }
    }
    IEnumerator DecreaseScaleOverTime(Vector3 _reduceScale)
    {
        DetectEnemies = true;
        float _elapsedTime = 0f;
        Vector3 _minScale = new Vector3(0.5f, 0.5f, 0.5f);
        while(_elapsedTime < _time && DetectEnemies)
        {
            transform.localScale -= _reduceScale * (Time.deltaTime * _time);
            if(transform.localScale.x < _minScale.x) { transform.localScale = _minScale; }
            _elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }

    private void Update()
    {
        CheckReduceScale();
    }
    void CheckReduceScale()
    {
        if(transform.localScale.x < _calculation.TreeItemIteration[4].x) { TreeIterationParents[4].SetActive(false); }
        if(transform.localScale.x < _calculation.TreeItemIteration[3].x) { TreeIterationParents[3].SetActive(false); }
        if(transform.localScale.x < _calculation.TreeItemIteration[2].x) { TreeIterationParents[2].SetActive(false); }
        if(transform.localScale.x < _calculation.TreeItemIteration[1].x) { TreeIterationParents[1].SetActive(false); }
    }
}
