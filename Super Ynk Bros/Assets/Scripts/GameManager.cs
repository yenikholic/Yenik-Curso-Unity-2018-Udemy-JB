using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Posibles estados del videojuego
public enum GameState{
    startMenu,
    pauseMenu,
    inGame,
    gameOver
}

    // Script que se encarga de manejar el estado del juego
    // Activación y cambio de estados: dentro del juego,
    // en menus, fin de juego.
    // se asigna a un bameobject vacío Game Manager por ejemplo

public class GameManager : MonoBehaviour
{
    // Variable que referncia al propio Game Manager, para crear el gamemanager como SINGLETON
    public static GameManager sharedInstance;

    // Variable para saber en qué estado del juego nos encontramos ahora mismo
    // Al inicio, queremos que empiece en el menú principal
    public GameState currentGameState = GameState.startMenu;

    public Canvas gameCanvas, startMenuCanvas, pauseMenuCanvas, gameoverMenuCanvas;

    private void Awake()
    {
        // GameManager se crea a sí mismo para que solo haya una instancia de él como lo q se llama SINGLETON
        sharedInstance = this;
    }
    private void Start()
    {
        ToStartMenu();
    }
    private void Update()
    {
        
        #region ** ACCESO Y CAMBIOS A MENÚ, PAUSA, GAMEOVER **
         
        // si MENU y pulsamos START (Enter) volvemos a IN GAME
        if (Input.GetButtonDown("Start") && this.currentGameState == GameState.pauseMenu)
        {
            StartGame();
        }
        // si IN GAME y pulsamos PAUSE (Backspace) abrimos el MENU
        else if (Input.GetButtonDown("Pause") && this.currentGameState == GameState.inGame)
        {
            ToPauseMenu();
        }
        // si GAME OVER y pulsamos el boton START (Enter) volvemos al último punto de guardado 
        // usando las posiciones de inicio guardadas en el PlayerController.
        else if (Input.GetButtonDown("Start") && this.currentGameState == GameState.gameOver)
        {
            Debug.Log("Volviendo al último punto guardado ...");
/*            if(PlayerController.sharedInstance.transform.position.x > 36)
            {
                LevelGenerator.sharedInstance.RemoveAllBlocks();
                LevelGenerator.sharedInstance.GenerateInitialBlocks();
            }                    */
            StartGame();
        }

        #endregion

    }
    // Método para iniciar el juego
    public void StartGame()
    {        
        SetGameState(GameState.inGame);

        if (PlayerController.sharedInstance.transform.position.x > 36)
        {
            LevelGenerator.sharedInstance.RemoveAllBlocks();
            LevelGenerator.sharedInstance.GenerateInitialBlocks();
        }
        
        PlayerController.sharedInstance.StartGame();
        PlayerController.sharedInstance.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCamera2D>().ResetCameraPosition();
    }

    // Método para salir del juego
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Método para cuando el jugador muera
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }
    // Método para volver al menú principal cuando el usuario quiera
    public void ToPauseMenu()
    {
        SetGameState(GameState.pauseMenu);
    }
    public void ToStartMenu()
    {
        SetGameState(GameState.startMenu);
    }

    // Método encargado de cambiar el estado del juego
    void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.startMenu)
        {
            pauseMenuCanvas.enabled = false;
            gameoverMenuCanvas.enabled = false;
            gameCanvas.enabled = false;
            //Hay que preparar la escena de Unity para mostrar el menú Start
            startMenuCanvas.enabled = true;
        }
        else if (newGameState == GameState.pauseMenu)
        {
            //Hay que preparar la escena de Unity para mostrar el menú Pausa
            pauseMenuCanvas.enabled = true;
        }
        else if(newGameState == GameState.inGame)
        {
            //Hay que preparar la escena de Unity para jugar
            startMenuCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            gameoverMenuCanvas.enabled = false;
            gameCanvas.enabled = true;
        }
        else if(newGameState == GameState.gameOver)
        {
            //Hay que preparar la escena de Unity para el Game Over
            startMenuCanvas.enabled = false;
            pauseMenuCanvas.enabled = false;
            gameoverMenuCanvas.enabled = true;
        }
        // Asignamos el estado de juego actual al que nos ha llegado por parametro
        this.currentGameState = newGameState;
    }
}
