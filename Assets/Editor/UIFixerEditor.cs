using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIFixer))]
public class UIFixerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UIFixer myTarget = (UIFixer)target;
        if (GUILayout.Button("Fix"))
        {
            myTarget.CalulateSize();
        }
        if (GUILayout.Button("Lock"))
        {
            myTarget.Lock();
        }
    }

}
