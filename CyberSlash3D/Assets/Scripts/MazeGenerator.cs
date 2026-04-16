using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Génère un labyrinthe de haies avec des clés cachées dans les culs-de-sac.
/// Algorithme : Depth-First Search (backtracking récursif).
/// </summary>
[ExecuteAlways]
public class MazeGenerator : MonoBehaviour
{
    // -------------------------------------------------------
    // Paramètres du labyrinthe
    // -------------------------------------------------------

    [Header("Labyrinthe")]
    [Range(4, 20)] public int largeur  = 8;
    [Range(4, 20)] public int hauteur  = 8;
    [Min(1f)]      public float tailleCellule = 3f;
    [Tooltip("0 = aléatoire à chaque génération")]
    public int graine = 0;

    // -------------------------------------------------------
    // Paramètres des buissons (murs)
    // -------------------------------------------------------

    [Header("Buissons (murs)")]
    [Tooltip("Glisse ici DecoBush_A, B, C et D")]
    public GameObject[] prefabsBuissons;

    [Min(0.3f)] public float espacementBuissons = 0.9f;
    public Vector2 echelleAleatoire = new Vector2(2f, 2.8f);

    [Tooltip("Nombre de rangées côte à côte → épaisseur du mur")]
    [Range(1, 3)] public int nbRangees = 2;
    [Min(0.1f)]   public float espacementRangees = 0.6f;

    [Tooltip("Nombre de couches empilées → hauteur du mur")]
    [Range(1, 4)] public int nbCouches = 3;
    [Min(0.1f)]   public float hauteurCouche = 0.8f;

    [Min(0f)] public float jitterAngle = 30f;
    [Min(0f)] public float jitterCote  = 0.1f;

    [Tooltip("Aligne automatiquement les buissons sur la hauteur du terrain")]
    public bool snapTerrain = true;

    // -------------------------------------------------------
    // Paramètres des clés
    // -------------------------------------------------------

    [Header("Clés")]
    [Tooltip("Prefab de la clé à ramasser")]
    public GameObject prefabCle;
    [Range(1, 5)] public int nombreCles = 3;
    [Min(0f)]     public float hauteurCle = 0.8f;

    // -------------------------------------------------------
    // Paramètres de la sortie
    // -------------------------------------------------------

    [Header("Sortie")]
    [Tooltip("Prefab du marqueur de sortie (optionnel)")]
    public GameObject prefabSortie;
    [Min(0f)] public float hauteurSortie = 0f;

    // -------------------------------------------------------
    // Interne
    // -------------------------------------------------------

    [Header("Racine générée (auto)")]
    [SerializeField] private Transform racine;

    // mursH[x, z] = mur horizontal à la ligne z, cellule x (le long de X)
    private bool[,] mursH;
    // mursV[x, z] = mur vertical à la ligne x, cellule z (le long de Z)
    private bool[,] mursV;
    private System.Random rng;

    // -------------------------------------------------------
    // Points d'entrée (menu contextuel Unity)
    // -------------------------------------------------------

    [ContextMenu("Générer le labyrinthe")]
    public void Generer()
    {
        AssurerRacine();
        ViderRacine();

        rng = (graine == 0)
            ? new System.Random(System.Environment.TickCount)
            : new System.Random(graine);

        InitialiserMurs();
        GenererDFS();
        OuvrirEntreeSortie();
        ConstruireMurs();
        PlacerCles();
        PlacerSortie();
    }

    [ContextMenu("Vider le labyrinthe")]
    public void Vider()
    {
        AssurerRacine();
        ViderRacine();
    }

    // -------------------------------------------------------
    // Gestion de la racine
    // -------------------------------------------------------

    void AssurerRacine()
    {
        if (racine != null) return;
        var existant = transform.Find("__LABYRINTHE__");
        if (existant != null) { racine = existant; return; }
        var go = new GameObject("__LABYRINTHE__");
        go.transform.SetParent(transform, false);
        racine = go.transform;
    }

    void ViderRacine()
    {
        if (racine == null) return;
        for (int i = racine.childCount - 1; i >= 0; i--)
        {
            var enfant = racine.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(enfant);
            else DestroyImmediate(enfant);
        }
    }

    // -------------------------------------------------------
    // Initialisation de la grille de murs
    // -------------------------------------------------------

    void InitialiserMurs()
    {
        // Au départ, toutes les cellules sont entourées de murs
        mursH = new bool[largeur, hauteur + 1];
        mursV = new bool[largeur + 1, hauteur];

        for (int x = 0; x < largeur; x++)
            for (int z = 0; z <= hauteur; z++)
                mursH[x, z] = true;

        for (int x = 0; x <= largeur; x++)
            for (int z = 0; z < hauteur; z++)
                mursV[x, z] = true;
    }

    // -------------------------------------------------------
    // Algorithme DFS : creuse des passages dans la grille
    // -------------------------------------------------------

    void GenererDFS()
    {
        var visite = new bool[largeur, hauteur];
        var pile   = new Stack<Vector2Int>();

        // Démarre depuis la cellule (0, 0)
        visite[0, 0] = true;
        pile.Push(Vector2Int.zero);

        while (pile.Count > 0)
        {
            var cellule = pile.Peek();
            var voisins = VoisinsNonVisites(cellule, visite);

            if (voisins.Count == 0)
            {
                pile.Pop(); // cul-de-sac → on revient en arrière
                continue;
            }

            // Choisit un voisin au hasard et supprime le mur entre eux
            var suivant = voisins[rng.Next(voisins.Count)];
            SupprimerMur(cellule, suivant);
            visite[suivant.x, suivant.y] = true;
            pile.Push(suivant);
        }
    }

    List<Vector2Int> VoisinsNonVisites(Vector2Int c, bool[,] visite)
    {
        var liste = new List<Vector2Int>(4);
        if (c.x > 0          && !visite[c.x - 1, c.y]) liste.Add(new Vector2Int(c.x - 1, c.y));
        if (c.x < largeur-1  && !visite[c.x + 1, c.y]) liste.Add(new Vector2Int(c.x + 1, c.y));
        if (c.y > 0          && !visite[c.x, c.y - 1]) liste.Add(new Vector2Int(c.x, c.y - 1));
        if (c.y < hauteur-1  && !visite[c.x, c.y + 1]) liste.Add(new Vector2Int(c.x, c.y + 1));
        return liste;
    }

    void SupprimerMur(Vector2Int a, Vector2Int b)
    {
        int dx = b.x - a.x;
        int dz = b.y - a.y;
        if      (dx ==  1) mursV[a.x + 1, a.y] = false; // mur Est de a
        else if (dx == -1) mursV[a.x,     a.y] = false; // mur Ouest de a
        else if (dz ==  1) mursH[a.x, a.y + 1] = false; // mur Nord de a
        else if (dz == -1) mursH[a.x, a.y    ] = false; // mur Sud de a
    }

    // -------------------------------------------------------
    // Entrée (sud) et sortie (nord)
    // -------------------------------------------------------

    void OuvrirEntreeSortie()
    {
        mursH[0,          0      ] = false; // Entrée : mur sud de (0,0)
        mursH[largeur - 1, hauteur] = false; // Sortie : mur nord de (largeur-1, hauteur-1)
    }

    // -------------------------------------------------------
    // Construction des murs en buissons
    // -------------------------------------------------------

    void ConstruireMurs()
    {
        if (prefabsBuissons == null || prefabsBuissons.Length == 0)
        {
            Debug.LogWarning("MazeGenerator : aucun prefab de buisson assigné !");
            return;
        }

        // Murs horizontaux (segments le long de l'axe X)
        for (int x = 0; x < largeur; x++)
        for (int z = 0; z <= hauteur; z++)
        {
            if (!mursH[x, z]) continue;
            Vector3 centre = new Vector3((x + 0.5f) * tailleCellule, 0f, z * tailleCellule);
            PlacerSegmentHaie("MurH", centre, Vector3.right, tailleCellule);
        }

        // Murs verticaux (segments le long de l'axe Z)
        for (int x = 0; x <= largeur; x++)
        for (int z = 0; z < hauteur; z++)
        {
            if (!mursV[x, z]) continue;
            Vector3 centre = new Vector3(x * tailleCellule, 0f, (z + 0.5f) * tailleCellule);
            PlacerSegmentHaie("MurV", centre, Vector3.forward, tailleCellule);
        }
    }

    void PlacerSegmentHaie(string nom, Vector3 centreLocal, Vector3 direction, float longueur)
    {
        // Convertit en position monde pour sampler le terrain
        Vector3 mondeCentre = racine.TransformPoint(centreLocal);
        float solY = HauteurSol(mondeCentre.x, mondeCentre.z);

        // Objet parent du segment (positionné au sol en coordonnées monde)
        var segment = new GameObject(nom);
        segment.transform.SetParent(racine, false);
        segment.transform.position = new Vector3(mondeCentre.x, solY, mondeCentre.z);
        segment.transform.rotation = Quaternion.identity;

        // Collider invisible pour bloquer le joueur
        var col        = segment.AddComponent<BoxCollider>();
        float hTotale  = nbCouches * hauteurCouche + 0.5f;
        float epTotale = (nbRangees - 1) * espacementRangees + 1f;
        col.center = new Vector3(0f, hTotale * 0.5f, 0f);
        col.size   = (direction == Vector3.right)
            ? new Vector3(longueur, hTotale, epTotale)
            : new Vector3(epTotale, hTotale, longueur);

        Vector3 dir  = direction.normalized;
        Vector3 perp = Vector3.Cross(Vector3.up, dir).normalized;

        int   nbB  = Mathf.Max(2, Mathf.CeilToInt(longueur / espacementBuissons) + 1);
        float pas  = longueur / (nbB - 1);
        float demi = longueur * 0.5f;
        float decalageRangee = (nbRangees - 1) * espacementRangees * 0.5f;

        for (int couche = 0; couche < nbCouches; couche++)
        for (int rangee = 0; rangee < nbRangees; rangee++)
        {
            float offsetRangee = rangee * espacementRangees - decalageRangee;

            for (int i = 0; i < nbB; i++)
            {
                float along  = -demi + i * pas;
                float cote   = Aleatoire(-jitterCote, jitterCote) * (1f - couche * 0.3f);
                float angle  = Aleatoire(-jitterAngle, jitterAngle);
                float ech    = Aleatoire(echelleAleatoire.x, echelleAleatoire.y);

                // Snap terrain individuel pour la couche basse
                float yLocal = couche * hauteurCouche;
                if (couche == 0 && snapTerrain)
                {
                    Vector3 approxMonde = segment.transform.position
                        + dir * along
                        + perp * (offsetRangee + cote);
                    float solBuisson = HauteurSol(approxMonde.x, approxMonde.z);
                    yLocal = solBuisson - solY;
                }

                var prefab = prefabsBuissons[rng.Next(prefabsBuissons.Length)];
                if (prefab == null) continue;

                var buisson = Instantiate(prefab, segment.transform);
                buisson.transform.localPosition =
                    dir * along + perp * (offsetRangee + cote) + Vector3.up * yLocal;
                buisson.transform.localRotation = Quaternion.Euler(0f, angle, 0f);
                buisson.transform.localScale   *= ech;
                buisson.name = "Buisson";
            }
        }
    }

    // -------------------------------------------------------
    // Placement des clés dans les culs-de-sac
    // -------------------------------------------------------

    void PlacerCles()
    {
        if (prefabCle == null)
        {
            Debug.LogWarning("MazeGenerator : prefab de clé non assigné.");
            return;
        }

        // Collecte tous les culs-de-sac (1 seul passage ouvert)
        var culsDeSac = new List<Vector2Int>();
        for (int x = 0; x < largeur; x++)
        for (int z = 0; z < hauteur; z++)
        {
            if (x == 0 && z == 0) continue;             // ignore l'entrée
            if (x == largeur-1 && z == hauteur-1) continue; // ignore la sortie
            if (EstCulDeSac(x, z)) culsDeSac.Add(new Vector2Int(x, z));
        }

        // Mélange les culs-de-sac pour un placement aléatoire
        Melanger(culsDeSac);
        int nb = Mathf.Min(nombreCles, culsDeSac.Count);

        for (int i = 0; i < nb; i++)
        {
            Vector2Int cell = culsDeSac[i];
            Vector3 posLocale = new Vector3(
                (cell.x + 0.5f) * tailleCellule,
                0f,
                (cell.y + 0.5f) * tailleCellule
            );
            Vector3 posMonde = racine.TransformPoint(posLocale);
            posMonde.y = HauteurSol(posMonde.x, posMonde.z) + hauteurCle;

            var cle = Instantiate(prefabCle, racine);
            cle.transform.position = posMonde;
            cle.name = "Cle_" + (i + 1);

            // Ajoute le script de ramassage si le prefab ne l'a pas déjà
            if (cle.GetComponent<KeyItem>() == null)
                cle.AddComponent<KeyItem>();
        }
    }

    bool EstCulDeSac(int x, int z)
    {
        int passages = 0;
        if (x > 0          && !mursV[x,     z]) passages++; // Ouest
        if (x < largeur-1  && !mursV[x + 1, z]) passages++; // Est
        if (z > 0          && !mursH[x, z    ]) passages++; // Sud
        if (z < hauteur-1  && !mursH[x, z + 1]) passages++; // Nord
        return passages == 1;
    }

    // -------------------------------------------------------
    // Placement de la sortie
    // -------------------------------------------------------

    void PlacerSortie()
    {
        if (prefabSortie == null) return;

        // Juste au nord de la dernière cellule (là où l'ouverture a été créée)
        Vector3 posLocale = new Vector3(
            (largeur - 0.5f) * tailleCellule,
            0f,
            hauteur * tailleCellule
        );
        Vector3 posMonde = racine.TransformPoint(posLocale);
        posMonde.y = HauteurSol(posMonde.x, posMonde.z) + hauteurSortie;

        var sortie = Instantiate(prefabSortie, racine);
        sortie.transform.position = posMonde;
        sortie.name = "Sortie";

        if (sortie.GetComponent<MazeSortie>() == null)
            sortie.AddComponent<MazeSortie>();
    }

    // -------------------------------------------------------
    // Utilitaires
    // -------------------------------------------------------

    float HauteurSol(float mondeX, float mondeZ)
    {
        if (snapTerrain)
        {
            var terrain = Terrain.activeTerrain;
            if (terrain != null)
                return terrain.SampleHeight(new Vector3(mondeX, 0f, mondeZ))
                     + terrain.transform.position.y;
        }
        return transform.position.y;
    }

    float Aleatoire(float min, float max)
    {
        if (Mathf.Approximately(min, max)) return min;
        return (float)(min + (max - min) * rng.NextDouble());
    }

    void Melanger<T>(List<T> liste)
    {
        for (int i = liste.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (liste[i], liste[j]) = (liste[j], liste[i]);
        }
    }
}
