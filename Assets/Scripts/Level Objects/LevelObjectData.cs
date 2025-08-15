using UnityEngine;

[CreateAssetMenu(menuName = "Game/Spawn Configuration")]
public class SpawnConfigSO : ScriptableObject
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;
        [Range(0, 100)]
        public float spawnWeight = 50f; // Шанс спавна относительно других объектов
    }

    public SpawnableObject[] spawnableObjects;
}
