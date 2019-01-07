using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float maxTime;
    public static float countDown;
    public GameObject[] fireworks;
    private bool end;

    // Start is called before the first frame update
    void Start()
    {
        end = false;
        countDown = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;

        if (Coin.coinCount <= 0 && !end)
        {
            //GameObject gameManager = GameObject.Find("GameManager");
            //Destroy(gameManager);   // destruye el gamemanager si ya no hay monedas

            //GameObject[] fireworks = GameObject.FindGameObjectsWithTag("Firework");     // recoge todos los gameObjects fuegos artificiales
            foreach (GameObject firework in fireworks)
            {
                firework.SetActive(true);     // activa cada fuego artificial del array fireworks[]
            }
            Debug.Log("Has cogido todas las monedas! Has ganado!");
            end = true;
        }

        if (countDown <= 0)
        {
            Debug.Log("Te has quedado sin tiempo... HAS PERDIDO!");

            Coin.coinCount = 0;

            SceneManager.LoadScene("Level_01");
        }
    }
}
