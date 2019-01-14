using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // Controlador del personaje 
    // movimiento, posición inicial o de punto de guardado
    // animaciones del personaje
    // salto y muerte
    // se asigna al Player

public class PlayerController : MonoBehaviour
{
    //Para hacer que el player sea singleton y no haya más de 1 jugador
    public static PlayerController sharedInstance;

    private Rigidbody2D rig2D;

    public SpriteRenderer sr;

    // animator asignamos aquí el sprite que tiene el animator.
    public Animator anim;

    // Esta variable sirve para detectar la capa del suelo
    public LayerMask groundLayer;

    // variable para guardar la posición para devolver el pj a esa posición despues de gameover
    private Vector3 startPosition;

    public float jumpForce = 33f;

    // NO PUEDE IR EN EL UPDATE, tiene que ir al FixedUpdate
    public float runSpeed = 8f;


    // inicialización
    void Awake()
    {
        sharedInstance = this;
        rig2D = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // al empezar la partida por primera vez
    private void Start()
    {
        anim.SetBool("isAlive", true);
        anim.SetBool("isGrounded", true);
    }

    // Empezamos el juego y lo ponemos en la posicion inicial
    public void StartGame()
    {
        anim.SetBool("isAlive", true);
        anim.SetBool("isGrounded", true);
        // cada vez que reiniciamos lo ponemos en la startPosition
        this.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Solo dejamos que salte si el juego está en modo inGame
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            // salto
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            // TRUCO ponemos el isGrounded de la animacion con la funcion IsTouchingTheGround directamente.
            anim.SetBool("isGrounded", IsTouchingTheGround());
        }
        
    }

    private void FixedUpdate()
    {
        // Solo nos movemos si estamos en el modo inGame
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            // Si apretamos la letra D
            if (Input.GetKey(KeyCode.D))
            {
                // No giramos el sprite
                sr.flipX = false;

                // si la velocidad hacia la derecha es menor que runSpeed
                if (rig2D.velocity.x < runSpeed)
                {
                    // La velocidad en x será runSpeed para ir hacia la derecha
                    // y dejaremos la velocidad de y como estaba
                    rig2D.velocity = new Vector2(runSpeed, rig2D.velocity.y);
                }
            }
            // Si apretamos la letra A
            else if (Input.GetKey(KeyCode.A))
            {
                // Giramos el sprite hacia la izquierda
                sr.flipX = true;

                // si la velocidad hacia la derecha es mayor que -runSpeed
                if (rig2D.velocity.x > -runSpeed)
                {
                    // La velocidad en x será -runSpeed para ir hacia la izquierda
                    // y dejaremos la velocidad de y como estaba
                    rig2D.velocity = new Vector2(-runSpeed, rig2D.velocity.y);
                }
            }
            // Si no se aprieta nada y la velocidad de x és mayor que 0 (que va hacia la derecha)
            else if (rig2D.velocity.x > 0f)
            {
                // reducimos la velocidad de x hasta que llegue a 0
                rig2D.velocity -= new Vector2(0.36f, 0);
            }
            // Si no se aprieta nada y la velocidad de x es menor que 0 (que va hacia la izquierda)
            else if (rig2D.velocity.x < 0f)
            {
                // aumentamos la velocidad de x hasta que llegue a 0
                rig2D.velocity += new Vector2(0.36f, 0);
            }
        }        
    }

    void Jump()
    {
        // Si toca el suelo
        if (IsTouchingTheGround())
        {
            // añadimos una fuerza de impulso al rigidbody
            // ley de newton        2da ley de newton
            //F = m * a             a = F/m
            rig2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }



    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 0.2f, groundLayer)) return true;
        else return false;
    }
    
    public void Kill()
    {
        this.anim.SetBool("isAlive", false);
        GameManager.sharedInstance.GameOver();
        
    }
}
