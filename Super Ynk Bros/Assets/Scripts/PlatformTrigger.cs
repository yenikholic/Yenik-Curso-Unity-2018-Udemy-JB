using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public BoxCollider2D platformCol;
    
    void Start()
    {
        platformCol.enabled = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.GetType() == typeof(BoxCollider2D) && col.tag == "Player")
        {
            platformCol.enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.GetType() == typeof(BoxCollider2D) && col.tag == "Player")
        {
            platformCol.enabled = false;
        }
    }
}
