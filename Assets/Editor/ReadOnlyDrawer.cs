using UnityEditor;
using UnityEngine;

// 自定义属性绘制器
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 将字段设为只读，禁用编辑
        GUI.enabled = false;

        // 绘制默认的GUI控件
        EditorGUI.PropertyField(position, property, label);

        // 恢复GUI的交互性
        GUI.enabled = true;
    }
}
