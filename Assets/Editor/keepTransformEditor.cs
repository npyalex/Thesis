using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Transform))]
public class keepTransformEditor : UnityEditor.Editor
{
    public void DrawABetterInspector(Transform t)
    {
        // Replicate the standard transform inspector gui        
        EditorGUIUtility.labelWidth = 25;
        EditorGUIUtility.fieldWidth = 50;

        EditorGUI.indentLevel = 0;
        Vector3 position = EditorGUILayout.Vector3Field("Position", t.localPosition);
        Vector3 eulerAngles = EditorGUILayout.Vector3Field("Rotation", t.localEulerAngles);
        Vector3 scale = EditorGUILayout.Vector3Field("Scale", t.localScale);

        EditorGUIUtility.labelWidth = 0;
        EditorGUIUtility.fieldWidth = 0;

        if (GUI.changed)
        {
            Undo.RecordObject(t, "Transform Change");

            t.localPosition = FixIfNaN(position);
            t.localEulerAngles = FixIfNaN(eulerAngles);
            t.localScale = FixIfNaN(scale);
        }
    }

    private Vector3 FixIfNaN(Vector3 v)
    {
        if (float.IsNaN(v.x))
        {
            v.x = 0.0f;
        }
        if (float.IsNaN(v.y))
        {
            v.y = 0.0f;
        }
        if (float.IsNaN(v.z))
        {
            v.z = 0.0f;
        }
        return v;
    }

    public override void OnInspectorGUI()
    {
        Transform t = (Transform)target;

        DrawABetterInspector(t);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            SaveData(t.gameObject);
        }

        if (GUILayout.Button("Load"))
        {
            LoadData(t.gameObject);
        }
        EditorGUILayout.EndHorizontal();
    }

    string GetInstanceFileName(GameObject baseObject)
    {
        return System.IO.Path.GetTempPath() + baseObject.name + "_" + baseObject.GetInstanceID() + ".keepTransform.txt";
    }

    public void SaveData(GameObject baseObject)
    {
        List<string> saveData = new List<string>();

        saveData.Add(this.GetInstanceID().ToString());

        saveData.Add(baseObject.transform.localPosition.x.ToString());
        saveData.Add(baseObject.transform.localPosition.y.ToString());
        saveData.Add(baseObject.transform.localPosition.z.ToString());

        saveData.Add(baseObject.transform.localRotation.eulerAngles.x.ToString());
        saveData.Add(baseObject.transform.localRotation.eulerAngles.y.ToString());
        saveData.Add(baseObject.transform.localRotation.eulerAngles.z.ToString());

        saveData.Add(baseObject.transform.localScale.x.ToString());
        saveData.Add(baseObject.transform.localScale.y.ToString());
        saveData.Add(baseObject.transform.localScale.z.ToString());


        System.IO.File.WriteAllLines(GetInstanceFileName(baseObject), saveData.ToArray());
    }

    public void LoadData(GameObject baseObject)
    {
        string[] lines = System.IO.File.ReadAllLines(GetInstanceFileName(baseObject));
        if (lines.Length > 0)
        {
            baseObject.transform.localPosition = new Vector3(System.Convert.ToSingle(lines[1]), System.Convert.ToSingle(lines[2]), System.Convert.ToSingle(lines[3]));
            baseObject.transform.localRotation = Quaternion.Euler(System.Convert.ToSingle(lines[4]), System.Convert.ToSingle(lines[5]), System.Convert.ToSingle(lines[6]));
            baseObject.transform.localScale = new Vector3(System.Convert.ToSingle(lines[7]), System.Convert.ToSingle(lines[8]), System.Convert.ToSingle(lines[9]));
            System.IO.File.Delete(GetInstanceFileName(baseObject));
        }
    }
}
