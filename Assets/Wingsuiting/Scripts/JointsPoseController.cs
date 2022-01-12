using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void PoseAnimationState(JointsPoseController controller);
public class JointsPoseController : MonoBehaviour
{
    public event PoseAnimationState OnStartAnimation;
    public event PoseAnimationState OnAnimationComplete;
    public event PoseAnimationState OnAnimation;
    public AviatorController controller;
	[SerializeField]
	public List<JointPose> Poses;
    [System.NonSerialized]
    public JointPose currentPose;
    [System.NonSerialized]
    public JointPose newPose;
    public List<Transform> Joints;
    public string NewPoseName = "T_Pose";
    public string CurrentPoseName = "T_Pose";
    public bool LockJoints;
    public bool UpdatePose;
    public float PoseTime01;
    public float UpdateSpeed = 1.0f;
    [System.NonSerialized]
    public bool inAnimate = false;
    private float lerpMaxTime;
    public float LerpTime
    {
        get{//Debug.Log(PoseTime01 + "    " + lerpMaxTime); 
            return lerpMaxTime;}
    }

    void Awake()
    {
        CurrentPoseName = "T_Pose";
        SetCurrentPose(CurrentPoseName);
        NewPoseName = CurrentPoseName;
        SetNewPose(CurrentPoseName);
        LerpPose(0.0f);
        controller.OnAwake();
    }
    void Update()
    {
        if (currentPose == null || newPose == null)
        {
            return;
        }
        if (PoseTime01 <= lerpMaxTime)
        {
            if(!inAnimate)
            {
                if(OnStartAnimation != null)
                {
                    OnStartAnimation(this);
                }
                inAnimate = true;
            }

            PoseTime01 += UpdateSpeed * Time.deltaTime;
            LerpPose(PoseTime01);
            if(OnAnimation != null)
            {
                OnAnimation(this);
            }
        } else if(inAnimate)
        {
            if( OnAnimationComplete != null )
            {
                LerpPose(lerpMaxTime);
                OnAnimationComplete(this);
            }
            inAnimate = false;
        }
    }
    public void SetPose(string poseName, float time01)
    {
        PoseTime01 = 0.0f;
        lerpMaxTime = time01;
        FixCurrentPose (poseName + "Fixed", Joints.ToArray());
        SetNewPose (poseName);
    }
    public void SetCurrentPose (string poseName)
    {
        if(Poses == null) {
            return;
        }
        foreach (var item in Poses) {
            if(item.name == poseName){
                  PoseTime01 = 0.0f;
                  CurrentPoseName = poseName;
                  currentPose = item;
            }
        }
    }
    public void SetNewPose (string poseName)
    {
        if(Poses == null) {
            return;
        }
        foreach (var item in Poses) {
            if(item.name == poseName){
                PoseTime01 = 0.0f;
                NewPoseName = poseName;
                newPose = item;
            }
        }
    }
    public void FixCurrentPose (string poseName, Transform[] joints)
    {
        currentPose = null;
        currentPose = new JointPose("Some in " + poseName, joints);
        PoseTime01 = 0.0f;
    }
    public void LerpPose (float time01)
    {
        if (currentPose != null && newPose != null)
        {
            for (int i = 0; i < newPose.positions.Length; i++)
            {
                Transform joint = Joints [i];
                joint.localPosition = Vector3.Lerp(currentPose.positions[i], newPose.positions [i], time01);
                joint.localRotation = Quaternion.Lerp(currentPose.rotations[i], newPose.rotations [i], time01);
            }
        }
    }
    public void AddPose (string poseName, Transform[] joints)
	{
		if(Poses == null) {
            Poses = new List<JointPose>(0);
		}
		foreach (var item in Poses) {
			if(item.name == poseName){
				return;
			}
		}
        Poses.Add(new JointPose(poseName, joints));
	}
    public void SavePose (string PoseName, Transform[] joints)
    {
        RemovePose(PoseName);
        AddPose(PoseName, joints);
    }
	public void RemovePose (string PoseName)
	{
		if(Poses == null) {
			return;
		}
		foreach (var item in Poses) {
			if(item.name == PoseName){
				Poses.Remove(item);
                if(currentPose == item){
                    currentPose = null;
                }
                if(newPose == item){
                    newPose = null;
                }
				return;
			}
		}
	}
	public void RemoveAllPoses ()
	{
		Poses = null;
        currentPose = null;
        newPose = null;
	}
}
[System.Serializable]
public class JointPose
{
    public string name;
    public Vector3[] positions;
    public Quaternion[] rotations;
    
    public JointPose (string name, Transform[] joints)
    {
        this.name = name;
        this.positions = new Vector3[joints.Length];
        this.rotations = new Quaternion[joints.Length];
        for (int i = 0; i < joints.Length; i++) {
            this.positions[i] = joints[i].localPosition;
            this.rotations[i] = joints[i].localRotation;
        }
    }
}