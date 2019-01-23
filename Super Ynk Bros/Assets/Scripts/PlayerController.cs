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
    private int maxHp, maxMp, hp, mp = 0;

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

    // NO PUEDE IR EN EL UPDATE, tiene que ir al FixedUpdate
    public const float RUN_SPEED = 12f, JUMP_FORCE = 33f, MIN_SPEED = 4f, HEALTH_TIME_DECREASE = 1f;
    public const int TIRE_MIN_HEALTH = 20;
    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 20;
    public const float SUPERJUMP_COST = 3f, SUPERJUMP_FORCE = 1.3f;
    public const float INVINC_TIME = 2f;
    private float timeForDamage = 0f;
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
        timeForDamage = 0f;
        anim.SetBool("isAlive", true);
        anim.SetBool("isGrounded", true);
    }

    // Empezamos el juego y lo ponemos en la posicion inicial
    public void StartGame()
    {
        this.maxHp = this.hp = INITIAL_HEALTH;
        this.maxMp = this.mp = INITIAL_MANA;
        anim.SetBool("isAlive", true);
        anim.SetBool("isGrounded", true);
        // cada vez que reiniciamos lo ponemos en la startPosition
        this.transform.position = startPosition;

        StartCoroutine("TirePlayer");
    }

    public void PauseGame()
    {
        rig2D.isKinematic = true;
    }

    public void ContinueGame()
    {
        rig2D.isKinematic = false;
    }

    IEnumerator TirePlayer()        // <<----  CORRUTINA  
                    // ERROR Cuando se baja al minimo y se incrementa la hp a más del minimo no se vuelve a ejecutar
    {
        while (this.hp > TIRE_MIN_HEALTH)
        {
            this.hp--;
            yield return new WaitForSeconds(HEALTH_TIME_DECREASE);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Solo dejamos que salte si el juego está en modo inGame
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            //contador para restar retraso de daño
            if (timeForDamage > 0)
            {
                timeForDamage -= Time.deltaTime;
            }

            // salto
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            // TRUCO ponemos el isGrounded de la animacion con la funcion IsTouchingTheGround directamente.
            if(Input.GetButton("Jump")) anim.SetBool("isGrounded", false);
            else anim.SetBool("isGrounded", IsTouchingTheGround());
        }
        if (GameManager.sharedInstance.currentGameState == GameState.startMenu)
        {
            this.rig2D.isKinematic = false;
        }
        
    }

    private void FixedUpdate()
    {
        // Solo nos movemos si estamos en el modo inGame
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            // cada vez va mas lento cuanta menos vida tenga
            float currentSpeed = (RUN_SPEED - MIN_SPEED) * (float)this.hp / (float)this.maxHp;
            // Si apretamos la letra D
            if (Input.GetKey(KeyCode.D))
            {
                // No giramos el sprite
                sr.flipX = false;

                // si la velocidad hacia la derecha es menor que runSpeed
                if (rig2D.velocity.x < currentSpeed)
                {
                    // La velocidad en x será runSpeed para ir hacia la derecha
                    // y dejaremos la velocidad de y como estaba
                    rig2D.velocity = new Vector2(currentSpeed, rig2D.velocity.y);
                }
            }
            // Si apretamos la letra A
            else if (Input.GetKey(KeyCode.A))
            {
                // Giramos el sprite hacia la izquierda
                sr.flipX = true;

                // si la velocidad hacia la derecha es mayor que -runSpeed
                if (rig2D.velocity.x > -currentSpeed)
                {
                    // La velocidad en x será -runSpeed para ir hacia la izquierda
                    // y dejaremos la velocidad de y como estaba
                    rig2D.velocity = new Vector2(-currentSpeed, rig2D.velocity.y);
                }
            }
            // Si no se aprieta nada y la velocidad de x és mayor que 0 (que va hacia la derecha)
            Vector3 vel = rig2D.velocity;
            vel.x *= 0.8f;
            rig2D.velocity = vel;
        }        
    }

    // metodo para saltar
    void Jump()
    {
        // Si toca el suelo
        if (IsTouchingTheGround())
        {
            if (this.mp >= SUPERJUMP_COST)
            {
                mp -= (int)SUPERJUMP_COST;
                rig2D.AddForce(Vector2.up * JUMP_FORCE * SUPERJUMP_FORCE, ForceMode2D.Impulse);
            }
            // añadimos una fuerza de impulso al rigidbody
            // ley de newton        2da ley de newton
            //F = m * a             a = F/m
            else
            {
                rig2D.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
            }
        }
    }

    // metodo para saber si esta en el suelo
    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down, 0.2f, groundLayer)) return true;
        else return false;
    }
    
    // metodo para matar al jugador
    public void Kill()
    {
        this.anim.SetBool("isAlive", false);
        GameManager.sharedInstance.GameOver();
        rig2D.velocity = new Vector2(0, rig2D.velocity.y);

        float currentMaxScore = PlayerPrefs.GetFloat("maxdistance", 0);
        if (this.GetDistance() > currentMaxScore)
        {
            PlayerPrefs.SetFloat("maxdistance", this.GetDistance());
        }

        StopCoroutine("TirePlayer");
    }

    // metodo para obtener la distancia recorrida
    public float GetDistance()
    {
        float travelledDistance = Vector2.Distance(new Vector2(startPosition.x,0), new Vector2(this.transform.position.x,0));
        return travelledDistance; // this.transform.position.x - startPosition.x
    }

    // metodos para sumar vida y mana
    public void CollectHealth(int hpoints)
    {
        this.hp += hpoints;
        if(this.hp > this.maxHp)
        {
            this.hp = maxHp;
        }
    }
    public void CollectMana(int mpoints)
    {
        this.mp += mpoints;
        if(this.mp > this.maxMp)
        {
            this.mp = maxMp;
        }
    }

    // metodo para las barras de vida y mana
    public int[] GetHealth()
    {
        return new int[]{ this.hp, this.maxHp };
    }
    public int[] GetMana()
    {
        return new int[] { this.mp, this.maxMp };
    }

    // metodo de daño al jugador
    public void GetDamage(int dmg)
    {
        if (timeForDamage <= 0)
        {
            this.hp -= dmg;
            timeForDamage = INVINC_TIME;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            GetDamage(10);           
        }
        if(collider.tag == "Rock")
        {
            GetDamage(5);
        }
        if(GameManager.sharedInstance.currentGameState == GameState.inGame && this.hp <= 0)
        {
            Kill();
        }
    }
}
