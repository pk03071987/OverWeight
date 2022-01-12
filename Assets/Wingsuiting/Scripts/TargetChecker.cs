using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetChecker : MonoBehaviour 
{
    public static int coins;
    [SerializeField]
    private int prize;
    private bool isChecked = false;
    private AudioSource hitAudio;

    void Awake ()
    {
        coins = 0;
        GameObject.Find("Coins").GetComponent<Text>().text = "Coins: " + coins;
        hitAudio = gameObject.GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (!isChecked)
        {
            isChecked = true;
            coins += prize;
            hitAudio.Play();
            GameObject.Find("Coins").GetComponent<Text>().text = "Coins: " + coins;
            StartCoroutine(WaitWhenPlayed ());
        }
    }
    IEnumerator WaitWhenPlayed ()
    {
        while (hitAudio.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
