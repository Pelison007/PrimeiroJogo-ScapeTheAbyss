using Unity.VisualScripting;
using UnityEngine;

public class DanoSkill : MonoBehaviour
{
    private TrovaoSkill trovaoSkill;
    AnimationEventsTrovao AnimationEvent;
    private Rigidbody2D rb;
    private bool bateu = false;

    private float speed;
    private int damage;

    public void Setup(int newDamage, float newSpeed)
    {
        damage = newDamage;
        speed = newSpeed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * speed;
        AnimationEvent = GetComponent<AnimationEventsTrovao>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground") && !bateu)
        {
            bateu = true;

            ContactPoint2D contact = collision.GetContact(0);

            float halfHeight = GetComponent<Collider2D>().bounds.extents.y;

            transform.position = new Vector3(
                transform.position.x,
                contact.point.y + halfHeight,
                transform.position.z
            );

            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            Debug.Log("Colidiu com: " + collision.gameObject.name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        EnemyHealtBOSS enemyBoss = other.GetComponentInParent<EnemyHealtBOSS>();

        if (enemy != null)
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            Debug.Log("Dano do TROVAO aplicado ao inimigo: " + other.gameObject.name);
        }
        if (enemyBoss != null)
        {
            other.GetComponent<EnemyHealtBOSS>().TakeDamage(damage);
            Debug.Log("Dano do TROVAO aplicado ao inimigo: " + other.gameObject.name);
        }
    }


}
