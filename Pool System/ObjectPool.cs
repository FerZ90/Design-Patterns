using System.Collections;
using UnityEngine;

public class ObjectPool
{
    private int _poolSize;
    private GameObject _poolPrefab;
    private bool configured;
    private PoolObject[] poolObjects;
    private Hashtable poolHash;

    public ObjectPool(int poolSize, GameObject prefab)
    {
        _poolSize = poolSize;
        _poolPrefab = prefab;

        if (!configured)
            Configure();
    } 

    void Configure()
    {
        poolObjects = new PoolObject[_poolSize];
        poolHash = new Hashtable();
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Object.Instantiate(_poolPrefab) as GameObject;
            Transform t = go.transform;      
            poolObjects[i] = new PoolObject(t);
            poolHash.Add(t, poolObjects[i]);
        }
        configured = true;
    }

    public bool Dispose(Transform t)
    {
        if (poolHash.ContainsKey(t))
        {
            PoolObject obj = (PoolObject)poolHash[t];              
            obj.Dispose();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Transform GetFirstAvailable()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].InUse)
            {
                poolObjects[i].Use();           
                return poolObjects[i].Object;
            }
        }

        return null;
    }
}

public class PoolObject
{
    private Transform _transform;
    private bool _inUse;
    public bool InUse => _inUse;
    public Transform Object => _transform;

    public PoolObject(Transform t) 
    {
        _transform = t;
    }
    public void Use() 
    {
        _transform.gameObject.SetActive(true);
        _inUse = true; 
    }
    public void Dispose() 
    { 
        _inUse = false;
        _transform.gameObject.SetActive(false);
    }   
}

