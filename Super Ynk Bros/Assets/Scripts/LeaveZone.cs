using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveZone : MonoBehaviour
{
    private bool entry = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !entry)
        {
            LevelGenerator.sharedInstance.AddLevelBlock();
            LevelGenerator.sharedInstance.RemoveOldestLevelBlock();
            entry = true;
        }
        
    }
}
