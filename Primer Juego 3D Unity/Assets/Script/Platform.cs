using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Platform"))
        {
            transform.parent = col.transform;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Platform"))
        {
            col.transform.SetParent(transform);
        }
    }
}
