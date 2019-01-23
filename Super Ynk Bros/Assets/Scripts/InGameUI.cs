using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    public Text goldLabel;
    public Text hpLabel;
    public Text maxDistanceLabel;
    public Text distanceLabel;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            int currentGold = GameManager.sharedInstance.currentGold;
            this.goldLabel.text = currentGold.ToString();

            float travelledDistance = PlayerController.sharedInstance.GetDistance();
            this.distanceLabel.text = "distance " + travelledDistance.ToString("f0");          
        }
        if(GameManager.sharedInstance.currentGameState == GameState.gameOver ||
            GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            this.maxDistanceLabel.text = "max distance " + PlayerPrefs.GetFloat("maxdistance", 0).ToString("f0");
        }
       
    }
}
