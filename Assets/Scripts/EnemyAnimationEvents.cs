using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyAnimationEvents : MonoBehaviour
{
    public EsqueletoAttak hitbox;
    EnemyAI enemy;

    void Awake()
    {
        enemy = GetComponentInParent<EnemyAI>();
    }

    // CHAMADO NO ÚLTIMO FRAME
    public void OnAttackEnd()
    {
        if (enemy != null)
            enemy.OnAttackEnd();
    }

    public void TocarSomAtaque()
    {
        if (hitbox.acertouPlayer)
        {
            // já tocou som de acerto no hitbox, então não faz nada
            return;
        }

        // se não acertou nada, toca som de erro
        if (hitbox.acertouPlayer == false)
        {
            AudioManager.instance.Play("AtkEsqueleto");
        }
    }
}
