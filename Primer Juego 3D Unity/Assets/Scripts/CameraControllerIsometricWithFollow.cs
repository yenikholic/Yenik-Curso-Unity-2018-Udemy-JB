using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerIsometricWithFollow : MonoBehaviour {


    public Transform target;                                    // la posición del objetivo de la cámara
    public Vector3 offsetFromTarget = new Vector3(0,10,-9);      // distancia que queremos que la cámara esté del personaje
    public float xTilt = 10;                                    // recorrido de la cámara en el eje x, mirar hacia arriba o hacia abajo

    Vector3 destination = Vector3.zero;                         // será la posición objetivo a la que queremos mover la cámara
    CharController charController;                              // necesitamos una referencia charController para acceder a la rotación del personaje y podamos hacer las de la cámara
    float rotateVel = 0;                                        // usaremos una funcion de smoothdamp para modificarla
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;                           // factor de suavidad de giro de la cámara 

    void Start()
    {
        SetCameraTarget(target);
    }

    public void SetCameraTarget (Transform t)
    {
        target = t;

        if (target != null)
        {
            if (target.GetComponent<CharController>())
            {
                charController = target.GetComponent<CharController>();
            }
            else
                Debug.LogError("The camera's target needs a character controller.");
        }
        else
            Debug.LogError("Your camera needs a target.");
    }

    void LateUpdate()
    {
        //moving
        MoveToTarget();
        //rotating
        LookAtTarget();
    }



    void MoveToTarget()
    { 
        destination = charController.TargetRotation * offsetFromTarget;  // el destino es la nueva posicion de la rotación del personaje * distancia de la camara
        destination += target.position;     // sumamos la rotación de la cámara a la posición del objetivo
        transform.position = destination;   // hacemos que la posición de la cámara sea el resultado de este cálculo
    }

    void LookAtTarget()
    {
        // similar a un lerp pero mas suave
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y,ref rotateVel, SmoothFactor);
        //la rotación de la cámara será devolviendo un quaternion pasandole un vector 3, transform.eulerAngles.x (cualquiera que fuera mi posición x)
        // la z no se rota así que no la contemplamos, y le pasamos el angulo Y.
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
    }

}
