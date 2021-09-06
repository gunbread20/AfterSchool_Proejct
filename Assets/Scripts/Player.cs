using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject prefabs;

    private void Awake()
    {
        GameObject cube = Instantiate(prefabs);
        cube.tag = "Cube";
        cube.transform.position = new Vector3(0, 1, 5);
    }

    void Start()
    {
        Debug.Log("Start");
    }

    void Update()
    {
        Vector3 result = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            result = Vector3.forward;
        else if (Input.GetKey(KeyCode.A))
            result = Vector3.left;
        else if (Input.GetKey(KeyCode.S))
            result = Vector3.back;
        else if (Input.GetKey(KeyCode.D))
            result = Vector3.right;


        transform.GetComponent<Rigidbody>().AddForce(result);
        //transform.position += result * 10 * Time.deltaTime;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + new Vector3(0, 10, -10), Time.deltaTime * 15);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);

        if (collision.gameObject.tag == ("Cube"))
        {
            Vector3 direction = transform.position - collision.gameObject.transform.position;

            transform.GetComponent<Rigidbody>().AddForce(direction * 100);
        }

    }
}
