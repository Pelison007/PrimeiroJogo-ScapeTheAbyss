using UnityEngine;

public class BossAnimatedEvents : MonoBehaviour 
{ 

    CombateBOSS combateBoss;

    void Awake()
    {
        combateBoss = GetComponentInParent<CombateBOSS>();
    }

    // CHAMADO NO ÚLTIMO FRAME
    public void EndAttack()
    {
        if (combateBoss != null)
        {
            Debug.Log("Esta diferente de nulo");
            combateBoss.EndAttack();
        }
        else
        {
            Debug.Log("Esta nulo");
        }
    }

    public void ApplyMeleeDamage() 
    {  
        if (combateBoss != null)
        {
            combateBoss.ApplyMeleeDamage();
        }
    }

    //public void TocarSomAtaque()
    //{
    //    if (bossIA.acertouPlayer)
    //    {
    //         já tocou som de acerto no hitbox, então não faz nada
    //        return;
    //    }

    //     se não acertou nada, toca som de erro
    //    if (bossIA.audioS != null && hitbox.Naoacerto != null)
    //    {
    //        hitbox.audioS.PlayOneShot(hitbox.Naoacerto, 1f);
    //    }
    //}
}
