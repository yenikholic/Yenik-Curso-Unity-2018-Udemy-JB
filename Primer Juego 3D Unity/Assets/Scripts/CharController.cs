using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{   
    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;       // velocidad a la que se moverá el personaje
        public float rotateVel = 2;       // velocidad a la que rotará el personaje
        public float jumpVel = 25;          // velocidad a la que decrementara y simulando gravedad.
        public float distToGrounded = 1.2f; //  distancia desde el centro del personaje hasta el suelo
        public LayerMask ground;            // desde que objetos especificos será capaz de saltar 
    }
    
    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 2.5f;
        public float lowJumpMultiplier = 2f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;    // retraso que tendrá al apretar la tecla
        public string FORWARD_AXIS = "Vertical"; // ponemos estos strings por si queremos modificar dentro del juego las teclas que usarán
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;          // mide si gira  de -1 a 1
    Rigidbody rBody;
    public int jumpNumber;
    public static int jumpsLeft;
    private float jumpDelay = 0.1f;
    public bool grounded = false;
    public bool isWalking = true;
    float forwardInput, turnInput, jumpInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }
    /*
    bool Grounded()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - moveSetting.distToGrounded, transform.position.z), Color.red);
        // un ray es un posicion y una direccion
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        jumpNumber = 2;
        targetRotation = transform.rotation;
        rBody = GetComponent<Rigidbody>(); // needs a rigidbody attached
        grounded = Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
        forwardInput = turnInput = jumpInput = 0;
    }

    void GetInput()
    {
        isWalking = !Input.GetKey(KeyCode.LeftShift);
        forwardInput = Input.GetAxis("Vertical"); // interpolado, negativo y positivo,  escala entre -1 0 y 1  con decimales no tiene que llevar raw
        
        if (Input.GetKey(KeyCode.Mouse1))
        {            
            turnInput = Input.GetAxis("Mouse X")*2;
        }
        else turnInput = Input.GetAxis("Horizontal");

        jumpInput = Input.GetAxisRaw("Jump"); // no interpolado, entonces da igual la escala, o 0 o 1, le damos un raw lo cuenta como entero

    }
    // Update is called once per frame
    void Update()
    {
        if (!isWalking && grounded) moveSetting.forwardVel = 20;
        else if(isWalking && grounded) moveSetting.forwardVel = 12;
        
        GetInput();
        Turn();
        Run();
        rBody.velocity = transform.TransformDirection(velocity);
        Jump();

        if (Input.GetButtonDown("Jump") && grounded)
        {
            grounded = false;
        }
        else grounded = Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    void FixedUpdate() // update para físicas
    {
        
      /*  if (grounded)
        {
            Debug.Log("entra");
            jumpsLeft = jumpNumber;
        }*/        

        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - moveSetting.distToGrounded, transform.position.z), Color.red);
        
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput)> 0 || (Input.GetKey(KeyCode.Mouse1) && Input.GetAxis("Horizontal") != 0)) // si el valor absoluto de forward input (va de -1 a 1) es mayor que inputDelay
        {
            /* move antiguo
             *   rBody.velocity = transform.forward * forwardInput * forwardVel;  lo multiplicamos porque forward input puede ser negativo o positivo atras o alante
             *
                *move
             en el eje z de profundidad añade movimiento de velocidad hacia alante*/
            velocity.z = moveSetting.forwardVel* forwardInput;

            velocity.x = moveSetting.forwardVel * Input.GetAxis("Horizontal");
        }
        else
        {
            //zero velocity
            velocity.z = 0;
            velocity.x = 0;
        }
    }

    void Turn()
    {
        /*
         *  a que angulo queremos rotar? rotamos a velocidad rotateVel * turninput(-1 a 1) * deltatime 
         *  en el eje y, mirara de izquierda a derecha, y hay que multiplicarlo por eél mismo para poder girar
         *  más de x grados.
         */ 
        if (Mathf.Abs(turnInput)> inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime , Vector3.up);            
        }
        transform.rotation = targetRotation;
    }

    void Jump()
    {
        // Better jump
        if(rBody.velocity.y < 0)
        {
            rBody.velocity += Vector3.up * Physics.gravity.y * (physSetting.downAccel - 1) * Time.deltaTime;
            jumpDelay -= Time.deltaTime;
        }
        else if (rBody.velocity.y > 0 && !Input.GetButton("Jump")  )
        {
            rBody.velocity += Vector3.up * Physics.gravity.y * (physSetting.lowJumpMultiplier - 1) * Time.deltaTime;
            jumpDelay -= Time.deltaTime;
        }

        // Jump
        else if (Input.GetButtonDown("Jump") && grounded && jumpsLeft > 0  && jumpDelay <= 0.1f || Input.GetButtonDown("Jump") && jumpsLeft > 0 && jumpDelay <= 0.1f)
        {

            jumpsLeft--;
            Debug.Log("jumps left: " + jumpsLeft);

            //jump
            rBody.velocity = Vector3.up * moveSetting.jumpVel;
            //rBody.AddForce(Vector3.up * moveSetting.jumpVel*10  );
            jumpDelay = 0.1f;
        }
        else if (jumpInput == 0 && grounded)
        {
            // zero out our velocity.y
            velocity.y = 0;
            jumpsLeft = jumpNumber;
        }
       /* else if (!grounded)
        {
            jumpDelay -= Time.deltaTime;      
            //decrease velocity.y
            velocity -= physSetting.downAccel*;
            //rBody.velocity += Vector3.up * Physics.gravity.y *100* Time.fixedDeltaTime;
            
        }*/
            
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain" || collision.gameObject.tag == "Platform")
        {
            jumpsLeft = jumpNumber;
        }
    }
}
