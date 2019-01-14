using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Transform target;                                    // la posición del objetivo de la cámara

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0,1.6f,0);      // distancia que queremos que la cámara esté desde el punto artificial añadido
        public float SmoothFactor = 100f;                           // factor de suavidad de giro de la cámara 
        public float distanceFromTarget = -8;
        public float zoomSmooth = 100;
        public float zoomStep = 2;
        public float maxZoom = -4;                          // distancia minima y maxima del zoom
        public float minZoom = -15;
        public bool smoothFollow = true;
        public float smooth = 0.02f;

        [HideInInspector]
            public float newDistance = -8;
        [HideInInspector]
            public float adjustmentDistance = -8;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;   
        public float yRotation = -180;
        public float maxXRotation = 25;     // no permite poner la camara más abajo de 25 grados del personaje
        public float minXRotation = -50;    // no permite poner la camara más arriba de 85 grados arriba del personaje
        public float vOrbitSmooth = 0.5f;    // suavizar la rotación en el eje x
        public float hOrbitSmooth = 0.5f;    // suavizar la rotacion en el eje y
    }

    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";    // input que tenemos que crear para volver a la posición inicial de la camara
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";             // input que rota la camara en el eje y
        public string ORBIT_VERTICAL = "OrbitVertical";                 // input que rota la camara en el eje y
        public string MOUSE_ORBIT = "MouseOrbit";             // input que rota la camara en el eje y
        public string MOUSE_ORBIT_VERTICAL = "MouseOrbitVertical";                 // input que rota la camara en el eje y
        public string ZOOM = "Mouse ScrollWheel";                       // input para hacer zoom
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }
    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;                         // será la posición objetivo a la que queremos mover la cámara
    Vector3 adjustedDestination = Vector3.zero;                 // si colisiona la camara usaremos esta variable, sino, usaremos destination
    Vector3 camVel = Vector3.zero;
    CharController charController;                              // necesitamos una referencia charController para acceder a la rotación del personaje y podamos hacer las de la cámara
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput, vMouseOrbitInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;


    void Start()
    {
        SetCameraTarget(target);

        vOrbitInput = hOrbitInput = zoomInput = hOrbitInput = mouseOrbitInput = vMouseOrbitInput = 0;
        //targetPos = target.position + position.targetPosOffset;
        // el destino de la nueva posición de la camara es el giro de angulo cogiendo la orbita del personaje en x e y multiplicando  
        // la distancia en negativo en dirección hacia alante( ya que la camara esta detras del personaje) multiplicado por 
        // la distancia de la camara del personaje.
        MoveToTarget();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        previousMousePos = currentMousePos = Input.mousePosition;
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

    void GetInput()
    {

        vOrbitInput = Input.GetAxis(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxis(input.ORBIT_HORIZONTAL);

        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
        mouseOrbitInput = Input.GetAxis(input.MOUSE_ORBIT);
        vMouseOrbitInput = Input.GetAxis(input.MOUSE_ORBIT_VERTICAL);
    }

    private void Update()
    {
        GetInput();
        ZoomInOnTarget();
    }

    void FixedUpdate()
    {
        //moving
        MoveToTarget();
        //rotating
        LookAtTarget();
        // player input orbit
        OrbitTarget();
       // MouseOrbitTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        //draw debug lines here
        for(int i = 0; i<5; i++)
        {
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }
            if (debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(targetPos); // using raycasts here
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    void MoveToTarget()
    {
        targetPos = target.position + Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x);
        // el destino de la nueva posición de la camara es el giro de angulo cogiendo la orbita del personaje en x e y multiplicando  
        // la distancia en negativo en dirección hacia alante( ya que la camara esta detras del personaje) multiplicado por 
        // la distancia de la camara del personaje.
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0)* -Vector3.forward * position.distanceFromTarget;  
        destination += targetPos;     // sumamos la rotación de la cámara a la posición del objetivo

        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            }
            else transform.position = adjustedDestination;
        }
        else
        {
            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);
            }
            else transform.position = destination;
        }
    }

    void LookAtTarget()
    {
        // mirando al origen del objetivo, y lookrotation va a tomar un vector3 que va a ser la direccion que nuestra camara va a mirar
        // restandole la posición de la camara a la posicion del objetivo
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 100 * Time.deltaTime);
    }

    void OrbitTarget()
    {
        if (hOrbitSnapInput > 0)
        {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth;

        CheckVerticalRotation();
    }
    /*
    void MouseOrbitTarget()
    {
        previousMousePos = currentMousePos;
        currentMousePos = Input.mousePosition;

        Vector3.Normalize(previousMousePos);
        Vector3.Normalize(currentMousePos);

        if(mouseOrbitInput > 0)
        {
            orbit.xRotation += (currentMousePos.y - previousMousePos.y) * orbit.vOrbitSmooth;
            orbit.yRotation += (currentMousePos.x - previousMousePos.x) * orbit.hOrbitSmooth;
        }
        if (vMouseOrbitInput > 0)
        {
            orbit.xRotation += (currentMousePos.y - previousMousePos.y) * (orbit.vOrbitSmooth / 2);
        }
    }
    */
    void CheckVerticalRotation()
    {
        if (orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }

        if (orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }
    }


    void ZoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;
        if (position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
            position.newDistance = position.maxZoom;
        }
        if (position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
            position.newDistance = position.minZoom;
        }
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        // actualizar los clippoints mientras se mueve
        public void UpdateCameraClipPoints (Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera) return;

            //clear the contents of intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 1) * z;
            float y = x / camera.aspect;

            // top left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; // added and rotated the point relative to camera

            // top rightt
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition; // added and rotated the point relative to camera

            // bottom left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; // added and rotated the point relative to camera

            // bottom right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition; // added and rotated the point relative to camera

            // camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if(Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }
            return false;
        }        

        // devuelve la distancia que la camara tiene que estar del objetivo
        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1) distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                        {
                            distance = hit.distance;
                        }
                    }
                }
            }

            if (distance == -1) return 0;
            else return distance;
        }

        // actualizar nuestro booleano de colisión
        public void CheckColliding(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition)) colliding = true;
            else colliding = false;
        }
    }
}
