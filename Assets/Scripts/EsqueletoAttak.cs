using UnityEngine;

public class EsqueletoAttak : MonoBehaviour
{
    public EsqueletoAttak hitbox;   // arraste o hitbox no Inspector
    public int Dano = 20;
    public AudioSource audioS;
    public AudioClip hitSound;
    public AudioClip Naoacerto;

    [HideInInspector]
    public bool acertouPlayer = false;

    private bool jaDeuDano;

    void OnEnable()
    {
        jaDeuDano = false;
        acertouPlayer = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (jaDeuDano) return;

        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(Dano);
                jaDeuDano = true;
                acertouPlayer = true;

                if (audioS != null && hitSound != null)
                    audioS.PlayOneShot(hitSound, 1f);
            }
        }
    }

}
