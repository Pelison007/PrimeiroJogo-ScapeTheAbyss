using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private PlayerHealth playerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Pega o PlayerHealth do Player que colidiu
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.vivo)
            {
                // Zera a vida
                playerHealth.vidaAtual = 0;
                if (playerHealth.healthBar != null)
                    playerHealth.healthBar.SetHealth(playerHealth.vidaAtual);
                // Chama a função de morte do Player
                playerHealth.LoseLife();
            }
        }
    }
}
