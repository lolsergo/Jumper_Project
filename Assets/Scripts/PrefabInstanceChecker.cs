using UnityEngine;

public class PrefabInstanceChecker : MonoBehaviour
{
    [SerializeField] private GameObject target; // сюда перетащи DeleteIcon или сам объект, с которым работаешь

    void Start()
    {
        if (target == null) target = gameObject; // если не указали - проверим сам этот объект

        // 1. Проверка через Scene API
        if (string.IsNullOrEmpty(target.scene.name))
        {
            Debug.LogWarning($"[PrefabCheck] Объект '{target.name}' НЕ принадлежит ни одной сцене — похоже, это ПРЕФАБ-АССЕТ.");
        }
        else
        {
            Debug.Log($"[PrefabCheck] Объект '{target.name}' находится в сцене '{target.scene.name}' — это ИНСТАНС в сцене.");
        }

#if UNITY_EDITOR
        // 2. Дополнительная проверка в эдиторе
        var prefabStatus = UnityEditor.PrefabUtility.GetPrefabAssetType(target);
        Debug.Log($"[PrefabCheck] PrefabUtility status: {prefabStatus}");
#endif
    }
}