using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    public string firstName = "Luke";
    public string lastName = "Skywalker";
    public string email = "luke@starwards.com";
    public int age = 32;
    public float height = 1.78f;
    public float weight = 82.5f;
    // Start is called before the first frame update
    void Start()
    {
        float playerHeight = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
