using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInjectionConfig", menuName = "Configs/Scene Injection Config")]
public class SceneInjectionConfig : ScriptableObject
{
    [Tooltip("������ ����, � ������� ����� ������ ��������")]
    public List<string> allowedScenes = new List<string>();
}