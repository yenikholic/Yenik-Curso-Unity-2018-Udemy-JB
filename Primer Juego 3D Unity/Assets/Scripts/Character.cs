using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public string name;
    public int level;
    private int currentExp;
    private int expToLvl;
    public float atkSpd;
    public float power;

    public int maxHp;
    public int currentHp;
    public int maxMp;
    public int currentMp;
    public float currStamina;
    public float maxStamina;
    public int str;
    public int vit;
    public int dex;
    public int mag;
    public int wis;
    public float speed;
    public float runSpeed;
    public float walkSpeed;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        /*
        warrior = new CharClass();
        warrior.str = 20;
        warrior.vit = 20;
        warrior.dex = 10;
        warrior.mag = 1;
        warrior.wis = 5;
        warrior.maxHp = 300;
        warrior.maxMp = 50;
        warrior.maxStamina = 200;
        warrior.runSpeed = 20;
        warrior.walkSpeed = 10;
        warrior.jumpForce = 50;
        */

        expToLvl = level * 300 + level / 2 * 60;
        currentExp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
