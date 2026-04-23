using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class GroupLabyrinthe
{
    [MenuItem("Outils/Grouper DecoBush_D dans Labyrinthe")]
    static void GrouperDecoBush()
    {
        List<GameObject> decoBushs = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go.scene.IsValid() && go.name.StartsWith("DecoBush_D"))
                decoBushs.Add(go);
        }

        if (decoBushs.Count == 0)
        {
            Debug.LogWarning("Aucun DecoBush_D trouvé dans la scène.");
            return;
        }

        GameObject labyrinthe = new GameObject("labyrinthe");
        Undo.RegisterCreatedObjectUndo(labyrinthe, "Créer labyrinthe");

        foreach (GameObject go in decoBushs)
        {
            Undo.SetTransformParent(go.transform, labyrinthe.transform, "Grouper dans labyrinthe");
        }

        EditorUtility.SetDirty(labyrinthe);
        Debug.Log($"{decoBushs.Count} DecoBush_D regroupés sous 'labyrinthe'.");
    }
}
