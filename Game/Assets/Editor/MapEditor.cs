using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(EditorViewer))]
public class MapEditor : Editor
{

	public override void OnInspectorGUI()
	{
		EditorViewer generator = (EditorViewer)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			generator.Generate();
		}

		if (GUILayout.Button("Generate Hexes"))
		{
			generator.GenerateHexes();
		}

		if (GUILayout.Button("Clear Hexes"))
		{
			generator.ClearHexes();
		}
	}
}
