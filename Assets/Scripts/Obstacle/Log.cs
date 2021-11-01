using DG.Tweening;
using UnityEngine;

public class Log : MonoBehaviour
{

    [SerializeField]
    PoolObjectType objectType;

    public void ReturnObject()
    {
        gameObject.transform.DOKill();

        ObjectPool.Instance.ReturnObject(
            objectType, gameObject);
    }

}