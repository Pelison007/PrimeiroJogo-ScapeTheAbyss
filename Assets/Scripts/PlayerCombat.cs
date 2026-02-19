using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Camera mainCamera;
    private Animator animacao;
    private Player player;
    // VARIAVEIS DE ATAQUE
    [Header("Ataque Automatico")]
    public GameObject attackHitBox;
    public float cooldown = 0.5f;
    private float proximoAtaque;
    // ---------------------------
    [Header("Ataque Habilidade")]
    [SerializeField] private GameObject skillPrefab;

    // -------------------
    public AudioSource SomPlayer;
    public AudioClip ataque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // habilidade

    
    void Start()
    {
        animacao = GetComponent<Animator>();
        player = GetComponent<Player>();
        mainCamera = Camera.main;
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

            SomPlayer.PlayOneShot(ataque);
            proximoAtaque = Time.time + cooldown;
            Debug.Log("Atacou!");
        }
    }
    public void CastSkill(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        animacao.SetTrigger("Cast");
    }


}
