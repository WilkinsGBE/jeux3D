using UnityEngine;

/// <summary>
/// Gère le compteur de clés et l'état de la sortie.
/// Placer ce script sur un GameObject vide dans la scène (ex: "GameManager").
/// </summary>
public class GestionnaireJeu : MonoBehaviour
{
    // Instance unique (singleton)
    public static GestionnaireJeu Instance { get; private set; }

    // Compteurs
    private static int clesTotales   = 0;
    private static int clesRamassees = 0;

    void Awake()
    {
        Instance = this;
        // Remet les compteurs à zéro au début de la partie
        clesTotales   = 0;
        clesRamassees = 0;
    }

    // -------------------------------------------------------
    // Appelé par KeyItem.Start() pour chaque clé dans la scène
    // -------------------------------------------------------
    public static void EnregistrerCle()
    {
        clesTotales++;
        Debug.Log($"[Labyrinthe] Clés dans la scène : {clesTotales}");
    }

    // -------------------------------------------------------
    // Appelé par KeyItem.OnTriggerEnter() quand le joueur ramasse une clé
    // -------------------------------------------------------
    public static void RamasserCle()
    {
        clesRamassees++;
        Debug.Log($"[Labyrinthe] Clés ramassées : {clesRamassees} / {clesTotales}");

        if (clesRamassees >= clesTotales)
        {
            Debug.Log("[Labyrinthe] Toutes les clés sont ramassées ! La sortie est ouverte.");
            MazeSortie.OuvrirSortie();
        }
    }

    // -------------------------------------------------------
    // Accesseurs utiles pour l'UI
    // -------------------------------------------------------
    public static int ClesRamassees => clesRamassees;
    public static int ClesTotales   => clesTotales;
}
