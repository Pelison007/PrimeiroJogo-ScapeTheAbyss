using UnityEngine;

public class EnemyHealth : Health
{
    public EnemyAI enemyAI;

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
        base.TakeDamage(dano); // Executa a lógica de diminuir vida do script pai

        // Atualiza a barra de vida do inimigo
        if (healthBar != null)
        {
            healthBar.SetHealth(vidaAtual);
        }
    }
    protected override void morto()
    {
        if (enemyAI != null)
        {
            enemyAI.Die();
            Destroy(healthBar.gameObject, 8f);
            Destroy(enemyAI.gameObject, 8f);
        }
    }
}
