using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PhysicsTriggerListener : MonoBehaviour
{
    [HideInInspector] public GameEventArgs onenter;
    [HideInInspector] public GameEventArgs onstay;
    [HideInInspector] public GameEventArgs onexit;
}

[CustomEditor(typeof(PhysicsTriggerListener))]
public class PhysicsTriggerListenerEditor : Editor
{
    private enum PhysicsEvents
    {
        OnEnter,
        OnExit,
        OnStay
    }

    private PhysicsEvents selected;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        selected = (PhysicsEvents)EditorGUILayout.EnumPopup(label:"Add New Event", selected:selected);
        if (EditorGUILayout.DropdownButton(new GUIContent("add new event type"),  FocusType.Passive))
        {
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("OnEnter"),true,
                () => { gm.AddDisabledItem(new GUIContent("OnEnter")); });
            gm.ShowAsContext();
        }
        


    }
}