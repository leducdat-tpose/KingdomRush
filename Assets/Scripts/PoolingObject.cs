using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    public static PoolingObject Instance;
    private Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private int _poolSize = 15;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
        foreach (var _prefab in _prefabs)
        {
            GameObject contain = new GameObject();
            contain.name = "Contain_" +_prefab.name;
            contain.transform.SetParent(this.transform);
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int j = 0; j < _poolSize; j++)
            {
                GameObject newGameObject = Instantiate(_prefab, contain.transform);
                newGameObject.name = _prefab.name;
                newGameObject.SetActive(false);
                queue.Enqueue(newGameObject);
            }
            _pools.Add(_prefab.name, queue);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public GameObject GetObject(GameObject gameObject)
    {
        if (_pools.TryGetValue(gameObject.name, out Queue<GameObject> queue))
        {
            if (queue.Count == 0) return CreateNewObject(gameObject);
            else
            {
                GameObject newObject = queue.Dequeue();
                newObject.SetActive(true);
                return newObject;
            }
        }
        else return CreateNewObject(gameObject);
    }
    
    public void ReturnObject(GameObject gameObject)
    {
        if (!_pools.TryGetValue(gameObject.name, out Queue<GameObject> queue)) return;
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
        queue.Enqueue(gameObject);
    }
    private GameObject CreateNewObject(GameObject prefab)
    {
        Transform contain = this.transform.Find("Contain_"+gameObject.name);
        GameObject newObject = Instantiate(prefab, contain);
        newObject.name = prefab.name;
        return newObject;
    }
}
