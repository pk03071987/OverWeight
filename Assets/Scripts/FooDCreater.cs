using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooDCreater : MonoBehaviour
{
    [SerializeField] bool continueFoodCreationLoop = false;
    [SerializeField] bool doCreateFood = false;
    [SerializeField] float delay = .1f;
    [SerializeField] float forwardDistance = 5f;
    [SerializeField] float minXDistance = 1f;
    [SerializeField] float maxXDistance = 1f;
    [SerializeField] float ydistance = 1f;
    [SerializeField] float scaleFactor = 1f;
    [SerializeField] GameObject[] foods;
    private void Start()
    {
        StartCoroutine(FoodCreationLoop());
    }

    IEnumerator FoodCreationLoop()
    {
        while(continueFoodCreationLoop)
        {
            if(doCreateFood)
            {
                GameObject g = NewFood();
                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }
    }
    
    float Xdistance
    {
        get
        {
            return Random.Range(minXDistance, maxXDistance) + Random.Range(-3f, 3f);
        }
    }
    public void StartFoodCreation()
    {
        doCreateFood = true;
    }
    GameObject NewFood()
    {
        GameObject g = Instantiate(foods[Random.Range(0, foods.Length)]);
        Vector3 v =transform.position+transform.forward * forwardDistance;
        v.x += Xdistance;
        v.y -= ydistance;
        g.transform.position = v;
        g.transform.localScale = Vector3.one * scaleFactor;
        return g;
    }
}
