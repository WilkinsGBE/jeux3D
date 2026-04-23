using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class GroupEntreeLabyrinthe
{
    [MenuItem("Outils/Grouper Entrée Labyrinthe")]
    static void Grouper()
    {
        string[] nomsCibles = { "Canvas", "BienvenuePanel", "Bienvenue-Wilkins" };

        List<GameObject> objets = new List<GameObject>();
        foreach (string nom in nomsCibles)
        {
            GameObject go = GameObject.Find(nom);
            if (go != null)
                objets.Add(go);
            else
                Debug.LogWarning($"Objet '{nom}' introuvable dans la scène.");
        }

        if (objets.Count == 0)
        {
            Debug.LogError("Aucun objet trouvé à grouper.");
            return;
        }

        GameObject parent = new GameObject("EntreeLabyrinthe");
        Undo.RegisterCreatedObjectUndo(parent, "Créer EntreeLabyrinthe");

        foreach (GameObject go in objets)
            Undo.SetTransformParent(go.transform, parent.transform, "Grouper dans EntreeLabyrinthe");

        Selection.activeGameObject = parent;
        EditorUtility.SetDirty(parent);
        Debug.Log($"{objets.Count} objet(s) regroupés sous 'EntreeLabyrinthe'. Positionne-le manuellement devant l'entrée.");
    }
}
