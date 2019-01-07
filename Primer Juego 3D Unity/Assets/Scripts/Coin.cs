using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public static int coinCount = 0; //static es para que la variable sea compartida por todas las instancias de la moneda y que no tenga cada moneda su coincount.
    private AudioSource aSource;
    private Renderer rend;
    private SphereCollider collider;

    // Use this for initialization
	void Start () {
        Coin.coinCount++;   // suma una moneda al contador de monedas cuando se crea
        aSource = GetComponent<AudioSource>();
        collider = GetComponent<SphereCollider>();
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, 100*Time.fixedDeltaTime));    // hace girar la moneda
	}

    private void OnTriggerEnter(Collider playerCol)
    {
        if (playerCol.tag == "Player")
        {
            rend.enabled = false;   // deja de mostrar la moneda
            collider.enabled = false; // desactiva el collider
            aSource.Play();         // reproduce el sonido de moneda al recogerla
            Coin.coinCount--;       // quita una moneda del contador de monedas
            Timer.countDown += 5;   // añade 5 segundos al timer al recoger la moneda

            Debug.Log("Te quedan " +Timer.countDown+ "segundos y " + Coin.coinCount + " monedas.");

            Destroy(gameObject, aSource.clip.length);       // al acabar el sonido destruye la moneda
        }
    }
}
