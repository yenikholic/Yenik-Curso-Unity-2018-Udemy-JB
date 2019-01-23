using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rig2D;
    public SpriteRenderer sr;
    public float RUN_SPEED = -5f;
    public static bool turnAround;

    void Awake()
    {
        rig2D = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        float currentRunSpeed = RUN_SPEED;
        if(turnAround == true)
        {
            currentRunSpeed = RUN_SPEED;
            this.transform.eulerAngles = new Vector3(0, 180f, 0);
            Debug.Log("trying to turn");
        }
        else
        {
            currentRunSpeed = -RUN_SPEED;
            this.transform.eulerAngles = new Vector3(0, 0, 0);            
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {  
            rig2D.velocity = new Vector2(currentRunSpeed, rig2D.velocity.y);
        }
    }

}
