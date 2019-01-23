﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMovement : MonoBehaviour
{
    public bool movingForward;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Collectible")
        {
            return;
        }

        if(movingForward == true)
        {
            Enemy.turnAround = true;
            
        }
        else
        {
            Enemy.turnAround = false;
        }
        movingForward = !movingForward;
        
    }
}
