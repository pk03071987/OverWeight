using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletctr : MonoBehaviour
{
    Vector3 startpos;
    Rigidbody rig;
    public float speed;
    public float distace=30;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        transform.eulerAngles = new Vector3(90, 0, 0);
        startpos = transform.position;
        rig = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rig.velocity = Vector3.forward * speed;
        if(transform.position.z-startpos.z>distace)
        {
            Destroy(gameObject);
        }

        if(transform.position.z> GameObject.FindGameObjectWithTag("endlevel").transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}
