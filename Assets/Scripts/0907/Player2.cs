using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;
    private int bulletType;

    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * 10 * Time.deltaTime;
        transform.position += new Vector3(0, 0, Input.GetAxis("Vertical")) * 10 * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            bulletType++;

            //if (bulletType > 2)
            {
                bulletType = 0;
            }
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hitResult;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitResult))
            {
                Bullet bullet = ObjectPool.Instance.GetObject((PoolObjectType)bulletType).GetComponent<Bullet>();
                Vector3 direction = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
                bullet.transform.position = transform.position;
                bullet.Shoot(direction.normalized);
            }
        }
    }


}
