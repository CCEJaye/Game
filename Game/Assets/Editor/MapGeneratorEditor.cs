using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{

	public override void OnInspectorGUI()
	{
		MapGenerator generator = (MapGenerator)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			generator.Generate();
		}

		if (GUILayout.Button("Clear"))
		{
			generator.Clear();
		}
	}
}
