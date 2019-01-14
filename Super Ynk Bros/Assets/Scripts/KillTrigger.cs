using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Mata al Player cuando entra en una zona de muerte, 
    // el script se asigna a una zona de muerte

public class KillTrigger : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            PlayerController.sharedInstance.Kill();
        }
    }
}
