using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// herencia de MonoBehaviour, comportamiento único
public class Car : MonoBehaviour
{
    public string make = "Tesla";
    public string model = "S";
    public int numberOfWheels = 4;
    public int maxSpeed = 250;

    // se ejecuta antes de que arranque el juego
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    // Update para las fisicas
    void FixedUpdate()
    {
            
    }
}
