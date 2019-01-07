using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectar : MonoBehaviour
{
    Rigidbody rig;
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            rig = col.gameObject.GetComponent<Rigidbody>();
            col.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            rig = col.gameObject.GetComponent<Rigidbody>();
            col.transform.SetParent(null);
        }
    }
}
