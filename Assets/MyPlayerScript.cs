using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerScript : MonoBehaviour
{
    public float speed = 10;
    public float rotSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

 

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {

            transform.Rotate(Vector3.up * -speed * 5 * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * speed * 5 *  Time.deltaTime);
        }
    }
}
