using System.Collections.Generic;
using UnityEngine;

public class Grass : Floor
{

    [SerializeField]
    PoolObjectType objectType;

    public override void Reset()
    {
        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

}