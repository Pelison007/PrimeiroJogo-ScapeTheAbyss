using Unity.VisualScripting;
using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    public float vidaMaxima = 100;
    public float vidaAtual;

    public HealthBar healthBar; // arraste no Inspector
    protected Coroutine healCoroutine; // guarda a coroutine atual

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        vidaAtual = vidaMaxima;
    }

    private void Update()
    {
        if (healthBar != null)
            healthBar.SetHealth(vidaAtual);
    }
    public virtual void TakeDamage(int dano)
    {
        vidaAtual -= dano;
        Debug.Log($"{gameObject.name} tomou {dano} de dano. Vida atual: {vidaAtual}");

        if (vidaAtual <= 0)
        {
            morto();
        }
    }

    public virtual void Heal(float quantidade)
    {
        vidaAtual += quantidade;
        if (vidaAtual > vidaMaxima)
            vidaAtual = vidaMaxima;

        Debug.Log($"{gameObject.name} recebeu {quantidade} de cura. Vida atual: {vidaAtual}");
    }

    public void HealOverTime(float totalCura, float duracao)
    {
        // Para qualquer cura anterior antes de iniciar nova
        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
            healCoroutine = null;
        }

        healCoroutine = StartCoroutine(HealCoroutine(totalCura, duracao));
    }

    private IEnumerator HealCoroutine(float totalCura, float duracao)
    {
        float curacaoRestante = totalCura;
        float taxa = totalCura / duracao;

        while (curacaoRestante > 0)
        {
            float delta = Mathf.Min(taxa * Time.deltaTime, curacaoRestante);
            vidaAtual += delta;

            if (vidaAtual > vidaMaxima)
                vidaAtual = vidaMaxima;

            curacaoRestante -= delta;
            // Atualiza a barra a cada frame
            if (healthBar != null)
                healthBar.SetHealth(vidaAtual);

            yield return null;
        }
    }

    protected void StopHealing()
    {
        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
            healCoroutine = null;
        }
    }

    protected virtual void morto()
    {
        Debug.Log($"{gameObject.name} morreu.");
        Destroy(gameObject);
    }
}
