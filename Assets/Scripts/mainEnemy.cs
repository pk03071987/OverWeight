using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class mainEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    playersController plrctr;
    GameObject playerToFollow;
    bool secondkill, goAgain,stopmoving;
    int numbershot;

    public GameObject bulletEffect;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        plrctr = GameObject.FindObjectOfType<playersController>();
    }

    // Update is called once per frame
    void Update()
    {
       if((plrctr.winlevel && agent.remainingDistance<=1.5f && !stopmoving) || (plrctr.winlevel && goAgain && !stopmoving))
       {
            goAgain = false;
            
            Debug.Log("gooooooooooooooood");
            if(plrctr.gameObject.transform.childCount>0)
            {
                secondkill = true;
                playerToFollow = plrctr.gameObject.transform.GetChild(Random.Range(0, plrctr.gameObject.transform.childCount)).gameObject;
                agent.SetDestination(playerToFollow.transform.position);
                GetComponent<Animator>().SetTrigger("run");
            }
       }

       if(playerToFollow==null && secondkill && plrctr.gameObject.transform.childCount > 0 && !stopmoving)
       {
            playerToFollow = plrctr.gameObject.transform.GetChild(Random.Range(0, plrctr.gameObject.transform.childCount)).gameObject;
            agent.SetDestination(playerToFollow.transform.position);
            GetComponent<Animator>().SetTrigger("run");
        }

        if (plrctr.gameObject.transform.childCount == 0)
        {
            stopmoving = true;
            
            agent.isStopped = true;
            StartCoroutine(backToIdle());
        }


        for (int i = 0; i < plrctr.gameObject.transform.childCount; i++)
        {
            if (plrctr.gameObject.transform.GetChild(i).gameObject != null)
            {
                if(Vector3.Distance(transform.position, plrctr.gameObject.transform.GetChild(i).position)<1.3f)
                {
                    StartCoroutine(stopkill());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "scdBullet")
        {
            Vector3 vb = other.gameObject.transform.position;
            vb.z += -1f;
            vb.y += 1;
            Instantiate(bulletEffect, vb, Quaternion.identity);
            numbershot++;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Debug.Log("buuuuuuuuuuuuulet");
            if(numbershot>4)
            {
                Vector3 v = transform.position;
                v.y += 1f;
                Instantiate(gamemanager.instance.enemydieEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            agent.isStopped = true;
            stopmoving = true;
            GetComponent<Animator>().SetTrigger("hit");
            StartCoroutine(killAnim(collision.gameObject));
            
        }

        
    }


    IEnumerator killAnim(GameObject gm)
    {
        yield return new WaitForSeconds(0.15f);
        gm.GetComponent<Animator>().SetTrigger("die");
        gm.transform.parent = null;
        //Destroy(gm);
        yield return new WaitForSeconds(0.25f);
        
        goAgain = true;
        stopmoving = false;
        agent.isStopped = false;
    }

    IEnumerator stopkill()
    {
        agent.isStopped = true;
        GetComponent<Animator>().SetTrigger("hit");
        yield return new WaitForSeconds(0.5f);
        agent.isStopped = false;
        GetComponent<Animator>().SetTrigger("run");

    }

    IEnumerator backToIdle()
    {
        yield return new WaitForSeconds(0.35f);
        GetComponent<Animator>().SetTrigger("idle");

    }
}
