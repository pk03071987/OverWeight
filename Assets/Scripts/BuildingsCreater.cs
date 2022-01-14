using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsCreater : MonoBehaviour
{
    [SerializeField] Transform playerObj;
    [SerializeField] GameObject[] buildingPrefabs;
    [SerializeField] List<GameObject> CreatedBuildings;
    [SerializeField] bool continueBuildingsCreation = false;
    [SerializeField] bool createBuilding = false;
    [SerializeField] float leftShift;
    [SerializeField] float rightShift;
    [SerializeField] float yLevel = 10f;
    [SerializeField] float yDecrement = 10f;
    [SerializeField] float delay =1f;
    void Start()
    {
        StartCoroutine(BuildingCreationLoop());
        StartCoroutine(CreateStartingBuildings());
    }


    public void OnStartGame()
    {
        createBuilding = true;
    }

    IEnumerator CreateStartingBuildings()
    {
        float zpos = 10f;
        float ypos = 10f;
        int i = 0;
        while(i<20)
        {
            i++;

            GameObject g = SideBuilding(zpos,ypos);
            g.SetActive(true);
            g = SideBuilding(zpos,ypos,false);
            g.SetActive(true);
            zpos += 40f;
            ypos += yDecrement;
            yield return null;
        }
    }
    IEnumerator BuildingCreationLoop()
    {
        while(continueBuildingsCreation)
        {
            if(createBuilding)
            {
                GameObject g = NewSideBuilding();
                g = NewSideBuilding(false);
                yLevel -= 10f;
                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }
   
    GameObject NewSideBuilding(bool isLeftSide=true)
    {
        GameObject g = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)]);
       Vector3 bPos = playerObj.position + playerObj.forward;
        if (isLeftSide) bPos.x += leftShift;
        else bPos.x += rightShift;
        bPos.y -= yLevel;
        g.transform.position = bPos;
        return g;
    }
    GameObject SideBuilding(float zpos,float ylevel_,bool isLeftSide = true)
    {
        GameObject g = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)]);
        Vector3 bPos = playerObj.position + playerObj.forward;
        if (isLeftSide) bPos.x += leftShift;
        else bPos.x += rightShift;
        bPos.y -= ylevel_;
        bPos.z += zpos;
        g.transform.position = bPos;
        return g;
    }
}
