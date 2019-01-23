using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectibleType
{
    gold,
    healthPotion,
    manaPotion
}

public class Collectible : MonoBehaviour
{
    public CollectibleType type = CollectibleType.gold;
    bool isCollected = false;
    public int collectibleValue;

    void Start() // Start is called before the first frame update
    {
        Show();
    }
    
    void Show()     // para mostrar
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<CircleCollider2D>().enabled = true;
        isCollected = false;
    }

    void Hide()     // para desactivar
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<CircleCollider2D>().enabled = false;
    }  
    
    void Collect()  // para recolectar
    {        
        isCollected = true;
        Hide();
        switch (this.type)
        {
            case CollectibleType.gold:
                GameManager.sharedInstance.CollectObject(this.collectibleValue);
                break;
            case CollectibleType.healthPotion:
                PlayerController.sharedInstance.CollectHealth(this.collectibleValue);
                break;
            case CollectibleType.manaPotion:
                PlayerController.sharedInstance.CollectMana(this.collectibleValue);
                break;
            default:
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)  // al chocar con Player
    {
        if(collider.tag == "Player")
        {
            Collect();
        }
    }
}
