using UnityEngine;
using System.Collections;

public class AviatorCamera : MonoBehaviour
{
    [SerializeField]
    private Transform aviator;
    private Vector3 deltaPosition;
    private Vector3 localPosition;
    private Vector3 position;
    [SerializeField]
    private float damp;
    private Transform _transform;
    [SerializeField]
    private JointsPoseController controller;
    private Vector3 aviatorPosition;

    void Awake ()
    {
        _transform = transform;
        deltaPosition = _transform.position - aviator.position;
        localPosition = VectorOperator.getLocalPosition(aviator, _transform.position);
        aviatorPosition = aviator.position;
	}
	
	void Update () 
    {
        if (controller.NewPoseName == "Rotate left" || controller.NewPoseName == "Rotate right" || controller.NewPoseName == "From Rotate left" || controller.NewPoseName == "From Rotate right"
            || controller.NewPoseName == "Left turn" || controller.NewPoseName == "Right turn")
        {
            position = VectorOperator.getWordPosition(aviator, localPosition);
        } else
        {
           position = deltaPosition + aviator.position;
        }
        //position = VectorOperator.getWordPosition(aviator, localPosition);
        if (controller.inAnimate)
        {
            _transform.position = Vector3.Lerp(_transform.position, position, 0.5f * damp * Time.deltaTime);
        } else
        {
            _transform.position = Vector3.Lerp(_transform.position, position, damp * Time.deltaTime);
        }


        aviatorPosition = Vector3.Lerp(aviatorPosition, aviator.position, 2.0f * damp * Time.deltaTime);
        _transform.LookAt(aviatorPosition);
	}
}
