using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(SpawnWave), false)]
public class SpawnWaveEditor : Editor
{
	SerializedProperty Type;
	SerializedProperty SpawnAmount;
	SerializedProperty SpawnInterval;
	SerializedProperty AmountPerLine;
	SerializedProperty OrientationSeed;
	SerializedProperty AmountPerEdgeSeed;
	SerializedProperty SpawnPosiSeed;
	SerializedProperty XSeed;
	SerializedProperty ZSeed;
	SerializedProperty EnemyPrefab;

	protected virtual void OnEnable ()
	{
		Type = serializedObject.FindProperty ("Type");
		SpawnAmount = serializedObject.FindProperty ("SpawnAmount");
		SpawnInterval = serializedObject.FindProperty ("SpawnInterval");
		AmountPerLine = serializedObject.FindProperty ("AmountPerLine");
		OrientationSeed = serializedObject.FindProperty ("OrientationSeed");
		AmountPerEdgeSeed = serializedObject.FindProperty ("AmountPerEdgeSeed");
		SpawnPosiSeed = serializedObject.FindProperty ("SpawnPosiSeed");
		XSeed = serializedObject.FindProperty ("XSeed");
		ZSeed = serializedObject.FindProperty ("ZSeed");
		EnemyPrefab = serializedObject.FindProperty ("EnemyPrefab");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		EditorGUILayout.PropertyField (Type, true);
		EditorGUILayout.PropertyField (SpawnAmount, true);
		EditorGUILayout.PropertyField (SpawnInterval, true);
		EditorGUILayout.PropertyField (EnemyPrefab,	true);

		SpawnType type = (SpawnType)Type.intValue;

		switch (type) {
		case SpawnType.Edge:
			EditorGUILayout.PropertyField (AmountPerEdgeSeed, true);
			EditorGUILayout.PropertyField (OrientationSeed, true);
			EditorGUILayout.PropertyField (SpawnPosiSeed, true);
			break;
		case SpawnType.Grid:
			EditorGUILayout.PropertyField (XSeed, true);
			EditorGUILayout.PropertyField (ZSeed, true);
			EditorGUILayout.PropertyField (AmountPerLine, true);
			break;
		case SpawnType.Line:
			EditorGUILayout.PropertyField (XSeed, true);
			EditorGUILayout.PropertyField (ZSeed, true);
			EditorGUILayout.PropertyField (OrientationSeed, true);
			EditorGUILayout.PropertyField (AmountPerLine, true);
			break;
		case SpawnType.Random:
			EditorGUILayout.PropertyField (XSeed, true);
			EditorGUILayout.PropertyField (ZSeed, true);
			break;
		}
		serializedObject.ApplyModifiedProperties ();
	}
}
