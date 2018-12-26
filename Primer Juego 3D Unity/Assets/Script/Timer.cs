using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float maxTime;
    public static float countDown;
    // Start is called before the first frame update
    void Start()
    {
        countDown = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            Debug.Log("Te has quedado sin tiempo... HAS PERDIDO!");

            Coin.coinCount = 0;

            SceneManager.LoadScene("Level_01");
        }
    }
}
