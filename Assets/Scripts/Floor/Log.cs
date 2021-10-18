using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Log : MonoBehaviour
{
    [SerializeField]
    PoolObjectType objectType;

    public void ReturnObject()
    {
        gameObject.transform.DOKill();

        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }
}
