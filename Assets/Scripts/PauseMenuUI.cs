using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public Player player;
    public GameObject pauseMenu;
    public bool isPaused = false;

    public PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput.actions["Pause"].performed += TogglePause; // “Pause” é o nome da action no InputActionAsset
    }

    private void OnDisable()
    {
        playerInput.actions["Pause"].performed -= TogglePause;
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        player.DesativaInput();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        player.AtivaInput();
    }

    public void QuitToMenu()
    {
        // Salva progresso do player
        if (player != null)
            player.SalvarPlayer();

        // Salva progresso do GameController
        GameController.gc.SalvarProgresso();


        pauseMenu.SetActive(false);
        GameController.gc.RetirarScreen();
        Time.timeScale = 1f;
        AudioManager.instance.Stop("Principal");
        SceneManager.LoadScene("Menu");
    }
}