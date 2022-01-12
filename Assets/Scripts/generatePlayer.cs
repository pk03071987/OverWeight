using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatePlayer : MonoBehaviour
{
    public GameObject playerToGenerate;
    Vector3 v;
    // Start is called before the first frame update
    void Start()
    {
        v = transform.position;
        v.y = 2;
        GameObject gm = Instantiate(playerToGenerate, v, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
