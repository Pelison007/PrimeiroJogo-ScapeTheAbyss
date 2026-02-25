using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    private HealthBar healthBarr;
    public static Porta porta;
    // Dentro da Unity
    public TextMeshProUGUI SpiritText;
    public TextMeshProUGUI ContagemEsqueletoText;
    public TextMeshProUGUI LifeText;
    public TextMeshProUGUI NextLevelText;
    public TextMeshProUGUI UltimaFaseText;
    public TextMeshProUGUI VoltarFaseText;
    public Image ImagemSpirit;
    public Image ImagemEsqueleto;
    public Image ImagemLife;
    // variaveis ajustaveis dentro da engine.
    public int lifes = 3;
    // variaveis pros textos
    public int Spirit = 0;
    public int TotalSpirit = 0;
    public int esqueleto = 0;
    public int totalEsqueleto = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        AudioManager.instance.Stop("menu");
        AudioManager.instance.Play("Principal");

        if(gc == null)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(gc != this)
        {
            Destroy(gameObject);
        }
        Debug.Log("GameController iniciou");

        int faseAtual = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        NextLevelText.text = $"Um passo a menos para a superficie - Fase {faseAtual}";
        VoltarFaseText.text = $"Descendo... Fase {faseAtual}";
        healthBarr = HealthBar.HealthBarr; 
        TotalSpirit = GameObject.FindGameObjectsWithTag("Spirit").Length;
        totalEsqueleto = GameObject.FindGameObjectsWithTag("Inimigo").Length;
        MostrarScreen();
        RefreshScreen();
    }

    public void SetLives(int life)
    {
        lifes += life;
        if (lifes >= 0) 
        RefreshScreen();
    }

    public void RefreshScreen()
    {
        SpiritText.text = Spirit.ToString() + " / " + TotalSpirit.ToString();
        ContagemEsqueletoText.text = esqueleto.ToString() + " / " + totalEsqueleto.ToString();
        LifeText.text = lifes.ToString();
    }

    public void MostrarScreen()
    {
        GameController.gc.SpiritText.gameObject.SetActive(true); // tira texto de spirito coletados
        GameController.gc.ContagemEsqueletoText.gameObject.SetActive(true); // tira texto de Esqueletos destruidos
        GameController.gc.LifeText.gameObject.SetActive(true); // tira texto de vidas restantes
        GameController.gc.ImagemSpirit.gameObject.SetActive(true); // tira imagem spiritos coletados
        GameController.gc.ImagemEsqueleto.gameObject.SetActive(true); // tira imagem de esqueletos destruidos
        GameController.gc.ImagemLife.gameObject.SetActive(true); // tira imagem de vidas restantes

        if (healthBarr != null)
            healthBarr.gameObject.SetActive(true);
    }
    public void RetirarScreen()
    {
        GameController.gc.SpiritText.gameObject.SetActive(false); // tira texto de spirito coletados
        GameController.gc.ContagemEsqueletoText.gameObject.SetActive(false); // tira texto de Esqueletos destruidos
        GameController.gc.LifeText.gameObject.SetActive(false); // tira texto de vidas restantes
        GameController.gc.ImagemSpirit.gameObject.SetActive(false); // tira imagem spiritos coletados
        GameController.gc.ImagemEsqueleto.gameObject.SetActive(false); // tira imagem de esqueletos destruidos
        GameController.gc.ImagemLife.gameObject.SetActive(false); // tira imagem de vidas restantes
    }
    public void MostrarTextoFase(string mensagem)
    {
        // Pode usar NextLevelText ou outro Text/TMP_Text específico
        NextLevelText.text = mensagem;
        NextLevelText.gameObject.SetActive(true); // garante que aparece
    }
    public void MostrarTextoFaseTemporario(string mensagem, float segundos)
    {
        MostrarTextoFase(mensagem);
        StartCoroutine(SumirTexto(3));
    }

    private IEnumerator SumirTexto(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        NextLevelText.gameObject.SetActive(false);
    }
}
