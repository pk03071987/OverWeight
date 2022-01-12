using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour 
{
	public Transform target;
	public float distance = 10.0f;
	public float minDistance = 1.5f;
	public float maxDistance = 15f;
	
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20.0f;
	public float yMaxLimit = 80.0f;
	
	private float x = 0.0f;
	private float y = 0.0f;
	private bool control = true;
    private Vector3 position;

    void Awake()
    {
        if (Application.isMobilePlatform)
        {
            transform.parent = GameObject.Find("WingsuitAviator").transform;
            enabled = false;
        }
    }

	void Start () 
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		if (GetComponent<Rigidbody>())
		GetComponent<Rigidbody>().freezeRotation = true;
	}
	
	void LateUpdate () 
	{
        if (!target)
        {
            return;
        }
        control = !Input.GetKey(KeyCode.Space);		
       
		if (control)
        {
            
            x += Input.GetAxis("Mouse X") * xSpeed * 0.01f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.01f;

	 		
            y = ClampAngle(y, yMinLimit, yMaxLimit);
	 		       
	       
            Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
            position = rotation * (new Vector3(0.0f, 0.0f, -distance)) + target.position;


            transform.rotation = rotation;
            if (Vector3.Distance(transform.position, target.position) > 2.0f * distance)
            {
                transform.position = Vector3.Lerp(transform.position, position, 10.0f * Time.deltaTime);
            } else
            {
                transform.position = position;
            }

            distance += -55.0f * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
            if (distance < minDistance)
                distance = minDistance;
            else
			if (distance > maxDistance)
                distance = maxDistance;
        } else
        {
            if (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, 1.0f * Time.deltaTime);
            } else
            {
                transform.position = position;
            }
        }
	}
	
	static float ClampAngle (float angle, float min, float max) 
	{
		if (angle < -360.0f)
			angle += 360.0f;
		if (angle > 360.0f)
			angle -= 360.0f;
		return Mathf.Clamp (angle, min, max);
    }
}
