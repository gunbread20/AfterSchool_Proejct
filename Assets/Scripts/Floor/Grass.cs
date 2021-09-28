using System.Collections.Generic;
using UnityEngine;

public class Grass : Floor
{
    List<GameObject> trees = new List<GameObject>();

    public override void Generate()
    {
        CreateTrees();
    }

    void CreateTrees()
    {
        int random = Random.Range(3, 5);

        for (int i = 0; i < random; i++)
        {
            trees.Add(ObjectPool.Instance.GetObject((PoolObjectType)Random.Range(3, 6)));

            trees[trees.Count - 1].transform.SetParent(transform, true);
            trees[trees.Count - 1].transform.position = GetRandomTreePos();
        }
    }

    public override void ReturnTrees()
    {
        for (int i = 0; i < trees.Count; i++)
        {
            switch (trees[i].tag)
            {
                case "Tree_Type0":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Tree_Type0, trees[i]);
                    break;
                case "Tree_Type1":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Tree_Type1, trees[i]);
                    break;
                case "Tree_Type2":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Tree_Type2, trees[i]);
                    break;
                default:
                    break;
            }
        }
    }

    Vector3 GetRandomTreePos()
    {
        Vector3 ranPos = Vector3.zero;

        ranPos = new Vector3(Random.Range(-10, 11), 0, transform.position.z);

        if (ranPos == Vector3.zero)
        {
            ranPos = GetRandomTreePos();
        }

        for (int i = 0; i < trees.Count; i++)
        {
            if (ranPos == trees[i].transform.position)
            {
                ranPos = GetRandomTreePos();
            }
        }
        
        return ranPos;
    }

    [SerializeField]
    PoolObjectType objectType;

    public override void Reset()
    {
        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

}