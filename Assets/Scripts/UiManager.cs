using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public Text LevelText, secondmessage;
    public bool skinEnter;
    public GameObject ingamepanel;
    public GameObject playerSelectionPanel;
    public GameObject startpanel, gameplaypanel, losepanel, winpanel;
    public GameObject settingPannel;
    public Text currentLevelText;
    public static bool isPause = false;
    public Sprite[] sp;
    private void Awake()
    {
        if (instance == null)
            instance = this;


    }
    // Start is called before the first frame update
    void Start()
    {
        //Advertisements.Instance.Initialize();
        //Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
        LevelText.text = "Level " + (gamemanager.instance.getLevel() + 1);
    }



    //public void skinmenu()
    //{
    //    // sound
    //    SoundManager.instance.Play("click");
    //    skinEnter = true;
    //    playerSelectionPanel.SetActive(true);
    //    ingamepanel.SetActive(false);
    //}

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
    public void OnClickPause(Image im)
    {
        isPause = isPause ? false : true;
        if (isPause)
            im.sprite = sp[0];
        else
            im.sprite = sp[1];
    }
}
