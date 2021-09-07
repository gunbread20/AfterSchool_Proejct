using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public PoolObjectType Type
    {
        get
        {
            return type;
        }
    }

    [SerializeField]
    private PoolObjectType type;

    private Vector3 direction;

    public void Shoot(Vector3 direction)
    {
        this.direction = direction;

        Invoke("ReturnObject", 2);
    }

    private void Update()
    {
        transform.position += direction * 10 * Time.deltaTime;
    }

    void ReturnObject()
    {
        ObjectPool.Instance.ReturnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            ReturnObject();
        }
    }
}
