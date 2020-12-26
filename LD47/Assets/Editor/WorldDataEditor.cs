using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Worldgeneration;

[CustomEditor(typeof(WorldData))]
public class WorldDataEditor : Editor
{
    private WorldData data;
    private SerializedProperty layers;
    private SerializedProperty pixelDefs;
    private bool colorDefinitionsFoldout = true;
    private IEnumerable<Color32> unusedColors;
    private (HashSet<string> duplicateNames, HashSet<Color32> duplicateColors) duplicates;

    private void OnEnable()
    {
        layers = serializedObject.FindProperty("worldLayers");
        pixelDefs = serializedObject.FindProperty("pixelDefinitions");
        data = (WorldData)target;
        data.OnValidation += Calculate;
    }

    private void OnDisable()
    {
        data.OnValidation -= Calculate;
    }

    private void Calculate()
    {
        unusedColors = GetUnusedColors(data);
        duplicates = GetDuplicates(data.pixelDefinitions);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        data = (WorldData)target;

        data.worldSize = EditorGUILayout.Vector2IntField("World Size", data.worldSize);

        EditorGUILayout.PropertyField(layers);
        
        colorDefinitionsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(colorDefinitionsFoldout, "Color Definitions");
        if (colorDefinitionsFoldout)
        {
            if (layers.arraySize > 0 && GUILayout.Button(new GUIContent("Update Colors", "Pixels with alpha equal to 0 are ignored.")))
            {
                RecalculateColors(data);
            }
            EditorGUILayout.PropertyField(pixelDefs);
            if (unusedColors != null && unusedColors.Count() > 0)
            {
                EditorGUILayout.HelpBox(Stringify("Unused Colors: ", unusedColors), MessageType.Warning);
            }
            if (duplicates.duplicateColors != null && duplicates.duplicateColors.Count > 0)
            {
                EditorGUILayout.HelpBox(Stringify("Duplicate Colors: ", duplicates.duplicateColors), MessageType.Error);
            }
            if (duplicates.duplicateNames != null && duplicates.duplicateNames.Count > 0)
            {
                EditorGUILayout.HelpBox(Stringify("Duplicate Names: ", duplicates.duplicateNames), MessageType.Error);
            }

        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }


    public void RecalculateColors(WorldData data)
    {
        var colors = GetAllPictureColors(data.worldLayers);
        var alreadyExistingColors = GetAllExistingColors(data.pixelDefinitions);

        foreach (var c in colors.Except(alreadyExistingColors))
        {
            data.pixelDefinitions.Add(new PixelDefinition("", c));
        }
    }

    private HashSet<Color32> GetAllPictureColors(List<WorldLayer> worldLayers)
    {
        HashSet<Color32> colors = new HashSet<Color32>();
        worldLayers.ForEach(wl => colors.UnionWith(wl.texture.GetPixels32().Where(c => c.a != 0)));
        return colors;
    }

    private HashSet<Color32> GetAllExistingColors(List<PixelDefinition> pixelDefinitions)
    {
        HashSet<Color32> alreadyExistingColors = new HashSet<Color32>();
        pixelDefinitions.ForEach(p => alreadyExistingColors.Add(p.color));
        return alreadyExistingColors;
    }

    private IEnumerable<Color32> GetUnusedColors(WorldData data) => GetAllExistingColors(data.pixelDefinitions).Except(GetAllPictureColors(data.worldLayers));

    private string Stringify<T>(string beginning, IEnumerable<T> things, string separator = " ")
    {
        string s = beginning;
        foreach (var c in things)
        {
            s += c + separator;
        }
        return s;
    }

    private (HashSet<string> duplicateNames, HashSet<Color32> duplicateColors) GetDuplicates(List<PixelDefinition> pixelDefinitions)
    {
        HashSet<string> duplicateNames = new HashSet<string>();
        HashSet<Color32> duplicateColors = new HashSet<Color32>();
        foreach (var def in pixelDefinitions)
        {
            foreach (var other in pixelDefinitions)
            {
                if (def == other)
                    continue;
                if (def.name == other.name)
                {
                    duplicateNames.Add(def.name);
                }
                if (def.color.Equals(other.color))
                {
                    duplicateColors.Add(def.color);
                }
            }
        }
        return (duplicateNames, duplicateColors);
    }
}
