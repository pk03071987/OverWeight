using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    GameObject playercContainer;
    Vector3 offset;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        playersController playercont = GameObject.FindObjectOfType<playersController>();
        if (playercont != null)
        {
            playercContainer = playercont.gameObject;
            offset = transform.position - playercContainer.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playercContainer != null)
            transform.position = Vector3.Lerp(transform.position, playercContainer.transform.position + offset, speed * Time.deltaTime);
    }
}
