using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    static GameController gc;

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
        SceneManager.LoadScene(cena);
        
    }
    public void Quit()
    {
        Application.Quit();
    }
}
