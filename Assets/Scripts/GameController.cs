using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int faseAtual;
    public static GameController gc;
    private HealthBar healthBarr;
    public static Porta porta;
    public Player player;
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
    private void Awake()
    {
        // Singleton
        if (gc == null)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Carrega progresso geral
            CarregarProgresso();
        }
        else if (gc != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Contains("Menu"))
        {
            // Pega o player da cena
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.GetComponent<Player>();
            }
            if (player != null)
            {
                player.CarregarPlayer(); // aplica variáveis salvas
                player.transform.position = player.ultimoRespawn; // aplica checkpoint
            }

            // Conta todos os objetos da cena (ativos)
            TotalSpirit = GameObject.FindGameObjectsWithTag("Spirit").Length;
            totalEsqueleto = GameObject.FindGameObjectsWithTag("Inimigo").Length;

            // Carrega variáveis que mudaram, mas não o total
            Spirit = PlayerPrefs.GetInt("Spirit", Spirit);
            esqueleto = PlayerPrefs.GetInt("Esqueleto", esqueleto);
            lifes = PlayerPrefs.GetInt("Lifes", lifes);

            RefreshScreen();
            MostrarScreen();
            Debug.Log("Cena carregada: " + scene.name);
            Debug.Log("Total Spirit na cena: " + TotalSpirit);
            Debug.Log("Total Esqueleto na cena: " + totalEsqueleto);
        }
    }

    public void RefreshScreen()
    {
        SpiritText.text = Spirit + " / " + TotalSpirit;
        ContagemEsqueletoText.text = esqueleto + " / " + totalEsqueleto;
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

    // ------------------- Salvar e Carregar -------------------
    public void SalvarProgresso()
    {
        PlayerPrefs.SetInt("Spirit", Spirit);
        PlayerPrefs.SetInt("TotalSpirit", TotalSpirit);
        PlayerPrefs.SetInt("Esqueleto", esqueleto);
        PlayerPrefs.SetInt("TotalEsqueleto", totalEsqueleto);
        PlayerPrefs.SetInt("Lifes", lifes);
        PlayerPrefs.SetInt("FaseAtual", faseAtual);

        PlayerPrefs.SetInt("FaseAtual", SceneManager.GetActiveScene().buildIndex);

        if (player != null)
            player.SalvarPlayer();

        PlayerPrefs.Save();
        Debug.Log("Progresso salvo!");
    }

    public void CarregarProgresso()
    {
        Spirit = PlayerPrefs.GetInt("Spirit", Spirit);
        TotalSpirit = PlayerPrefs.GetInt("TotalSpirit", TotalSpirit);
        esqueleto = PlayerPrefs.GetInt("Esqueleto", esqueleto);
        totalEsqueleto = PlayerPrefs.GetInt("TotalEsqueleto", totalEsqueleto);
        lifes = PlayerPrefs.GetInt("Lifes", lifes);

        faseAtual = PlayerPrefs.GetInt("FaseAtual", 0);
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll(); // apaga todo o progresso
        SceneManager.LoadScene("GameOver"); // carrega primeira fase
        Debug.Log("Jogo reiniciado!");
    }

    // ------------------- Vidas -------------------
    public void SetLives(int life)
    {
        lifes += life;
        if (lifes < 0) lifes = 0;
        RefreshScreen();
    }

}
