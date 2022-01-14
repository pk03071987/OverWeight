using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using HomaGames.HomaBelly;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    public GameObject effect1, effect2, enemydieEffect, obstacleEffect, wineffect;
    public Text playerCounterGameplay, plauerCounterWin, newbestTxt, goalsizetxt;

    public GameObject[] LevelsContenu;
    public GameObject[] PlayersList;

    public int sizegoal;

    void Awake()
    {
        instance = this;
        onstartfirsttime();
    }

    // Start is called before the first frame update
    void Start()
    {
        instantiateLevel();
        //print(getLevel());
        //// game analytics
        //GmAnManager.instance.GA_game_start(getLevel() + 1);
        //// fb analytics
        //FbManager.instance.LogLevelStartedEvent(getLevel() + 1);
        ////homa games
        //HomaBelly.Instance.TrackProgressionEvent(ProgressionStatus.Start, "start level number " + (gamemanager.instance.getLevel() + 1));
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            resetall();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            setLevel(getLevel() + 1);
            if (LevelsContenu.Length <= getLevel())
                return;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void LevelToLoad()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void onstartfirsttime()
    {
        if (!PlayerPrefs.HasKey("firsttime"))
        {
            PlayerPrefs.SetInt("count", 1);
            PlayerPrefs.SetInt("coin", 0);
            PlayerPrefs.SetInt("level", 0);
            PlayerPrefs.SetInt("playerCounter", 0);
            PlayerPrefs.SetInt("firsttime", 0);
            PlayerPrefs.SetInt("pourcentage", 0);
            PlayerPrefs.SetInt("activSkin", 0);
            PlayerPrefs.SetInt("character0", 1);
            PlayerPrefs.SetInt("skin0", 1);
            string st = System.DateTime.Now.Ticks.ToString();
            PlayerPrefs.SetString("Timer", st);
            for (int i = 1; i < 9; i++)
            {
                PlayerPrefs.SetInt("skin" + i, 0);
            }
        }
    }

    // count Active skin
    public int getcountActive()
    {
        return PlayerPrefs.GetInt("count");
    }
    public void setcountActive(int nbr)
    {
        PlayerPrefs.SetInt("count", nbr);
    }

    // player counter
    public int getplayerCounter()
    {
        return PlayerPrefs.GetInt("playerCounter");
    }
    public void setplayerCounter(int nbr)
    {
        PlayerPrefs.SetInt("playerCounter", nbr);
    }


    // coin
    public int getcoin()
    {
        return PlayerPrefs.GetInt("coin");
    }
    public void setcoin(int nbr)
    {
        PlayerPrefs.SetInt("coin", nbr);
    }

    // menu active
    public int getMenuActive()
    {
        return PlayerPrefs.GetInt("menu");
    }
    public void setMenuActive(int nbr)
    {
        PlayerPrefs.SetInt("menu", nbr);
    }
    //pourcentage get set
    public void setpourcentage(int pourcentage)
    {
        PlayerPrefs.SetInt("pourcentage", pourcentage);
    }

    public int getpourcentage()
    {
        return PlayerPrefs.GetInt("pourcentage");
    }

    //skin variables

    public void setskin(int numSkin, int active)
    {
        PlayerPrefs.SetInt("skin" + numSkin, active);
    }

    public int getskin(int numSkin)
    {
        return PlayerPrefs.GetInt("skin" + numSkin);
    }
    public int Getchracter()
    {
        return PlayerPrefs.GetInt("character");
    }
    // active skin

    public void setactivSkin(int activSkin)
    {
        PlayerPrefs.SetInt("activSkin", activSkin);
    }

    public int getactivSkin()
    {
        return PlayerPrefs.GetInt("activSkin");
    }
    // level number

    public void setLevel(int nbr)
    {
        PlayerPrefs.SetInt("level", nbr);
    }

    public int getLevel()
    {
        return PlayerPrefs.GetInt("level");
    }

    // reset
    public void resetall()
    {
        PlayerPrefs.SetInt("count", 1);
        PlayerPrefs.SetInt("coin", 0);
        PlayerPrefs.DeleteKey("firsttime");
        PlayerPrefs.SetInt("pourcentage", 0);
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("skin0", 1);
        PlayerPrefs.SetInt("activSkin", 0);
        PlayerPrefs.SetInt("character0", 1);
        PlayerPrefs.SetInt("character1", 0);
        PlayerPrefs.SetInt("character2", 0);
        PlayerPrefs.SetInt("character3", 0);
        PlayerPrefs.SetInt("character4", 0);
        PlayerPrefs.SetInt("character", 0);
        PlayerPrefs.SetString("Timer", string.Empty);
        for (int i = 1; i < 9; i++)
        {
            PlayerPrefs.SetInt("skin" + i, 0);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void instantiateLevel()
    {
        if (LevelsContenu.Length > getLevel())
            //Instantiate(LevelsContenu[getLevel()]);
            LevelsContenu[getLevel()].SetActive(true);
        print(LevelsContenu.Length);
        print(getLevel());
    }
   
}
