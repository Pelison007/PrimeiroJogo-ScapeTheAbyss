using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ItemType
{
    GuardaChuva,
    TrovaoSkill
}

public class ItemPickup : MonoBehaviour
{
    [Header("Configuração do Item")]
    public ItemType itemType;

    [Header("UI")]
    public GameObject pickupCanvas;
    public GameObject mensagem;
    public TMP_Text textComponent;

    [Header("Configuração Texto")]
    public float typingSpeed = 0.05f;
    public float tempoEntreFrases = 2f;

    private Coroutine mensagemCoroutine;
    private Player player;
    private bool isTyping = false;

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
        if (isTyping) return;

        isTyping = true; // trava imediatamente

        player.DesativaInput();

        if (mensagem != null)
            mensagem.SetActive(true);

        // Se já existir coroutine rodando, para ela
        if (mensagemCoroutine != null)
            StopCoroutine(mensagemCoroutine);

        mensagemCoroutine = StartCoroutine(EpicMessageCoroutine(player));
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
    private IEnumerator EpicMessageCoroutine(Player player)
    {
        isTyping = true;
        Time.timeScale = 0f;

        string[] mensagens = new string[]
        {
            "EU, Grande Guerreiro, Fui derrotado... mas o meu poder não se perdeu.",
            "Pegue o meu Feitiço TrovãoSagrado, a força que derreteu tantos inimigos!",
            "Aperte R para aceitar o poder!"
        };

        for (int i = 0; i < mensagens.Length; i++)
        {
            yield return StartCoroutine(TypeTextCoroutine(mensagens[i]));

            // Se não for a última mensagem → passa automaticamente
            if (i < mensagens.Length - 1)
            {
                yield return new WaitForSecondsRealtime(tempoEntreFrases);
            }
            else
            {
                // Última mensagem → espera apertar R
                while (!Keyboard.current.rKey.wasPressedThisFrame)
                {
                    yield return null;
                }
            }

        }

        // Finaliza tudo
        textComponent.text = "";
        mensagem.SetActive(false);

        Time.timeScale = 1f;
        player.AtivaInput();

        mensagemCoroutine = null;
        isTyping = false;
    }

    // ===============================
    // EFEITO DIGITAÇÃO
    // ===============================
    private IEnumerator TypeTextCoroutine(string message)
    {
        textComponent.text = "";

        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}