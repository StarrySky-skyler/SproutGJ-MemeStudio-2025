using UnityEditor;
using UnityEngine;

// �Զ������Ի�����
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property,
        GUIContent label)
    {
        // ���ֶ���Ϊֻ�������ñ༭
        GUI.enabled = false;

        // ����Ĭ�ϵ�GUI�ؼ�
        EditorGUI.PropertyField(position, property, label);

        // �ָ�GUI�Ľ�����
        GUI.enabled = true;
    }
}
