using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageLvl : MonoBehaviour
{
    public List<Transform> obstacles_pos_list;
    public List<Transform> players_pos_list;
    public List<Transform> enemies_pos_list;

    public int[] mainsize;



    public int number_of_obstacles;
    public int number_of_players;
    public int number_of_enemies;

    public GameObject obstacle_obj;
    public GameObject player_obj;
    public GameObject enemy_obj;
    public Transform enemy_parent;

    public int obstacles_text_numebers = 10;
    public Transform player;
    void Start()
    {
        instantiate_level();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void shuffle_list(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void instantiate_level()
    {
        if (gamemanager.instance.getLevel() < 2)
        {
            number_of_obstacles = Random.Range(30, 40);
            gamemanager.instance.sizegoal = mainsize[Random.Range(3, 7)];

        }
        if (gamemanager.instance.getLevel() >= 2 && gamemanager.instance.getLevel() < 7)
        {
            number_of_obstacles = Random.Range(40, 55);
            gamemanager.instance.sizegoal = mainsize[Random.Range(0, mainsize.Length)];
        }
        if (gamemanager.instance.getLevel() >= 7)
        {
            number_of_obstacles = Random.Range(60, 80);
            gamemanager.instance.sizegoal = mainsize[Random.Range(0, mainsize.Length)];
        }
        gamemanager.instance.goalsizetxt.text = gamemanager.instance.sizegoal.ToString();
        //shuffle list obstacles
        shuffle_list(obstacles_pos_list);

        //shuffle list players
        shuffle_list(players_pos_list);

        //inst obstacles
        for (int i = 0; i < number_of_obstacles; i++)
        {
            GameObject gm = Instantiate(obstacle_obj, obstacles_pos_list[i].transform.position, obstacle_obj.transform.rotation);
            //GameObject gm = Instantiate(obstacle_obj, new Vector3(0, player.transform.position.y-((i+1)*5),12), obstacle_obj.transform.rotation);
            int t = Random.Range(1, obstacles_text_numebers);
            //gm.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = t.ToString();
            float scaleTocut = (obstacles_text_numebers - t) * ((gm.transform.GetChild(0).localScale.x / 2) / obstacles_text_numebers);
            //Debug.Log(t + " scale size " + scaleTocut);
            gm.transform.GetChild(0).localScale = new Vector3(gm.transform.GetChild(0).localScale.x - scaleTocut, gm.transform.GetChild(0).localScale.y, gm.transform.GetChild(0).localScale.z - scaleTocut);
        }

        //inst players
        for (int i = 0; i < number_of_players; i++)
        {
            Instantiate(player_obj, players_pos_list[i].transform.position, obstacle_obj.transform.rotation);
        }

        //inst enemies
        for (int i = 0; i < number_of_enemies; i++)
        {
            Instantiate(enemy_obj, enemies_pos_list[i].transform.position, enemy_obj.transform.rotation, enemy_parent);
        }

    }
}
