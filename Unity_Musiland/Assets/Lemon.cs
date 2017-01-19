using UnityEngine;
using System.Collections;

public class Lemon : MonoBehaviour {
    public float LemonSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float toMove = LemonSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * toMove);

    }

    void OnCollisionEnter2D(Collision2D collision) // Si le lemon touche quelque chose
    {
        if (collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);

        }

        // Ajouter if ennemi
    }

    void OnCollisionExit2D(Collision2D collision) // Si le lemon sort de l'écran
    {
        if (collision.gameObject.tag == "MainCamera")
        {
            Destroy(gameObject);

        }
    }
}
