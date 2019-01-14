using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Posibles estados del videojuego
public enum GameState{
    menu,
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
    public GameState currentGameState = GameState.menu;

    private void Awake()
    {
        // GameManager se crea a sí mismo para que solo haya una instancia de él como lo q se llama SINGLETON
        sharedInstance = this;
    }
    private void Update()
    {
        #region ** ACCESO Y CAMBIOS A MENÚ, PAUSA, GAMEOVER **
         
        // si MENU y pulsamos START (Enter) volvemos a IN GAME
        if (Input.GetButtonDown("Start") && this.currentGameState == GameState.menu)
        {
            StartGame();
        }
        // si IN GAME y pulsamos PAUSE (Backspace) abrimos el MENU
        else if (Input.GetButtonDown("Pause") && this.currentGameState == GameState.inGame)
        {
            BackToMenu();
        }
        // si GAME OVER y pulsamos el boton START (Enter) volvemos al último punto de guardado 
        // usando las posiciones de inicio guardadas en el PlayerController.
        else if (Input.GetButtonDown("Start") && this.currentGameState == GameState.gameOver)
        {
            Debug.Log("Volviendo al último punto guardado ...");
            PlayerController.sharedInstance.StartGame();
            StartGame();
        }

        #endregion

    }
    // Método para iniciar el juego
    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    // Método para cuando el jugador muera
    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    // Método para volver al menú principal cuando el usuario quiera
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    // Método encargado de cambiar el estado del juego
    void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu)
        {
            //Hay que preparar la escena de Unity para mostrar el menú

        }
        else if(newGameState == GameState.inGame)
        {
            //Hay que preparar la escena de Unity para jugar

        }
        else if(newGameState == GameState.gameOver)
        {
            //Hay que preparar la escena de Unity para el Game Over

        }
        // Asignamos el estado de juego actual al que nos ha llegado por parametro
        this.currentGameState = newGameState;
    }
}
