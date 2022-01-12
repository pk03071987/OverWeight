using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soliderCtr : MonoBehaviour
{
    float i,j=0.30f;
    public GameObject bullet;
    public GameObject scndBullet;
    public bool activePlayer, winshoots;
    bool die;
    int numbershoot;
    public GameObject gmToFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(die)
        {
            transform.parent = null;
            activePlayer = false;
        }
        
        if (GameObject.FindObjectOfType<playersController>().gamerun && activePlayer && !GameObject.FindObjectOfType<playersController>().winlevel && GameObject.FindObjectOfType<playersController>().starshooting)
        {
            i -= Time.deltaTime;
            if (i <= 0)
            {
                i = 0.35f;
                Instantiate(bullet, transform.position, Quaternion.identity);
                
            }
        }

        if(winshoots)
        {
            j -= Time.deltaTime;
            if (j <= 0 && numbershoot<4)
            {
                numbershoot++;
                j = 0.30f;
                onelastshoot(gmToFollow);

            }
            
        }


        if(GameObject.FindObjectOfType<playersController>().winlevel)
        {
            GetComponent<Animator>().SetTrigger("idle");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GameObject.FindObjectOfType<playersController>().winlevel || transform.parent == null || die)
        {
            return;
        }
        if(collision.gameObject.tag =="Player" && transform.parent.GetComponent<playersController>()!=null && !die)
        {
            collision.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            collision.gameObject.GetComponent<soliderCtr>().activePlayer = true;
            collision.transform.parent = transform.parent;
            collision.gameObject.GetComponent<Animator>().SetTrigger("run");
            
        }



        if(collision.gameObject.tag=="obstacle")
        {
            die = true;
            transform.parent = null;
            activePlayer = false;
            GetComponent<Animator>().SetTrigger("die");

        }

        if(collision.gameObject.tag=="leftWall")
        {
            GameObject.FindObjectOfType<playersController>().leftw = true;
        }

        if (collision.gameObject.tag == "rightWall")
        {
            GameObject.FindObjectOfType<playersController>().rightw = true;
        }

        if(collision.gameObject.tag=="enemy")
        {
            //Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "leftWall")
        {
            GameObject.FindObjectOfType<playersController>().leftw = false;
        }

        if (collision.gameObject.tag == "rightWall")
        {
            GameObject.FindObjectOfType<playersController>().rightw = false;
        }
    }

    public void onelastshoot(GameObject g)
    {
        GameObject gm = Instantiate(scndBullet, transform.position, Quaternion.identity);
        gm.GetComponent<secondBullet>().ToFollow = g;
        gm.GetComponent<secondBullet>().startshoot = true;
        gmToFollow = g;
        winshoots = true;
    }


}
