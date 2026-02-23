using UnityEngine;

public class ProjetilGuspee : MonoBehaviour
{
    private float speed;
    private int damage;
    private Vector2 direction;
    private Rigidbody2D rb;

    // Inicializa o projétil com os valores do Boss
    public void Initialize(Vector2 dir, float projSpeed, int projDamage)
    {
        direction = dir.normalized;
        speed = projSpeed;
        damage = projDamage;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health health = collision.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage); // aplica o dano recebido do Boss
                Debug.Log("Guspe causou " + damage + " de Dano no Player");
            }


            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            Destroy(gameObject);
        }

    }
}
