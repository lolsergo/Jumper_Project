#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary> ��������� �������� ��� ������������� ����������� ����� </summary>
[CustomEditor(typeof(MonoViewBinder))]
public class MonoViewBinderEditor : Editor
{
    /// <summary> ������������ ��������� � ��������� ������ </summary>
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawBindingSection("View", "viewBinding", "view", "viewType", "viewId");
        EditorGUILayout.Space(10);
        DrawBindingSection("ViewModel", "viewModelBinding", "viewModel", "viewModelType", "viewModelId");
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary> ������ ������ �������� (View ��� ViewModel) </summary>
    private void DrawBindingSection(
        string header,
        string bindingModeProp,
        string instanceProp,
        string typeProp,
        string idProp
    )
    {
        EditorGUILayout.LabelField(header, EditorStyles.boldLabel);

        // ���� ������ ������
        var bindingMode = serializedObject.FindProperty(bindingModeProp);
        EditorGUILayout.PropertyField(bindingMode);

        // �������� ����
        var mode = (MonoViewBinder.BindingMode)bindingMode.enumValueIndex;
        switch (mode)
        {
            case MonoViewBinder.BindingMode.FromInstance:
                EditorGUILayout.PropertyField(serializedObject.FindProperty(instanceProp));
                break;

            case MonoViewBinder.BindingMode.FromResolve:
                EditorGUILayout.PropertyField(serializedObject.FindProperty(typeProp));
                break;

            case MonoViewBinder.BindingMode.FromResolveId:
                EditorGUILayout.PropertyField(serializedObject.FindProperty(typeProp));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(idProp));
                break;
        }
    }
}
#endif
