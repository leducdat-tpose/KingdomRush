using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Editor{
    [CustomPropertyDrawer(typeof(TowerInfo))]
    public class TowerInfoPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            float labelWidth = 40;
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.BeginProperty(position, label, property);
            Rect levelRect = new Rect(position.x, position.y, position.width/10 + labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty levelProp = property.FindPropertyRelative("Level");
            Rect prefabRect = new Rect(levelRect.xMax, position.y, position.width/2.5f + labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty prefabProp = property.FindPropertyRelative("Prefab");
            Rect costRect = new Rect(prefabRect.xMax, position.y, position.width/10 + labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty costProp = property.FindPropertyRelative("Cost");
            Rect damageCauseRect = new Rect(position.x, levelRect.yMax + 3, position.width/10 + labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty damageCauseProp = property.FindPropertyRelative("DamageCause");
            Rect sellMoneyRect = new Rect(damageCauseRect.xMax, levelRect.yMax + 3, position.width/10 + labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty sellMoneyProp = property.FindPropertyRelative("SellMoney");
            Rect attackRangeRect = new Rect(sellMoneyRect.xMax, levelRect.yMax + 3, position.width/10 + labelWidth, EditorGUIUtility.singleLineHeight);
            SerializedProperty attackRangeProp = property.FindPropertyRelative("AttackRange");
            
            EditorGUI.PropertyField(levelRect, levelProp);
            EditorGUI.PropertyField(prefabRect, prefabProp);
            EditorGUI.PropertyField(costRect, costProp);
            EditorGUI.PropertyField(damageCauseRect, damageCauseProp);
            EditorGUI.PropertyField(sellMoneyRect, sellMoneyProp);
            EditorGUI.PropertyField(attackRangeRect, attackRangeProp);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2.5f;
        }
    }
}

