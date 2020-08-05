using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HexGenerator))]
public class HexEditor : Editor
{

	public override void OnInspectorGUI()
	{
		HexGenerator generator = (HexGenerator)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			generator.GenerateMap(Worlds.WorldCollection.Continent, 0);
		}

		if (GUILayout.Button("Clear Hexes"))
		{
			generator.ClearHexes();
		}
	}
}
