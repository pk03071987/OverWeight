using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class obstacleGenerate : MonoBehaviour
{
    public GameObject obstacle;
    public int number = 10;
    Vector3 v;
    // Start is called before the first frame update
    void Start()
    {
        v = transform.position;
        v.y = 3;
        GameObject gm=Instantiate(obstacle, v, Quaternion.identity);
        gm.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = Random.Range(1, number).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
