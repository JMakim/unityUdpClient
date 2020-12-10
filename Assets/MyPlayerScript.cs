using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerScript : MonoBehaviour
{
    public float speed = 100;
    public float rotSpeed = 50;

    public NetworkMan NM;

    public GameObject player;

    public float x;
    public float z;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Network");
        InvokeRepeating("Pos", 1, 1f);
        // InvokeRepeating("Pos",1,0.3f); //send 3 per sec
        // InvokeRepeating("Pos",1,0.1f); //send 10 per sec
        // InvokeRepeating("Pos",1,0.03f); //send 30 per sec
        NM = player.GetComponent<NetworkMan>();
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
            transform.Rotate(Vector3.up * speed * 5 * Time.deltaTime);
        }

        x = this.transform.position.x;
        z = this.transform.position.z;
        y = this.transform.rotation.y;

    }



    void Pos()
    {
        NM.SendPosition(x, y, z);
    }
}
