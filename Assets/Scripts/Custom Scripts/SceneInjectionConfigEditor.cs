//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//[CustomEditor(typeof(SceneInjectionConfig))]
//public class SceneInjectionConfigEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        var config = (SceneInjectionConfig)target;
//        var scenesInBuild = EditorBuildSettings.scenes;

//        EditorGUILayout.LabelField("—цены из Build Settings:", EditorStyles.boldLabel);

//        for (int i = 0; i < scenesInBuild.Length; i++)
//        {
//            var path = scenesInBuild[i].path;
//            var sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
//            bool selected = config.allowedScenes.Contains(sceneName);

//            bool toggle = EditorGUILayout.ToggleLeft(sceneName, selected);
//            if (toggle && !selected)
//                config.allowedScenes.Add(sceneName);
//            else if (!toggle && selected)
//                config.allowedScenes.Remove(sceneName);
//        }

//        serializedObject.ApplyModifiedProperties();
//        if (GUI.changed)
//        {
//            EditorUtility.SetDirty(config);
//        }
//    }
//}
//#endif
