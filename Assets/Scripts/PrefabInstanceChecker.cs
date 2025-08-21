using UnityEngine;

public class PrefabInstanceChecker : MonoBehaviour
{
    [SerializeField] private GameObject target; // ���� �������� DeleteIcon ��� ��� ������, � ������� ���������

    void Start()
    {
        if (target == null) target = gameObject; // ���� �� ������� - �������� ��� ���� ������

        // 1. �������� ����� Scene API
        if (string.IsNullOrEmpty(target.scene.name))
        {
            Debug.LogWarning($"[PrefabCheck] ������ '{target.name}' �� ����������� �� ����� ����� � ������, ��� ������-�����.");
        }
        else
        {
            Debug.Log($"[PrefabCheck] ������ '{target.name}' ��������� � ����� '{target.scene.name}' � ��� ������� � �����.");
        }

#if UNITY_EDITOR
        // 2. �������������� �������� � �������
        var prefabStatus = UnityEditor.PrefabUtility.GetPrefabAssetType(target);
        Debug.Log($"[PrefabCheck] PrefabUtility status: {prefabStatus}");
#endif
    }
}