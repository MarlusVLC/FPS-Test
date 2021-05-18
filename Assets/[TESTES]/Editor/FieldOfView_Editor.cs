using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfView_Editor : Editor
{
    private Vector3 _objectPos;
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView) target;
        Handles.color = Color.white;
        Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle/2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle / 2, false);
        
        if (fow.ShowCircumference)
        {
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);
        }
        else
        {
            Handles.DrawWireArc(fow.transform.position, Vector3.up, viewAngleA, fow.ViewAngle, fow.ViewRadius);
        }

        _objectPos = fow.transform.position;
        Handles.DrawLine(_objectPos, _objectPos + viewAngleA * fow.ViewRadius);
        Handles.DrawLine(_objectPos, _objectPos + viewAngleB * fow.ViewRadius);

        
        
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.VisibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
        }
    }
}
