using UnityEngine;
using UnityEditor;

public static class SnapToTerrain
{
    [MenuItem("Tools/Snap Selected to Terrain")]
    static void SnapSelected()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogWarning("Aucun terrain actif dans la scène.");
            return;
        }

        Undo.RecordObjects(Selection.transforms, "Snap to Terrain");

        foreach (Transform t in Selection.transforms)
        {
            float y = terrain.SampleHeight(t.position) + terrain.transform.position.y;
            t.position = new Vector3(t.position.x, y, t.position.z);
        }

        Debug.Log($"{Selection.transforms.Length} objet(s) snappé(s) au terrain.");
    }
}
