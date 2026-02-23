using Unity.VisualScripting;
using UnityEngine;

public class TrovaoSkill : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool bateu = false;

    
    public float speed = 20f;
    [SerializeField] public float cooldownSkill = 5f;
    private float ultimoUso = -Mathf.Infinity;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * speed;
    }

    void Update()
    {
        if (!bateu)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

    public bool CanUse()
    {
        return Time.time >= ultimoUso + cooldownSkill;
    }

    public void TriggerCooldown()
    {
        ultimoUso = Time.time;
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
        }
    }

    public void DestruirSkill()
    {
        Destroy(gameObject);
    }
}
