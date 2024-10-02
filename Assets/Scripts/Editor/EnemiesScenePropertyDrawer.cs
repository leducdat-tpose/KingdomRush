using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(EnemiesInWave))]
    public class EnemiesScenePropertyDrawer:PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUIUtility.labelWidth = 60;
            EditorGUI.BeginProperty(position, label, property);
            Rect labelRect = new Rect(position.x, position.y, position.width,EditorGUIUtility.singleLineHeight);
            Rect enemyRect = new Rect(position.x, position.y, 2*position.width/3, EditorGUIUtility.singleLineHeight);
            SerializedProperty prefabProp = property.FindPropertyRelative("prefab");
            Rect amountRect = new Rect(position.x + 2*position.width/3, position.y, position.width/3, EditorGUIUtility.singleLineHeight);
            SerializedProperty amountProp = property.FindPropertyRelative("amount");
            EditorGUI.PropertyField(enemyRect, prefabProp);
            EditorGUI.PropertyField(amountRect, amountProp);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 1.2f;
        }
    }
}
