using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public enum ItemType
{
    GuardaChuva,
    TrovaoSkill,
    GameOver
};

public class ItemPickup : MonoBehaviour
{
    [Header("Configuração do Item")]
    public ItemType itemType;

    [Header("UI")]
    public Menu menu;
    public GameObject pickupCanvas;
    public GameObject mensagem;
    public TMP_Text textComponent;
    public Button[] buttons;

    [Header("Configuração Texto")]
    public float typingSpeed = 0.05f;
    public float tempoEntreFrases = 2f;

    private Coroutine mensagemCoroutine;
    private Player player;
    private bool isTyping = false;

    public BossIA bossIA;
    


    // ===============================
    // COLETA GUARDA-CHUVA
    // ===============================
    public void ColetarGuardaChuva(Player player)
    {
        Time.timeScale = 0f;

        if (mensagem != null)
            mensagem.SetActive(true);

        player.AguardarConfirmacaoGuardaChuva();
    }

    // ===============================
    // COLETA SKILL TROVÃO
    // ===============================
    public void ColetarTrovaoSkill(Player player)
    {
        player.temTrovao = true;
        player.SalvarPlayer(); // 👈 ESSENCIAL
        string[] mensagens =
        {
            "EU, Grande Guerreiro, Fui derrotado... mas o meu poder não se perdeu.",
            "Pegue o meu <color=yellow>Feitiço TrovãoSagrado</color>, a força que derreteu tantos inimigos!",
            "Aperte <color=red>R </color>para aceitar o poder!"
        };

        MostrarMensagem(mensagens, true, () =>
        {
            player.AtivaInput();
        });

        player.DesativaInput();
    }

    public void GameOver(Player player)
    {
        string[] mensagens;
        
        if (!bossIA.vivo)
        {
            
            mensagens = new string[]
            {
            "Então... foi você que conseguiu.",
            "Eu não estava lutando por vontade própria. A escuridão estava me controlando.",
            "Obrigado por me libertar. Agora posso descansar em paz."

            };

            player.DesativaInput();

            MostrarMensagem(mensagens, false, () =>
            {
                SceneManager.LoadScene("GameOver");
            });

            foreach (Button btn in buttons)
            {
                btn.gameObject.SetActive(true); // Mostra cada botão
            }
        }

        else
        {
            mensagens = new string[]
            {
            "Derrote o monstro para que eu possa descansar em paz...!"
            };

            foreach (Button btn in buttons)
            {
                buttons[0].gameObject.SetActive(false);
            }
        }
        player.DesativaInput();

        MostrarMensagem(mensagens, true, () =>
        {
            player.AtivaInput();
        });
    }

    // ===============================
    // TRIGGER
    // ===============================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.GetComponent<Player>();

        if (pickupCanvas != null)
            pickupCanvas.SetActive(true);

        player.SetNearbyItem(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (pickupCanvas != null)
            pickupCanvas.SetActive(false);

        if (player != null)
            player.ClearNearbyItem(this);

        player = null;
    }

    // ===============================
    // COROUTINE PRINCIPAL
    // ===============================
    

    // ===============================
    // EFEITO DIGITAÇÃO
    // ===============================
    private IEnumerator TypeTextCoroutine(string message)
    {
        textComponent.text = message;
        textComponent.ForceMeshUpdate();

        textComponent.maxVisibleCharacters = 0;

        int totalVisibleCharacters = textComponent.textInfo.characterCount;

        while (textComponent.maxVisibleCharacters < totalVisibleCharacters)
        {
            textComponent.maxVisibleCharacters++;

            AudioManager.instance.Play("type");

            yield return new WaitForSecondsRealtime(typingSpeed);

            if (textComponent.maxVisibleCharacters == totalVisibleCharacters)
                AudioManager.instance.Stop("type");
        }
        
    }

    public void MostrarMensagem(string[] mensagens, bool esperarTecla, System.Action onFinish = null)
    {
        if (mensagemCoroutine != null)
            StopCoroutine(mensagemCoroutine);

        mensagemCoroutine = StartCoroutine(MessageCoroutine(mensagens, esperarTecla, onFinish));
    }

    private IEnumerator MessageCoroutine(string[] mensagens, bool esperarTecla, System.Action onFinish)
    {
        isTyping = true;
        

        mensagem.SetActive(true);

        for (int i = 0; i < mensagens.Length; i++)
        {
            yield return StartCoroutine(TypeTextCoroutine(mensagens[i]));

            if (i < mensagens.Length - 1)
            {
                yield return new WaitForSecondsRealtime(tempoEntreFrases);
            }
            else
            {
                if (esperarTecla)
                {
                    bool teclaApertada = false;

                    // Loop até o jogador apertar R
                    while (!teclaApertada)
                    {
                        if (Keyboard.current.rKey.wasPressedThisFrame)
                        {
                            teclaApertada = true;
                        }
                        yield return null;
                    }
                }
                else
                {
                    yield return new WaitForSecondsRealtime(2f);
                }
            }
        }
        
        textComponent.text = "";
        mensagem.SetActive(false);

        
        isTyping = false;

        onFinish?.Invoke(); // EXECUTA AÇÃO FINAL
    }
    public void LoadScenes(string cena)
    {
        SceneManager.LoadScene(cena);
    }

}