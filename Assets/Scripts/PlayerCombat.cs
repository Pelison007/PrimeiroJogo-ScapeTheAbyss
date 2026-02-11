using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Animator animacao;
    private Player player;
    // VARIAVEIS DE ATAQUE
    public GameObject attackHitBox;
    public float cooldown = 0.5f;
    private float proximoAtaque;
    // -------------------
    public AudioSource SomPlayer;
    public AudioClip ataque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animacao = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    public void ResetarHitBox()
    {
        if (attackHitBox != null)
        {
            attackHitBox.GetComponent<AttackHitBox>().ResetarDano();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        else if (Time.time < proximoAtaque) { 
            return;
        }
        else {
            animacao.SetTrigger("Ataque");
            attackHitBox.GetComponent<AttackHitBox>().ResetarDano();
            // ativa hit box, e desativa dps de 0.2f
            // ----------------------------
            SomPlayer.PlayOneShot(ataque);
            proximoAtaque = Time.time + cooldown;
            Debug.Log("Atacou!");
        }
    }

}
