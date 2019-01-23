using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Seguimiento de la camara suave para el personaje
public class SmoothCamera2D : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;

    public float minYToFollow = 7f;
    public float maxYToFollow = 14f;
    public float startY = 7f;
    public float maxY = 19f;
    public float minY = 7f;

    private void Awake()
    {        
        Application.targetFrameRate = 60;  
    }

    // Use this for initialization
    void Start()
    {
        targetPos = transform.position + offset;        
    }

    public void ResetCameraPosition()
    {
        if (target)
        {
            Vector3 targetDirection = (target.transform.position + offset);
            transform.position = targetDirection;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;

            Vector3 targetDirection = (target.transform.position - posNoZ + offset);
            interpVelocity = targetDirection.magnitude * 6f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);            

            // si la la altura objetivo es mayor que maxY la altura objetivo será maxY
            if (targetPos.y > maxY) targetPos.y = maxY;
            // sino si la altura es menor que minY la latura sera minY
            else if (targetPos.y < minY) targetPos.y = minY;

            transform.position = Vector3.Lerp(transform.position, targetPos, 0.75f);

        }
    }
}
