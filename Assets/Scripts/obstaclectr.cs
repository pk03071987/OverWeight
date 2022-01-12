using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class obstaclectr : MonoBehaviour
{
    public TextMeshProUGUI txt;
    bool forOnce;
    float number;
    float scaleToCut;
    public Material[] materials;
    public GameObject[] EffectsColor;

    public GameObject bulletEffect;
    int intMat;
    
    // Start is called before the first frame update
    void Start()
    {
        intMat = Random.Range(0, materials.Length);
        GetComponent<MeshRenderer>().material = materials[intMat];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="bullet")
        {
            Debug.Log("to eat");
            Vector3 vb = other.gameObject.transform.position;
            vb.z += -1.5f;
            Instantiate(bulletEffect, vb, Quaternion.identity);
            int t = 0;
            t = int.Parse(gamemanager.instance.playerCounterGameplay.text);
            t += 1;
            gamemanager.instance.playerCounterGameplay.text = t.ToString();
            gamemanager.instance.plauerCounterWin.text = t.ToString();
            txt = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            if(!forOnce)
            {
                forOnce = true;
                number = int.Parse(txt.text);
                scaleToCut = (transform.localScale.x / 2) / number;
            }
            transform.localScale = new Vector3(transform.localScale.x - scaleToCut, transform.localScale.y, transform.localScale.z - scaleToCut);
            txt.text = (int.Parse(txt.text) - 1).ToString();
            if(int.Parse(txt.text)<=0)
            {
                Vector3 v = transform.position;
                v.y = 2f;
                Instantiate(EffectsColor[intMat], v, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }
            Destroy(other.gameObject);
        }
    }
}
