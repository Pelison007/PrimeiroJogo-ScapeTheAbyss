using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName;

    [Header("UI")]
    public GameObject pickupCanvas;
    public GameObject mensagemGuardaChuva; // NOVO

    private Player player;

    public void PickUp(Player player)
    {
        pickupCanvas.SetActive(false);
        Debug.Log("Pegou o item: " + itemName);

        if (itemName == "GuardaChuva")
        {
            player.GetUmbrella();

            // pausa o jogo
            Time.timeScale = 0f;
            Debug.Log("MOSTRAR MENSAGEM GUARDA-CHUVA");
            // mostra a mensagem
            mensagemGuardaChuva.SetActive(true);

            player.AguardarConfirmacaoGuardaChuva(); // 👈 novo
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            pickupCanvas.SetActive(true);

            player.SetNearbyItem(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pickupCanvas.SetActive(false);

            player.ClearNearbyItem(this);
            player = null;
        }
    }
}
