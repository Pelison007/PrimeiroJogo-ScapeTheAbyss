using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    static GameController gc;
    [SerializeField] public Player player;
    [SerializeField] public ItemPickup itemPickup;

    void Awake()
        {
            gc = GameController.gc;
        }

    public void LoadScenes(string cena)
    {
        if(cena == "GameOver")
        {
            Time.timeScale = 1f;
            AudioManager.instance.Stop("type");
            AudioManager.instance.Stop("Principal");
            gc.RetirarScreen();
            AudioManager.instance.Play("menu");
        }
        if (cena == "Menu")
        {
            AudioManager.instance.Stop("menu");
            Debug.Log("Parou a musica");
        }
        SceneManager.LoadScene(cena);
    }
    public void Retomar()
    {
        Time.timeScale = 1f;
        itemPickup.mensagem.SetActive(false);
        player.AtivaInput();
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void ReiniciartGame()
    {
        GameController.gc.RestartGame();
    }
}
