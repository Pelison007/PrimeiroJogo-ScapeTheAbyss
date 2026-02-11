using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public Player player;
    public GameObject pauseMenu; // Painel de pause
    public bool isPaused = false;

    // Referência à ação de pause do Input System
    public InputAction pauseAction;
    public PlayerInput playerInput;
    private void OnEnable()
    {
        pauseAction.Enable(); // Habilita a ação
        pauseAction.performed += OnPause; // Liga a função ao evento performed
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPause; // Desliga o evento
        pauseAction.Disable(); // Desabilita a ação
    }

    // Esta função é chamada quando a ação é acionada
    public void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); // Mostra o menu
        Time.timeScale = 0f;       // Pausa o jogo
        isPaused = true;
        // DESATIVA todos os comandos (Move, Jump, etc)
        player.DesativaInput();
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false); // Esconde o menu
        Time.timeScale = 1f;        // Volta o jogo
        isPaused = false;
        // REATIVA os comandos do Player
        player.AtivaInput();

    }

    public void QuitToMenu()
    {
        pauseMenu.SetActive(false);
        GameController.gc.RetirarScreen();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // Nome da sua cena
    }
}