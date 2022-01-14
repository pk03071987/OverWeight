using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fatfitCtr : MonoBehaviour
{
    public int playerScore = 60;
    public float bodysizefat, bodysizefit;
    public SkinnedMeshRenderer dress, body;
    public Transform weightpos;
    public Transform look;
    public TextMeshProUGUI txt;
    public ParticleSystem[] eff;

    bool foroncetime;
    // Start is called before the first frame update
    void Start()
    {
        eff[0].Stop();
        eff[1].Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindObjectOfType<playersController>().winlevel)
        {
            if(Vector3.Distance(transform.parent.position,weightpos.position)>=0.1f)
            {
                GetComponent<Animator>().SetTrigger("walk");
                transform.LookAt(weightpos);
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, weightpos.position,5*Time.deltaTime);
            }
            else
            {
                transform.LookAt(look);
                GetComponent<Animator>().SetTrigger("idle");
                txt.text = playerScore.ToString();
                Vector3 v = new Vector3(0, 8, -1);
                Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, v, 5 * Time.deltaTime);
                StartCoroutine(fineshpart());
            }
            
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (GameObject.FindObjectOfType<playersController>().winlevel)
        {
            return;
        }
        if (collision.gameObject.tag == "fat")
        {
            Destroy(collision.gameObject);
            eff[Random.Range(0,eff.Length)].Play();
            if(body.GetBlendShapeWeight(0)<100 && body.GetBlendShapeWeight(1) == 0)
            {
                playerScore += 10;
                bodysizefat = body.GetBlendShapeWeight(0) + 25;
            }
            else if(body.GetBlendShapeWeight(1) > 0)
            {
                playerScore += 10;
                bodysizefit = body.GetBlendShapeWeight(1) - 25;
            }

            body.SetBlendShapeWeight(0, bodysizefat);
            dress.SetBlendShapeWeight(0, bodysizefat);
            body.SetBlendShapeWeight(1, bodysizefit);
            dress.SetBlendShapeWeight(1, bodysizefit);

        }

        if (collision.gameObject.tag == "diet")
        {
            Destroy(collision.gameObject);
            eff[Random.Range(0, eff.Length)].Play();

            if (body.GetBlendShapeWeight(1) < 100 && body.GetBlendShapeWeight(0) == 0)
            {
                playerScore -= 10;
                bodysizefit = body.GetBlendShapeWeight(1) + 25;
            }
            else if (body.GetBlendShapeWeight(0) > 0)
            {
                playerScore -= 10;
                bodysizefat = body.GetBlendShapeWeight(0) - 25;
            }
            body.SetBlendShapeWeight(0, bodysizefat);
            dress.SetBlendShapeWeight(0, bodysizefat);
            body.SetBlendShapeWeight(1, bodysizefit);
            dress.SetBlendShapeWeight(1, bodysizefit);
            gamemanager.instance.setcoin(gamemanager.instance.getcoin() + 10);
            SoundManager.instance.playVibration();
        }

        if (collision.gameObject.tag == "leftWall")
        {
            GameObject.FindObjectOfType<playersController>().leftw = true;
        }

        if (collision.gameObject.tag == "rightWall")
        {
            GameObject.FindObjectOfType<playersController>().rightw = true;
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

    IEnumerator fineshpart()
    {
        yield return new WaitForSeconds(1.5f);
        if (playerScore == gamemanager.instance.sizegoal)
        {
            gamemanager.instance.wineffect.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            if(!foroncetime)
            {
                foroncetime = true;
                //Advertisements.Instance.ShowInterstitial();
                UiManager.instance.winpanel.SetActive(true);
            }
            
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            if (!foroncetime)
            {
                foroncetime = true;
                //Advertisements.Instance.ShowInterstitial();
                UiManager.instance.losepanel.SetActive(true);
            }
            
        }
    }

}
