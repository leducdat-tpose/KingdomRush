using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
public enum AttackType{
    None,
    Melee,
    Ranged,
}
public class CreatureData : ScriptableObject {
    public float MaxHealth;
    [Header("Move Section")]
    public bool CanMove = false;
    public float MoveSpeed;
    [Header("Attack Section")]
    public AttackType AttackType;
    public float Damage;
    public float CoolDownAttack;
    public GameObject ProjectilePrefab;
}

//true in CustomEditor to apply the custom in derived class from CreatureData
// [CustomEditor(typeof(CreatureData), true)]
// public class CreatureDataEditor : Editor {
//     SerializedProperty canMoveProp;
//     SerializedProperty moveSpeedProp;

//     SerializedProperty attackTypeProp;
//     SerializedProperty damageProp;
//     SerializedProperty coolDownAttackProp;
//     SerializedProperty projectileProp;
    
//     private void OnEnable() {
//         canMoveProp = serializedObject.FindProperty("CanMove");
//         moveSpeedProp = serializedObject.FindProperty("MoveSpeed");

//         attackTypeProp = serializedObject.FindProperty("AttackType");
//         damageProp = serializedObject.FindProperty("Damage");
//         coolDownAttackProp = serializedObject.FindProperty("CoolDownAttack");
//         projectileProp = serializedObject.FindProperty("ProjectilePrefab");
//     }

//     public override void OnInspectorGUI() {
//         serializedObject.Update();

//         EditorGUI.BeginChangeCheck();
//         DrawPropertiesExcluding(serializedObject, "Damage", "CoolDownAttack", "ProjectilePrefab");
//         // if (canMoveProp.boolValue && canMoveProp != null) {
//         //     EditorGUILayout.PropertyField(moveSpeedProp);
//         // }

//         if (EditorGUI.EndChangeCheck()) {
//             serializedObject.ApplyModifiedProperties();
//         }
//     }
// }