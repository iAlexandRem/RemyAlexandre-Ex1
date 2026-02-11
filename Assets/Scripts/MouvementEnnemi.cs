using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UIElements;

public class MouvementEnnemi : MonoBehaviour
{
    // Variables
    public float vitesseY;
    public float vitesseRotation;
    private Vector2 position;

    
    void Start()
    {
        transform.position = new Vector2(Random.Range(-5, 7), Random.Range(-6, 7)); // Position random d'une zone de la map
    

        int nbAleatoire = Random.Range(0, 2); // Nombre entier aléatoire entre 0 et 1
        if (nbAleatoire == 0)
        {
            vitesseY = -vitesseY; // Direction inverse (1 chance sur 2)
        }
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + vitesseY);

        if (transform.position.y > 6.0f) // Dépassement de la map vers le bas
        {
            transform.position = new Vector2(transform.position.x, -6.0f);
        }
        else if (transform.position.y < -6.0f) // Dépassement de la map vers le haut
        {
            transform.position = new Vector2(transform.position.x, 6.0f);
        }
    }
}
