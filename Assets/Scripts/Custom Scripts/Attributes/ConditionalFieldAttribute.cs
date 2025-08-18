using UnityEngine;

/// <summary> јтрибут дл€ условного отображени€ полей в инспекторе </summary>
public class ConditionalFieldAttribute : PropertyAttribute
{
    /// <summary> »м€ пол€-услови€ </summary>
    public string ConditionProperty { get; }

    /// <summary> «начение, с которым сравниваем условие (опционально) </summary>
    public object CompareValue { get; }

    /// <param name="conditionProperty">ѕоле, от которого зависит видимость</param>
    /// <param name="compareValue">≈сли указано, поле покажетс€ только при совпадении значений</param>
    public ConditionalFieldAttribute(string conditionProperty, object compareValue = null)
    {
        ConditionProperty = conditionProperty;
        CompareValue = compareValue;
    }
}
