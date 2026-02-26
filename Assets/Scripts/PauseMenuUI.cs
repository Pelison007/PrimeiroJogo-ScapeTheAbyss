using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Referências")]
    public Player player;
    public GameObject pauseMenu;

    [Header("Input System")]
    public PlayerInput playerInput;

    [HideInInspector]
    public bool isPaused = false;

    private void OnEnable()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            var pauseAction = playerInput.actions["Pause"];
            if (pauseAction != null)
                pauseAction.performed += TogglePause;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null && playerInput.actions != null)
        {
            var pauseAction = playerInput.actions["Pause"];
            if (pauseAction != null)
                pauseAction.performed -= TogglePause;
        }
    }

    private void TogglePause(InputAction.CallbackContext ctx)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        if (player != null)
            player.DesativaInput();
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        if (player != null)
            player.AtivaInput();
    }

    public void QuitToMenu()
    {
        if (player != null)
            player.SalvarPlayer();

        if (GameController.gc != null)
        {
            GameController.gc.SalvarProgresso();
            GameController.gc.RetirarScreen();
        }

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        if (AudioManager.instance != null)
            AudioManager.instance.Stop("Principal");

        SceneManager.LoadScene("Menu");
    }
}