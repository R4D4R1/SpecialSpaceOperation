using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Bullet,
    Asteroid,
    VFXExplosion,
    PSRockHit,
    HP,
    HPHits
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int PoolSize = 0;
    public GameObject prefab;
    public GameObject container;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [SerializeField] public List<PoolInfo> listOfPools;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < listOfPools.Count; i++)
        {
            InitPool(listOfPools[i]);
        }
    }

    private void InitPool(PoolInfo info)
    {
        for (int i = 0; i < info.PoolSize; i++)
        {
            GameObject objectInstance = null;
            objectInstance = Instantiate(info.prefab, info.container.transform);
            objectInstance.SetActive(false);
            objectInstance.transform.position = Vector3.zero;
            info.pool.Add(objectInstance);
        }
    }

    public GameObject GetPooledObject(PoolObjectType type, Vector3 position)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject objectInstance = null;

        if (pool.Count > 0)
        {
            objectInstance = pool[pool.Count - 1];
            pool.Remove(objectInstance);
        }
        else
        {
            objectInstance = Instantiate(selected.prefab, position, Quaternion.identity);
            objectInstance.transform.parent = selected.container.transform;
        }

        objectInstance.transform.position = position;
        objectInstance.SetActive(true);
        return objectInstance;
    }

    public void ReleaseObject(GameObject obj, PoolObjectType type)
    {
        obj.SetActive(false);

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if (!pool.Contains(obj))
            pool.Add(obj);
    }

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        for (int i = 0; i < listOfPools.Count; i++)
        {
            if (type == listOfPools[i].type)
            {
                return listOfPools[i];
            }
        }

        return null;
    }
}