using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneInjectionConfig", menuName = "Configs/Scene Injection Config")]
public class SceneInjectionConfig : ScriptableObject
{
    [Tooltip("Список сцен, в которых нужно делать реинжект")]
    public List<string> allowedScenes = new List<string>();
}