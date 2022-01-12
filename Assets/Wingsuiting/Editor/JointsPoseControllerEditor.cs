using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(JointsPoseController))]
public class JointsPoseControllerEditor : Editor
{
    private JointsPoseController jsController;
    private int jointsCount;
    private int savedJointsCount;

    void OnEnable()
    {
        jsController = target as JointsPoseController;
        if (jsController.Joints == null)
        {
            jsController.Joints = new List<Transform>(0);
        }
        jointsCount = jsController.Joints.Count; 
        savedJointsCount = jointsCount;
    }

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            return;
        }
        jsController.LockJoints = EditorGUILayout.Toggle("Lock joints ", jsController.LockJoints);
        if (!jsController.LockJoints)
        {
            jointsCount = EditorGUILayout.IntField("Joints counts", jointsCount);
            if (GUILayout.Button("Set joint count"))
            {
                savedJointsCount = jointsCount;
            }
            EditorGUILayout.LabelField("\n");
            if (GUILayout.Button("Set up all childs"))
            {
                savedJointsCount = jointsCount = jsController.transform.childCount;
                jsController.Joints = new List<Transform>(0);
                for (int i = 0; i < jointsCount; i++) {
                    jsController.Joints.Add(jsController.transform.GetChild(i));
                }

            }
            EditorGUILayout.LabelField("\n");
            if (jsController.Joints.Count > savedJointsCount)
            {
                jsController.Joints.RemoveAt(savedJointsCount);
            }
            for (int i = 0; i < savedJointsCount; i++)
            {
                if (i < jsController.Joints.Count)
                {
                    jsController.Joints [i] = EditorGUILayout.ObjectField("Joint: " + i, jsController.Joints [i], typeof(Transform), true) as Transform;
                } else
                {
                    jsController.Joints.Add(null);
                    jsController.Joints [i] = EditorGUILayout.ObjectField("Joint: " + i, jsController.Joints [i], typeof(Transform), true) as Transform;
                }
            }
            return;
        }
        jsController.UpdatePose = EditorGUILayout.Toggle("Update pose ", jsController.UpdatePose);
       
        if (jsController.UpdatePose)
        {
            jsController.PoseTime01 = EditorGUILayout.Slider("Pose change time", jsController.PoseTime01, 0.0f, 1.0f);
            jsController.CurrentPoseName = EditorGUILayout.TextField("Current pose name", jsController.CurrentPoseName);
        }
        jsController.NewPoseName = EditorGUILayout.TextField("New pose name", jsController.NewPoseName);
        if (!jsController.UpdatePose)
        {
            if (GUILayout.Button("Add mew pose"))
            {
                while (jsController.Joints[jsController.Joints.Count - 1] == null)
                {
                    jsController.Joints.RemoveAt(jsController.Joints.Count - 1);
                }
                jsController.Joints.TrimExcess();
                jsController.AddPose(jsController.NewPoseName, jsController.Joints.ToArray());
            }
            if (GUILayout.Button("Save pose"))
            {
                while (jsController.Joints[jsController.Joints.Count - 1] == null)
                {
                    jsController.Joints.RemoveAt(jsController.Joints.Count - 1);
                }
                jsController.Joints.TrimExcess();
                jsController.SavePose(jsController.NewPoseName, jsController.Joints.ToArray());
            }

            if (GUILayout.Button("Remove pose"))
            {
                jsController.RemovePose(jsController.NewPoseName);
            }
            if (GUILayout.Button("Remove all poses"))
            {
                jsController.RemoveAllPoses();
            }
             EditorGUILayout.LabelField("Set new pose");
            if (jsController.Poses != null)
            {
                foreach (var item in jsController.Poses)
                {
                    if (item != null)
                    {
                        if (GUILayout.Button(item.name))
                        {
                            jsController.CurrentPoseName = item.name;
                            jsController.SetCurrentPose(item.name);
                            jsController.NewPoseName = item.name;
                            jsController.SetNewPose(item.name);
                            jsController.LerpPose(0.0f);
                        }
                    }
                }
            }
        } else
        {
            if (GUILayout.Button("Fix  pose"))
            {
                while (jsController.Joints[jsController.Joints.Count - 1] == null)
                {
                    jsController.Joints.RemoveAt(jsController.Joints.Count - 1);
                }
                jsController.Joints.TrimExcess();
                jsController.FixCurrentPose(jsController.NewPoseName, jsController.Joints.ToArray());
            }
        
            EditorGUILayout.LabelField("Set current pose");
            if (jsController.Poses != null)
            {
                foreach (var item in jsController.Poses)
                {
                    if (item != null)
                    {
                        if (GUILayout.Button(item.name))
                        {
                            jsController.CurrentPoseName = item.name;
                            jsController.SetCurrentPose(item.name);
                        }
                    }
                }
            }

            EditorGUILayout.LabelField("Set new pose");
            if (jsController.Poses != null)
            {
                foreach (var item in jsController.Poses)
                {
                    if (item != null)
                    {
                        if (GUILayout.Button(item.name))
                        {
                            jsController.NewPoseName = item.name;
                            jsController.SetNewPose(item.name);
                        }
                    }
                }
            }
            if (jsController.UpdatePose)
            {
                jsController.LerpPose(jsController.PoseTime01);
            }
        }
        EditorGUILayout.LabelField("\n");

        jsController.controller = EditorGUILayout.ObjectField("Aviator controller", jsController.controller, typeof(AviatorController), true) as AviatorController;
        if (jsController.controller)
        {
            if (GUILayout.Button("Set default rotations to controller"))
            {
                jsController.controller.SetDefaultRotations();
               // jsController.controller.
            }
        }

        EditorGUILayout.LabelField("\n");
    }
}
