﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustStorm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) gameObject.GetComponent<ParticleSystem>().Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
