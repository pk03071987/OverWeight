using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondBullet : MonoBehaviour
{
    Vector3 startps;
    public float speed;
    public GameObject ToFollow;
    public bool startshoot;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        transform.eulerAngles = new Vector3(90, 0, 0);
        startps = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(ToFollow!=null && startshoot)
        {
            transform.position = Vector3.MoveTowards(transform.position, ToFollow.transform.GetChild(0).position, speed*Time.deltaTime);
        }
        
        if(startshoot && ToFollow==null)
        {
            Destroy(gameObject);
        }
    }
}
