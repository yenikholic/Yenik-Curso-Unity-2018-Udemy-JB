using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public static int coinCount = 0; //static es para que la variable sea compartida por todas las instancias de la moneda y que no tenga cada moneda su coincount.
	// Use this for initialization
	void Start () {
        Coin.coinCount++;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider playerCol)
    {
        if (playerCol.tag == "Player")
        {
            Coin.coinCount--;
            Timer.countDown += 5;
            Debug.Log("Te quedan " +Timer.countDown+ "segundos y " + Coin.coinCount + " monedas.");
            if (Coin.coinCount <= 0)
            {
                GameObject gameManager = GameObject.Find("GameManager");
                Destroy(gameManager);

                GameObject[] fireworks = GameObject.FindGameObjectsWithTag("Firework");
                foreach(GameObject firework in fireworks)
                {
                    firework.GetComponent<ParticleSystem>().Play();
                }
                Debug.Log("Has cogido todas las monedas! Has ganado!");
            }
            Destroy(gameObject);
        }       
    }
}
