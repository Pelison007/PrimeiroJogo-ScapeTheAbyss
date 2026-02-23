using UnityEngine;

public class EnemyHealtBOSS : Health
{
    public BossIA bossIA;

    public float curaAoDesagregar = 30;
    public float duracaoCuraFixa = 10f;

    private bool curando = false; // flag para saber se está curando

    protected override void Awake()
    {
        // Chama o Awake do pai (Health) para definir vidaAtual e vidaMaxima
        base.Awake();

        // Configura a barra de vida no início
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(vidaMaxima);
        }
    }
    public override void TakeDamage(int dano)
    {
        // Para a cura se estiver em andamento
        if (curando)
        {
            StopHealing(); // método da classe base que para a coroutine
            curando = false;
            Debug.Log("Cura interrompida pelo dano!");
        }

        base.TakeDamage(dano); // Executa a lógica de diminuir vida do script pai

        // Atualiza a barra de vida do inimigo
        if (healthBar != null)
        {
            healthBar.SetHealth(vidaAtual);
        }
    }

    // Método exclusivo do boss para curar quando desagraga
    public void CurarAoDesagregar()
    {
        if (!curando)
        {
            float vidaFaltando = vidaMaxima - vidaAtual;
            if (vidaFaltando > 0)
            {
                curando = true;
                HealOverTime(vidaFaltando, duracaoCuraFixa);
                Debug.Log("Boss começou a curar até a vida máxima!");
            }
        }
    }

    protected override void morto()
    {
        if (bossIA != null)
        {
            bossIA.Die();
            Destroy(healthBar.gameObject, 8f);
            Destroy(bossIA.gameObject, 8f);
        }
    }
}
