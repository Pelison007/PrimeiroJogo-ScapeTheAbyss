using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool isRestarting = false;
    public Transform spawnFase1; // arraste aqui o ponto de spawn da fase 1 no Inspector
    public bool ultimaEntradaFoiDireita;
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

            // Se for a primeira vez que roda o jogo, seta spawn inicial
            if (!PlayerPrefs.HasKey("UltimoRespawnX"))
            {
                PlayerPrefs.SetFloat("UltimoRespawnX", spawnFase1.position.x);
                PlayerPrefs.SetFloat("UltimoRespawnY", spawnFase1.position.y);
                PlayerPrefs.Save();
            }

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
            // Pega o player
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.GetComponent<Player>();
            }

            // Move o player para o último checkpoint
            if (player != null)
            {
                player.CarregarPlayer(); // carrega dados e ultimoRespawn
                if (!player.ignorarUltimoRespawn)
                {
                    player.transform.position = player.ultimoRespawn;
                    Debug.Log("Player movido para último checkpoint: " + player.ultimoRespawn);
                }
            }

            if (FaseManager.fm != null && player != null)
            {
                FaseManager.fm.IrParaFase(faseAtual, player.transform, ultimaEntradaFoiDireita);
            }

            // Atualiza contagem de objetos
            TotalSpirit = GameObject.FindGameObjectsWithTag("Spirit").Length;
            totalEsqueleto = GameObject.FindGameObjectsWithTag("Inimigo").Length;

            // Carrega variáveis do progresso
            Spirit = PlayerPrefs.GetInt("Spirit", Spirit);
            esqueleto = PlayerPrefs.GetInt("Esqueleto", esqueleto);
            lifes = PlayerPrefs.GetInt("Lifes", lifes);

            RefreshScreen();
            MostrarScreen();
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
        PlayerPrefs.SetInt("UltimaEntrada", ultimaEntradaFoiDireita ? 1 : 0);

        PlayerPrefs.SetInt("FaseAtual", faseAtual);

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
        ultimaEntradaFoiDireita = PlayerPrefs.GetInt("UltimaEntrada", 0) == 1;

        faseAtual = PlayerPrefs.GetInt("FaseAtual", 0);
    }

    public void RestartGame()
    {
        // Limpa todo progresso
        PlayerPrefs.DeleteAll();

        // Reseta fase atual
        faseAtual = 0;

        // Ignora checkpoint antigo
        if (player != null)
            player.ignorarUltimoRespawn = true;

        // Carrega a fase
        SceneManager.LoadScene("Fase 1");
    }

    // ------------------- Vidas -------------------
    public void SetLives(int life)
    {
        lifes += life;
        if (lifes < 0) lifes = 0;
        RefreshScreen();
    }


}
