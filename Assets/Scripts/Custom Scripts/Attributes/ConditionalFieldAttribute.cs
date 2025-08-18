using UnityEngine;

/// <summary> ������� ��� ��������� ����������� ����� � ���������� </summary>
public class ConditionalFieldAttribute : PropertyAttribute
{
    /// <summary> ��� ����-������� </summary>
    public string ConditionProperty { get; }

    /// <summary> ��������, � ������� ���������� ������� (�����������) </summary>
    public object CompareValue { get; }

    /// <param name="conditionProperty">����, �� �������� ������� ���������</param>
    /// <param name="compareValue">���� �������, ���� ��������� ������ ��� ���������� ��������</param>
    public ConditionalFieldAttribute(string conditionProperty, object compareValue = null)
    {
        ConditionProperty = conditionProperty;
        CompareValue = compareValue;
    }
}
