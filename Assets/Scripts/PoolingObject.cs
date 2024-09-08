using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    public static PoolingObject Instance;
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private int _poolSize = 6;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < _prefabs.Length; i++)
        {
            GameObject contain = new GameObject();
            contain.name = "Contain_" +_prefabs[i].name;
            contain.transform.SetParent(this.transform);
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int j = 0; j < _poolSize; j++)
            {
                GameObject prefab = CreateNewObject(_prefabs[i]);
                prefab.transform.SetParent(contain.transform);
                prefab.SetActive(false);
                queue.Enqueue(prefab);
            }
            pools.Add(_prefabs[i].name, queue);
        }
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.name = prefab.name;
        return newObject;
    }
}
