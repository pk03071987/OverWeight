using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playersController : MonoBehaviour
{
    float x;
    public float speed, speedForward;
    public bool gamerun, winlevel;
    public bool leftw, rightw;
    Vector3 a, currentspeed;
    GameObject endlvl;
    float levellong, startZ;
    public Image lvlbar;
    Vector3 direction, pressPos;
    public bool FrOnce;
    enemyCtr enemCtr;
    public bool starshooting;
    public float tryspeed;
    bool countAnim;
    public LayerMask layers;

    // Start is called before the first frame update
    void Start()
    {
        currentspeed = new Vector3(tryspeed, 0, 0);
        endlvl = GameObject.FindGameObjectWithTag("endlevel");
        startZ = transform.position.z;
        levellong = endlvl.transform.position.z - startZ;
        Debug.Log(levellong);
        enemCtr = GameObject.FindObjectOfType<enemyCtr>();
    }
    public void OnPlayGame()
    {
        SoundManager.instance.playVibration();
        gamerun = true;
        StartCoroutine(alertTogoal());
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("run");
        transform.GetChild(1).GetComponent<Animator>().SetTrigger("walk");
        gamemanager.instance.startpanel.SetActive(false);
        gamemanager.instance.hud.SetActive(true);
        UiManager.instance.currentLevelText.text = "LEVEL " + gamemanager.instance.getLevel();
    }
    float timer;
    // Update is called once per frame
    void Update()
    {
        if (UiManager.isPause)
            return;
        //if(transform.childCount-1>0 && transform.childCount-1> int.Parse(gamemanager.instance.playerCounterGameplay.text))
        //{
        //    gamemanager.instance.playerCounterGameplay.text = (transform.childCount-1).ToString();
        //    gamemanager.instance.plauerCounterWin.text = (transform.childCount-1).ToString();
        //}

        lvlbar.fillAmount = (transform.position.z - startZ) / levellong;
        
        if (transform.position.z - endlvl.transform.position.z > 3f)
        {
            winlevel = true;
        }

        if (transform.position.z - endlvl.transform.position.z >= 0f)
        {
            //if (!countAnim)
            //{
            //    gamemanager.instance.playerNum.text = transform.childCount.ToString();
            //    gamemanager.instance.enemyNum.text = enemCtr.gameObject.transform.childCount.ToString();
            //    gamemanager.instance.enemVsPlayer.SetActive(true);
            //}
        }

        if (Input.GetMouseButtonDown(0) && !gamerun)
        {
            //gamerun = true;
            //StartCoroutine(alertTogoal());
            //transform.GetChild(0).GetComponent<Animator>().SetTrigger("run");
            //transform.GetChild(1).GetComponent<Animator>().SetTrigger("walk");
            //gamemanager.instance.startpanel.SetActive(false);
            //gamemanager.instance.gamePlaypanel.SetActive(true);
        }
        if (gamerun && !winlevel && transform.childCount > 0)
        {
            timer += Time.deltaTime;
            Debug.Log("timer " + timer);
            //  transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speedForward * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y - speedForward * Time.deltaTime, transform.position.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            starshooting = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                x = hit.point.x;
                pressPos = hit.point;
            }


        }

        if (Input.GetMouseButtonUp(0))
        {
            starshooting = false;
        }

        if (Input.GetMouseButton(0) && transform.childCount > 0 && !winlevel)
        {



            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                direction = hit.point - pressPos;

                //Debug.Log("without//// " + Input.mousePosition.magnitude);
                //if (direction.magnitude < 2)
                //{
                //    return;
                //}


                if (hit.point.x > x && !rightw)
                {
                    pressPos = Input.mousePosition;


                    float vx = hit.point.x - x;
                    x = hit.point.x;


                    a = transform.position;
                    a.x += vx * 1.5f;
                    //a.x += direction.normalized.magnitude * (speed/2)*Time.deltaTime;
                    //transform.position = Vector3.Lerp(transform.position, a, speed);
                    //transform.Translate(Vector3.right * speed * Time.deltaTime);
                    transform.position = Vector3.SmoothDamp(transform.position, a, ref currentspeed, Time.deltaTime, speed * direction.normalized.magnitude);
                    //transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);

                }
                if (hit.point.x < x && !leftw)
                {
                    pressPos = Input.mousePosition;
                    float vx = x - hit.point.x;
                    x = hit.point.x;
                    a = transform.position;
                    a.x -= vx * 1.5f;
                    //a.x -= direction.normalized.magnitude * (speed / 2) * Time.deltaTime;
                    //transform.position = Vector3.Lerp(transform.position, a, speed);
                    //transform.Translate(Vector3.right * -speed * Time.deltaTime);
                    transform.position = Vector3.SmoothDamp(transform.position, a, ref currentspeed, Time.deltaTime, speed * direction.normalized.magnitude);
                    //transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                }


            }


            //if (Input.mousePosition.x > x && !rightw)
            //{
            //    float vx = Input.mousePosition.x - x;
            //    x = Input.mousePosition.x;
            //    a = transform.position;
            //    a.x += vx * Time.deltaTime;
            //    //a.x += direction.normalized.magnitude * (speed/2)*Time.deltaTime;
            //    //transform.position = Vector3.Lerp(transform.position, a, speed*Time.deltaTime);
            //    transform.Translate(Vector3.right * speed * Time.deltaTime);
            //    //transform.position = Vector3.SmoothDamp(transform.position, a,ref currentspeed, Time.deltaTime,speed,Time.deltaTime);
            //    //transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);

            //}
            //if (Input.mousePosition.x < x && !leftw)
            //{
            //    float vx = x - Input.mousePosition.x;
            //    x = Input.mousePosition.x;
            //    a = transform.position;
            //    a.x -= vx * Time.deltaTime;
            //    //a.x -= direction.normalized.magnitude * (speed / 2) * Time.deltaTime;
            //    //transform.position = Vector3.Lerp(transform.position, a, speed*Time.deltaTime);
            //    transform.Translate(Vector3.right * -speed * Time.deltaTime);
            //    //transform.position = Vector3.SmoothDamp(transform.position, a, ref currentspeed,Time.deltaTime, speed,Time.deltaTime);
            //    //transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            //}
        }



        if (winlevel && !FrOnce)
        {
            FrOnce = true;
            //StartCoroutine(playerlastfight());
        }

        if (transform.childCount == 0)
        {
            StartCoroutine(losegame());
        }

    }


    IEnumerator playerlastfight()
    {
        yield return new WaitForSeconds(0.8f);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i < enemCtr.gameObject.transform.childCount)
            {
                Debug.Log(i + " // " + enemCtr.gameObject.transform.childCount);
                GameObject gm = enemCtr.gameObject.transform.GetChild(i).gameObject;
                transform.GetChild(i).gameObject.GetComponent<soliderCtr>().onelastshoot(gm);

            }
            else if (i >= enemCtr.gameObject.transform.childCount)
            {
                Debug.Log("numb " + i);
                GameObject gm = enemCtr.gameObject.transform.GetChild(Random.Range(0, enemCtr.gameObject.transform.childCount - 1)).gameObject;
                transform.GetChild(i).gameObject.GetComponent<soliderCtr>().onelastshoot(gm);

            }
        }
    }

    IEnumerator losegame()
    {
        yield return new WaitForSeconds(1f);
        gamemanager.instance.losepanel.SetActive(true);
    }

    IEnumerator alertTogoal()
    {
        gamemanager.instance.messagepanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        gamemanager.instance.messagepanel.SetActive(false);

    }
}
