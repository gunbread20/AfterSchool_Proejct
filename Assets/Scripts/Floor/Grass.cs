using System.Collections.Generic;
using UnityEngine;

public class Grass : Floor
{
    List<GameObject> trees = new List<GameObject>();

    public override void Generate()
    {
        ReturnTrees();

        CreateTrees();
    }

    void CreateTrees()
    {
        int random = Random.Range(3, 5);

        for (int i = 0; i < 6; i++)
        {
            trees.Add(ObjectPool.Instance.GetObject((PoolObjectType)Random.Range(3, 6)));

            trees[trees.Count - 1].transform.SetParent(transform, true);

            switch (i)
            {
                case 0:
                    trees[trees.Count - 1].transform.position = new Vector3(-13, 0, transform.position.z);

                    break;
                case 1:
                    trees[trees.Count - 1].transform.position = new Vector3(-12, 0, transform.position.z);

                    break;
                case 2:
                    trees[trees.Count - 1].transform.position = new Vector3(-11, 0, transform.position.z);

                    break;

                case 3:
                    trees[trees.Count - 1].transform.position = new Vector3(11, 0, transform.position.z);

                    break;
                case 4:
                    trees[trees.Count - 1].transform.position = new Vector3(12, 0, transform.position.z);

                    break;
                case 5:
                    trees[trees.Count - 1].transform.position = new Vector3(13, 0, transform.position.z);

                    break;

                default:
                    trees[trees.Count - 1].transform.position = GetRandomTreePos();
                    break;
            }
            
        }

        if (transform.position.z >= 1)
        {
            for (int i = 0; i < random; i++)
            {
                trees.Add(ObjectPool.Instance.GetObject((PoolObjectType)Random.Range(3, 6)));

                trees[trees.Count - 1].transform.SetParent(transform, true);
                trees[trees.Count - 1].transform.position = GetRandomTreePos();
            }
        }
        
    }

    void ReturnTrees()
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

        trees.Clear();
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