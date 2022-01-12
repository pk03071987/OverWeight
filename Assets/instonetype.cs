using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instonetype : MonoBehaviour
{
    int i;
    // Start is called before the first frame update
    void Start()
    {
        i = transform.childCount;
        transform.GetChild(Random.Range(0,i)).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
