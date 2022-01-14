using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playersController : MonoBehaviour
{
    float x;
    public float speed, downwardSpeed;
    public float forwardSpeed = 10f;
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
        int id = gamemanager.instance.Getchracter();
        for (int i = 0; i < UiManager.instance.character.Length; i++)
        {
            if (i == id)
            {
               UiManager.instance.character[i].SetActive(true);
               UiManager.instance.selection[i].SetActive(true);
            }
            else
            {
                UiManager.instance.character[i].SetActive(false);
                UiManager.instance.selection[i].SetActive(false);
            }
        }
    }
    public void OnPlayGame()
    {
        SoundManager.instance.playVibration();
        gamerun = true;
        StartCoroutine(alertTogoal());
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("run");
        transform.GetChild(1).GetComponent<Animator>().SetTrigger("walk");
        UiManager.instance.startpanel.SetActive(false);
        UiManager.instance.playerSelectionPanel.SetActive(false);
        UiManager.instance.hud.SetActive(true);
        UiManager.instance.currentLevelText.text = "LEVEL " + (gamemanager.instance.getLevel()+1);
    }
    float timer;
    // Update is called once per frame
    void Update()
    {
        if (UiManager.isPause)
            return;

        lvlbar.fillAmount = (transform.position.z - startZ) / levellong;

        if (transform.position.z - endlvl.transform.position.z > 3f)
        {
            winlevel = true;
        }

        if (transform.position.z - endlvl.transform.position.z >= 0f)
        {
          
        }

        if (Input.GetMouseButtonDown(0) && !gamerun)
        {
          
        }
        if (gamerun && !winlevel && transform.childCount > 0)
        {
<<<<<<< Updated upstream
            timer += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y - downwardSpeed * Time.deltaTime, transform.position.z+(forwardSpeed*Time.deltaTime));
=======
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speedForward * Time.deltaTime);
            //  transform.position = new Vector3(transform.position.x, transform.position.y - speedForward * Time.deltaTime, transform.position.z);
>>>>>>> Stashed changes
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
                if (hit.point.x > x && !rightw)
                {
                    pressPos = Input.mousePosition;
                    float vx = hit.point.x - x;
                    x = hit.point.x;
                    a = transform.position;
                    a.x += vx * 1.5f;
                    transform.position = Vector3.SmoothDamp(transform.position, a, ref currentspeed, Time.deltaTime, speed * direction.normalized.magnitude);
                }

                if (hit.point.x < x && !leftw)
                {
                    pressPos = Input.mousePosition;
                    float vx = x - hit.point.x;
                    x = hit.point.x;
                    a = transform.position;
                    a.x -= vx * 1.5f;
                    transform.position = Vector3.SmoothDamp(transform.position, a, ref currentspeed, Time.deltaTime, speed * direction.normalized.magnitude);
                }
            }
        }



        if (winlevel && !FrOnce)
        {
            FrOnce = true;
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
                GameObject gm = enemCtr.gameObject.transform.GetChild(i).gameObject;
                transform.GetChild(i).gameObject.GetComponent<soliderCtr>().onelastshoot(gm);

            }
            else if (i >= enemCtr.gameObject.transform.childCount)
            {
                GameObject gm = enemCtr.gameObject.transform.GetChild(Random.Range(0, enemCtr.gameObject.transform.childCount - 1)).gameObject;
                transform.GetChild(i).gameObject.GetComponent<soliderCtr>().onelastshoot(gm);

            }
        }
    }

    IEnumerator losegame()
    {
        yield return new WaitForSeconds(1f);
        UiManager.instance.losepanel.SetActive(true);
    }

    IEnumerator alertTogoal()
    {
        UiManager.instance.messagepanel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UiManager.instance.messagepanel.SetActive(false);

    }
}
