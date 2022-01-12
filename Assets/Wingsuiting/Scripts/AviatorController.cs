using UnityEngine;
using System.Collections;

public class AviatorController : MonoBehaviour
{
    [SerializeField]
    private JointsPoseController posController;
    [SerializeField]
    private Transform root;

    [SerializeField]
    private JointsRandomAnimations[] joints;
    [SerializeField]
    private Transform
        hipSuit;
    private Transform[] hipSuitPoints;
    [SerializeField]
    private Transform[] armSuits;
    private Vector3[] hipSuitRotations;
    private Vector3[] armSuitRotations;
    [SerializeField]
    private Transform
        aviatorRoot;

    private float rotationY;
    private float velocityY;
    private float velocityZ;
    public Vector3 velocity
    {
        get;
        private set;
    }
    [SerializeField]
    private float suitFrequency;
    [SerializeField]
    private float suitMagnitude;
    private float time;
    [System.NonSerialized]
    public bool parachuteIsOpened = false;

    [SerializeField]
    private Transform parachute;
    private Vector3 parachuteStrSqale;
    private bool isMobilePlatform;

    public void OnAwake()
    {
        parachuteStrSqale = parachute.localScale;
        parachute.localScale = 0.01f * Vector3.one;
        parachute.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        hipSuitPoints = new Transform[hipSuit.childCount];
        hipSuitRotations = new Vector3[hipSuit.childCount];
        isMobilePlatform = Application.isMobilePlatform;
        for (int i = 0; i < hipSuitRotations.Length; i++)
        {
            hipSuitPoints [i] = hipSuit.GetChild(i);
            hipSuitRotations [i] = hipSuitPoints [i].localRotation.eulerAngles;
        }
        armSuitRotations = new Vector3[armSuits.Length];
        for (int i = 0; i < armSuitRotations.Length; i++)
        {
            armSuitRotations[i] = armSuits[i].localRotation.eulerAngles;
        }
        foreach (var item in joints)
        {
            item.OnStart();
        }
        posController.OnStartAnimation += (JointsPoseController controller) => 
        {
            if (controller.NewPoseName == "Open parachute")
            {
                parachute.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
        };
        posController.OnAnimationComplete += (JointsPoseController controller) => 
        {
            if (controller.NewPoseName == "Open parachute")
            {
                parachute.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            }
            SetCurrentState ();
        };
    }
    void SetCurrentState ()
    {
        foreach (var item in joints)
        {
            item.SetCurrentValue();
        }
    }
  
    void Update()
    {
        if (parachuteIsOpened)
        {
            if (parachute.localScale.magnitude < parachuteStrSqale.magnitude)
            {
                parachute.localScale *= 1.0f + 5.0f * Time.deltaTime;
            } else
            {
                parachute.localScale = parachuteStrSqale;
            }
        }
        time += Time.deltaTime;
        if (time > 100000.0f)
        {
            time = 0.0f;
        }
        for (int i = 0; i < hipSuitPoints.Length; i++)
        {
            hipSuitPoints [i].localRotation = Quaternion.Euler(hipSuitRotations[i] + suitMagnitude * Mathf.Sin(suitFrequency * time + ((float)i)) * (Mathf.PI / 2.0f) * Vector3.right);
        }
        for (int i = 0; i < armSuits.Length; i++)
        {
            armSuits[i].localRotation = Quaternion.Euler(armSuitRotations[i] + 0.75f * suitMagnitude * Mathf.Sin(suitFrequency * time + ((float)i)) * (Mathf.PI / 2.0f) * Vector3.right);
        }
       if (!posController.inAnimate)
        {
            foreach (var item in joints)
            {
                item.AnimateJoint();
            }
        } 
        VelocityControl();
        if (posController.NewPoseName == "Salto" || posController.NewPoseName == "From Salto")
        {
            velocity = velocityY * Vector3.up + velocityZ * VectorOperator.getProjectXZ( velocity.normalized, true);
        } else if(posController.NewPoseName == "Open parachute")
        {
            velocity =  velocityY * Vector3.up - velocityZ * root.up;
        }else
        {
            velocity =  velocityY * Vector3.up + velocityZ * root.forward;
        }
        velocity *= 3.0f;
        transform.position += velocity * Time.deltaTime;
        transform.Rotate(rotationY* Time.deltaTime * Vector3.up);

    }
    void VelocityControl()
    {
        if (posController.NewPoseName == "Stop n drop")
        {
            rotationY = 0.0f;
            velocityY = -4.0f;
            velocityZ = 10.0f;
        } else if (posController.NewPoseName == "Slow n hold")
        {
            rotationY = 0.0f;
            velocityY = -7.0f;
            velocityZ = 15.0f;
        } else if (posController.NewPoseName == "Energency stop")
        {
            rotationY = 0.0f;
            velocityY = -10.0f;
            velocityZ = 2.0f;
        } else if (posController.NewPoseName == "Open up")
        {
            rotationY = 0.0f;
            velocityY = -5.0f;
            velocityZ = 13.0f;
        } else if (posController.NewPoseName == "Squeeze")
        {
            rotationY = 0.0f;
            velocityY = -15.0f;
            velocityZ = 17.0f;
        } else if (posController.NewPoseName == "Proper kinesthetic")
        {
            rotationY = 0.0f;
            velocityY = -4.0f;
            velocityZ = 7.0f;
        } else if (posController.NewPoseName == "Backfly position 1")
        {
            rotationY = 0.0f;
            velocityY = -15.0f;
            velocityZ = 2.0f;
        } else if (posController.NewPoseName == "Backfly position 2")
        {
            rotationY = 0.0f;
            velocityY = -12.0f;
            velocityZ = 2.0f;
        } else if (posController.NewPoseName == "Backfly position 3")
        {
            rotationY = 0.0f;
            velocityY = -9.0f;
            velocityZ = 2.0f;
        } else if (posController.NewPoseName == "Right turn")
        {
            rotationY = 25.0f * posController.LerpTime;
            velocityY = -7.0f;
            velocityZ = 12.0f;
        } else if (posController.NewPoseName == "Left turn")
        {
            rotationY = -25.0f * posController.LerpTime;
            velocityY = -7.0f;
            velocityZ = 12.0f;
        } else if (posController.NewPoseName == "Salto")
        {
            rotationY = 0.0f;
            velocityY = -11.0f;
            velocityZ = 10.0f;
        } else if (posController.NewPoseName == "Rotate left")
        {
            rotationY = 0.5f;
            velocityY = -11.0f;
            velocityZ = 10.0f;
        } else if (posController.NewPoseName == "Rotate right")
        {
            rotationY = -0.5f;
            velocityY = -11.0f;
            velocityZ = 10.0f;
        } else if (posController.NewPoseName == "Open parachute")
        {
            float horizontal = 0.0f;
            if (isMobilePlatform)
            {
                horizontal = Mathf.Clamp(3.0f * Input.gyro.gravity.x, -1.0f, 1.0f);

                if (Mathf.Abs(horizontal) < 0.3f)
                {
                    horizontal = 0.0f;
                }
            } else
            {
                horizontal = Input.GetAxis("Horizontal");
            }
            rotationY = 10.0f * horizontal;
            velocityY = -1.5f;
            velocityZ = 2.0f;
        }
    }
    public void SetDefaultRotations()
    {
        foreach (var item in joints)
        {
            item.minValue = item.joint.localRotation.eulerAngles;
            item.maxValue = item.joint.localRotation.eulerAngles;
        }
    }
}
[System.Serializable]
public class JointsRandomAnimations
{
    public Transform joint;
    [System.NonSerialized]
    public float time;
    [System.NonSerialized]
    public Vector3 startValue;
    public Vector3 minValue;
    public Vector3 maxValue;
    private Vector3 currentValue;
    public float frequency = 1.0f;
    private Vector3 minValueDelta;
    private Vector3 maxValueDelta;

    public void OnStart()
    {
        startValue = joint.localRotation.eulerAngles;
        minValueDelta = minValue - startValue;
        maxValueDelta = maxValue - startValue;
        time = Random.Range(0.0f, 0.5f * Mathf.PI/frequency);
    }
    public void SetCurrentValue()
    {
        startValue = joint.localRotation.eulerAngles;
        currentValue = startValue;
        minValue = startValue + minValueDelta;
        maxValue = startValue + maxValueDelta;
    }
    public void AnimateJoint()
    {
        time += Time.deltaTime;
        if (time > 100000.0f)
        {
            time = 0.0f;
        }
        currentValue = Vector3.Lerp(minValue, maxValue, 0.5f + 0.5f * Mathf.Sin(frequency * time));
        joint.localRotation = Quaternion.Lerp( joint.localRotation, Quaternion.Euler(currentValue), 10.0f * Time.deltaTime);
    }
}
