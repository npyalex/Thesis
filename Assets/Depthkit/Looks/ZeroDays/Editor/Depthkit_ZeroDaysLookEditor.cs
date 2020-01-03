/************************************************************************************

Depthkit Unity SDK License v1
Copyright 2016-2018 Scatter All Rights reserved.  

Licensed under the Scatter Software Development Kit License Agreement (the "License"); 
you may not use this SDK except in compliance with the License, 
which is provided at the time of installation or download, 
or which otherwise accompanies this software in either electronic or hard copy form.  

You may obtain a copy of the License at http://www.depthkit.tv/license-agreement-v1

Unless required by applicable law or agreed to in writing, 
the SDK distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License. 

************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Depthkit
{

    [CustomEditor(typeof(Depthkit_ZeroDaysLook))]
    [CanEditMultipleObjects]
    public class Depthkit_ZeroDaysLookEditor : Editor
    {
        SerializedProperty _meshDensityProp;
        SerializedProperty _selfOcclusionProp;
        Texture2D _logo;

        void OnEnable()
        {

            //set the property types
            _meshDensityProp = serializedObject.FindProperty("_meshDensity");
            _selfOcclusionProp = serializedObject.FindProperty("_selfOcclusion");

            _logo = Resources.Load("zerodays-logo-32", typeof(Texture2D)) as Texture2D;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            Depthkit_ZeroDaysLook renderer = (Depthkit_ZeroDaysLook)target;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            Rect rect = GUILayoutUtility.GetRect(_logo.width, _logo.height); GUI.DrawTexture(rect, _logo);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Zero Days Look - Version " + renderer.GetVersion(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("By Scatter for Zero Days VR");
            EditorGUILayout.LabelField("Based on the Participant Media");
            EditorGUILayout.LabelField("feature-length documentary Zero Days");
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

//            EditorGUILayout.BeginVertical();
//            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_selfOcclusionProp);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_meshDensityProp);
            if (EditorGUI.EndChangeCheck())
            {
                renderer.SetGeometryDirty();
            }


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Line Layer"))
            {
                renderer.AddLineLayer();
            }
            if (GUILayout.Button("Add Point Layer"))
            {
                renderer.AddPointLayer();
            }
            GUILayout.EndHorizontal();


            //APPLY PROPERTY MODIFICATIONS
            serializedObject.ApplyModifiedProperties();

        }
    }
}