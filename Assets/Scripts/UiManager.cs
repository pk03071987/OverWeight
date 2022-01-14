using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public Text LevelText, errormessage;
    public bool skinEnter;
    public GameObject playerSelectionPanel;
    public GameObject startpanel, losepanel, winpanel, hud, messagepanel;
    public GameObject settingPannel;
    public Text currentLevelText;
    public static bool isPause = false;
    public GameObject pausePannel;
    public GameObject[] character;
    public GameObject[] bt;
    public GameObject[] selection;
    public Text Timer;
    DateTime perviousTime;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //  PlayerPrefs.DeleteAll();
        //  PlayerPrefs.SetInt("coin", 2000);

        LevelText.text = "Level " + (gamemanager.instance.getLevel() + 1);
        PlayerPrefs.SetInt("character0", 1);
        for (int i = 0; i < character.Length; i++)
        {
            int ch = PlayerPrefs.GetInt("character" + i);
            if (ch == 0)
            {
                bt[i].SetActive(true);
            }
            else
            {
                bt[i].SetActive(false);
            }
        }
    }

    public void btn_retry()
    {
        // sound

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextlvl()
    {
        gamemanager.instance.setLevel(gamemanager.instance.getLevel() + 1);
        if (gamemanager.instance.LevelsContenu.Length <= gamemanager.instance.getLevel())
            return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    bool isSetting;
    public Animator anim;
    public AnimationClip[] clip;
    public void OnClickSetting()
    {
        isSetting = isSetting ? false : true;
        //settingPannel.SetActive(isSetting);
        anim.enabled = true;
        if (isSetting)
        {
            anim.Play(clip[0].name);
        }
        else
        {
            anim.Play(clip[1].name);
        }
    }
    void Update()
    {
        RemainTimer();
    }
    public void OnClickPause()
    {
        isPause = isPause ? false : true;
        Debug.Log("is pause" + isPause);
        pausePannel.SetActive(isPause);
    }
    public void OnClickStore()
    {
        playerSelectionPanel.SetActive(true);
    }
    public void SelcetCharacter(int id)
    {
        bool isCharacterSelection = false;
        int ch = PlayerPrefs.GetInt("character" + id);
        if (id == 0)
        {
            isCharacterSelection = true;
            PlayerPrefs.SetInt("character", id);
        }
        if (id == 1)
        {
            if (ch == 0)
            {
                if (gamemanager.instance.getcoin() >= 200)
                {
                    PlayerPrefs.SetInt("coin", gamemanager.instance.getcoin() - 200);

                    PlayerPrefs.SetInt("character" + id, 1);
                    PlayerPrefs.SetInt("character", id);
                    isCharacterSelection = true;
                    bt[id].SetActive(false);
                }
                else
                {
                    StartCoroutine("ErrorMessage");
                    Debug.Log("not enough coins");
                }
            }
            else
            {
                PlayerPrefs.SetInt("character", id);
                isCharacterSelection = true;
            }
        }
        else if (id == 2)
        {
            if (ch == 0)
            {
                if (gamemanager.instance.getcoin() >= 500)
                {
                    PlayerPrefs.SetInt("coin", gamemanager.instance.getcoin() - 500);
                    PlayerPrefs.SetInt("character" + id, 1);
                    PlayerPrefs.SetInt("character", id);
                    isCharacterSelection = true;
                    bt[id].SetActive(false);
                }
                else
                {
                    StartCoroutine("ErrorMessage");
                    Debug.Log("not enough coins");
                }
            }
            else
            {
                PlayerPrefs.SetInt("character", id);
                isCharacterSelection = true;
            }
        }
        else if (id == 3)
        {
            if (ch == 0)
            {
                if (gamemanager.instance.getcoin() >= 700)
                {
                    PlayerPrefs.SetInt("coin", gamemanager.instance.getcoin() - 700);
                    PlayerPrefs.SetInt("character" + id, 1);
                    PlayerPrefs.SetInt("character", id);
                    isCharacterSelection = true;
                    bt[id].SetActive(false);
                }
                else
                {
                    StartCoroutine("ErrorMessage");
                    Debug.Log("not enough coins");
                }
            }
            else
            {
                PlayerPrefs.SetInt("character", id);
                isCharacterSelection = true;
            }
        }
        else if (id == 4)
        {
            if (ch == 0)
            {
                //if (gamemanager.instance.getcoin() >= 700)
                //{
                //    //PlayerPrefs.SetInt("coin", gamemanager.instance.getcoin() - 700);
                //    PlayerPrefs.SetInt("character" + id, 1);
                //    PlayerPrefs.SetInt("character", id);
                //    isCharacterSelection = true;
                //    bt[id].SetActive(false);
                //}
                //else
                //{
                //    StartCoroutine("ErrorMessage");
                //    Debug.Log("not enough coins");
                //}
            }
            else
            {
                PlayerPrefs.SetInt("character", id);
                isCharacterSelection = true;
            }
        }
        if (isCharacterSelection == true)
        {
            for (int i = 0; i < character.Length; i++)
            {
                if (i == id)
                {
                    character[i].SetActive(true);
                    selection[i].SetActive(true);
                }
                else
                {
                    character[i].SetActive(false);
                    selection[i].SetActive(false);
                }
            }
        }

    }
    IEnumerator ErrorMessage()
    {
        errormessage.enabled = true;
        yield return new WaitForSeconds(2);
        errormessage.enabled = false;
    }
    public void gotoHome()
    {
        isPause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    TimeSpan remingTime;
    void RemainTimer()
    {
        string pTime = PlayerPrefs.GetString("Timer");
        if (!string.IsNullOrEmpty(pTime))
        {
         //   perviousTime = DateTime.Parse(pTime);
            perviousTime = new DateTime(Convert.ToInt64(PlayerPrefs.GetString("Timer"))).AddHours(24);
            remingTime = perviousTime - DateTime.Now;
            if (remingTime.Hours <= 0 && remingTime.Minutes <= 0 && remingTime.Seconds <= 0)
            {
                PlayerPrefs.SetInt("character4", 1);
                PlayerPrefs.SetInt("character", 4);
                //isCharacterSelection = true;
                bt[4].SetActive(false);
            }
            else
            {
                Timer.text = remingTime.Hours + ":" + remingTime.Minutes + ":" + remingTime.Seconds;
            }
        }
    }
}
