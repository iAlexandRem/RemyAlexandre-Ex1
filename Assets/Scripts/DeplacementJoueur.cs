// using System;
using UnityEngine;
using UnityEngine.InputSystem; // Cette ligne permet de détecter les touches du clavier

public class DeplacementJoueur : MonoBehaviour
{
    [Header("Déplacement du joueur")]
    public Vector2 positionInitialeJoueur;
    public float vitesse = 5;
    public float vitesseRotation = 180;

    [Header("Limites de la zone d'arrivée")]
    public float positionZoneX = 8f;
    public float positionZoneMinY = -3f;
    public float positionZoneMaxY = 3f;

    private float nouvellePositionY;
    private float ecartPositionY;
    private bool peutBouger = false;

    GameObject zoneArrivee;
    GameObject cercleObstacles;

    void Start()
    {
        positionInitialeJoueur = transform.position;
        ReplacerJoueur(); //Sinon étrangement dans le Build, le joueur ne commence pas au bon endroit

        //On cherche les objets sur la scène
        zoneArrivee = FindAnyObjectByType<ZoneArrivee>().gameObject;
        cercleObstacles = FindAnyObjectByType<PositionObstacles>().gameObject;

        if (!zoneArrivee)
        {
            Debug.LogWarning("Vous devez avoir un objet avec le script ZoneArrivee dans la scene");
        }
        else
        {
            nouvellePositionY = Random.Range(positionZoneMinY, positionZoneMaxY); //Une 1ere position aléatoire en Y de zone d'arrivée pour commencer
            zoneArrivee.transform.position = new Vector2(positionZoneX, nouvellePositionY);
            ReplacerZone();
        }
    }


    void Update()
    {
        // Variables temporaires pour gérer le déplacement et la rotation du joueur
        float deplacement = 0;
        float rotation = 0;

        // Gestion des touches pour la rotation du joueur
        if (Keyboard.current.aKey.isPressed)
        {
            rotation = vitesseRotation * Time.deltaTime;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            rotation = vitesseRotation * Time.deltaTime * -1;
        }

        // Gestion des touches pour le déplacement du joueur
        if (Keyboard.current.wKey.isPressed)
        {
            deplacement = vitesse * Time.deltaTime;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            deplacement = vitesse * Time.deltaTime * -1; //Permet d'aller par en arrière en multipliant par -1
        }

        //On applique la rotation et le déplacement
        transform.Rotate(0, 0, rotation);
        transform.Translate(deplacement * Vector2.up); //Vector2.up correspond à la flèche verte du joueur

        //On bloque la position pour éviter de sortir de l'écran
        //Mathf.Clamp limite une valeur entre 2 points
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4, 4));


        //Vérification de la collision avec la zone d'arrivée
        float distanceAvecZone = Vector2.Distance(zoneArrivee.transform.position, this.transform.position);
        float distanceMinPourCollision = (this.transform.localScale.x / 2) + (zoneArrivee.transform.localScale.x / 2);

        //Si la distance entre la zone d'arrivée et le joueur est plus petite que la largeur du joueur + la largeur de la zone (la moitié car le point de pivot est au centre du sprite)
        if (distanceAvecZone < distanceMinPourCollision)
        {
            ReplacerZone();
            ReplacerJoueur();
        }

        if (peutBouger == true)
        {
            zoneArrivee.transform.position = new Vector2(positionZoneX, zoneArrivee.transform.position.y + ecartPositionY * Time.deltaTime);

            if (Mathf.Abs(nouvellePositionY - zoneArrivee.transform.position.y) < 0.01f) //Seulement si l'écart (toujours positif) entre nouvelle et position actuelle est presque nul
            {
                peutBouger = false; //La zone devrait s'arrêter
            }
        }
    }

    public void ReplacerZone()
    {
        nouvellePositionY = Random.Range(positionZoneMinY, positionZoneMaxY); //Une nouvelle position de zone d'arrivée aléatoire en Y
        ecartPositionY = nouvellePositionY - zoneArrivee.transform.position.y; //Calcul écart entre nouvelle et ancienne position en Y 
        peutBouger = true; //Je veux une translation updatée entre les positions


        //Les obstacles changent de position
        cercleObstacles.transform.position = transform.position = new Vector2(Random.Range(-5, 8), Random.Range(-4, 5)); // Position random d'une zone de la map
    }

    // Fonction qui sert à replacer le joueur.
    // Est utilisée aussi dans le script DetectionProximiteJoueur
    public void ReplacerJoueur()
    {
        transform.eulerAngles = new Vector3(0, 0, -90);
        transform.position = positionInitialeJoueur;
    }
}
