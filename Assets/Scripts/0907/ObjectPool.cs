using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PoolObjectType
{
    RedBullet,
    GreenBullet,
    BlueBullet
}

[Serializable]
public class ObjectPoolData
{
    public List<GameObject> prefabs = new List<GameObject>();

    public List<int> prefabCreateCounts = new List<int>();
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField]
    ObjectPoolData objectPoolData;

    Dictionary<PoolObjectType, Queue<GameObject>> poolObjectMap = 
        new Dictionary<PoolObjectType, Queue<GameObject>>();

    private void Awake()
    {
        Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < objectPoolData.prefabs.Count; i++)
        {
            poolObjectMap.Add((PoolObjectType)i, new Queue<GameObject>());

            for (int j = 0; j < objectPoolData.prefabCreateCounts[i]; j++)
                poolObjectMap[(PoolObjectType)i].Enqueue(CreateNewObject(i));
        }
    }

    private GameObject CreateNewObject(int index)
    {
        var newObj = Instantiate(objectPoolData.prefabs[index]);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public GameObject GetObject(PoolObjectType type)
    {
        if (Instance.poolObjectMap[type].Count > 0)
        {
            var obj = Instance.poolObjectMap[type].Dequeue();
            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject((int)type);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(transform);

            poolObjectMap[type].Enqueue(newObj);

            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        PoolObjectType type = obj.GetComponent<Bullet>().Type;
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolObjectMap[type].Enqueue(obj);
    }
}
