using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Transform pontoRespawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Player player = collision.GetComponent<Player>();
        if (player == null)
            return;

        if (pontoRespawn != null)
        {
            player.AtualizaRespawn(pontoRespawn.position);
            Debug.Log("Checkpoint ativado");
        }
        else
        {
            Debug.LogWarning("Checkpoint sem pontoRespawn configurado");
        }
    }
}