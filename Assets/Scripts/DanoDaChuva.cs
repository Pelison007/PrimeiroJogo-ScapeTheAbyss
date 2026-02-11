using UnityEngine;

public class DanoDaChuva : MonoBehaviour
{

    Player player;
    public int dano = 10;
    private bool ativaParaDano = true;

    [Header("Audio")]
    public AudioSource audioS;       // AudioSource no prefab da gota

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bateu no chão
        if (collision.CompareTag("chao"))
        {
            ativaParaDano = false;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
            }
            Destroy(gameObject, 0.8f);
            return;
        }

        // Bateu no guarda-chuva
        if (collision.CompareTag("GuardaChuva"))
        {
            Player player = collision.GetComponentInParent<Player>();
            if (player != null && player.GuardaChuvaAberto)
            {
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.gravityScale = 0f;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                transform.parent = collision.transform;
                Destroy(gameObject, 0.8f);
                return; // Sai do método aqui → não aplica dano
            }
        }

        // Agora aplicamos dano só se não bateu no guarda-chuva
        if (!ativaParaDano)
            return;

        Player playerHit = collision.GetComponentInParent<Player>();
        if (playerHit != null)
        {
            PlayerHealth health = playerHit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(dano);
                if (audioS != null)
                    playerHit.PlayerSound(9);
            }

            // Para a gota ao colidir com o Player
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            transform.parent = collision.transform;
            Destroy(gameObject, 1.3f);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
