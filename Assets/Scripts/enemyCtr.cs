using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class enemyCtr : MonoBehaviour
{
    playersController plrctr;
    public bool ForOnce;
    // Start is called before the first frame update
    void Start()
    {
        plrctr = GameObject.FindObjectOfType<playersController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(plrctr.winlevel && !ForOnce)
        {
            ForOnce = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                if(i< plrctr.gameObject.transform.childCount)
                {
                    Debug.Log(i + " // " + plrctr.gameObject.transform.childCount);
                    transform.GetChild(i).GetComponent<NavMeshAgent>().SetDestination(plrctr.gameObject.transform.GetChild(i).position);
                    transform.GetChild(i).GetComponent<Animator>().SetTrigger("run");
                    transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                else if(i >= plrctr.gameObject.transform.childCount)
                {
                    Debug.Log("numb " + i);
                    transform.GetChild(i).GetComponent<NavMeshAgent>().SetDestination(plrctr.gameObject.transform.GetChild(Random.Range(0, plrctr.gameObject.transform.childCount-1)).position);
                    transform.GetChild(i).GetComponent<Animator>().SetTrigger("run");
                    transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        if (transform.childCount == 0)
        {
            StartCoroutine(wingame());
        }
    }

    IEnumerator wingame()
    {
        if(int.Parse(gamemanager.instance.plauerCounterWin.text)>gamemanager.instance.getplayerCounter())
        {
            gamemanager.instance.setplayerCounter(int.Parse(gamemanager.instance.plauerCounterWin.text));
            gamemanager.instance.newbestTxt.gameObject.SetActive(true);
        }
        for (int i = 0; i < plrctr.gameObject.transform.childCount; i++)
        {
            plrctr.gameObject.transform.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("happy");
        }
        gamemanager.instance.effect1.SetActive(true);
        gamemanager.instance.effect2.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UiManager.instance.winpanel.SetActive(true);
    }
}
