using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Gyro : MonoBehaviour
{
    public bool isActive = false;
    [System.NonSerialized]
    public float controlStartPosition;

    void Update ()
    {
        if (!isActive)
        {
            isActive = Input.gyro.userAcceleration.magnitude > 0.075f;
            controlStartPosition = Input.gyro.gravity.y;
        }
    }
}
