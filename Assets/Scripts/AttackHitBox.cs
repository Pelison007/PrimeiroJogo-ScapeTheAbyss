using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    // dano de ataque
    public int Dano = 50;
    private bool jaDeuDano;

    public void ResetarDano()
    {
        jaDeuDano = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hitbox tocou em: " + other.name);
        if (!other.CompareTag("Inimigo")) return;
       

        if (jaDeuDano) return;

        // pega a vida do objeto que entra em contato / Other sgnifica o objeto que entra em contato com o hitbox

        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            
            health.TakeDamage(Dano);
            jaDeuDano = true;

            AudioManager.instance.Play("AcertouEsqueleto");


            Debug.Log("Causou " + Dano + " de dano em " + other.name);
        }
    }

}
