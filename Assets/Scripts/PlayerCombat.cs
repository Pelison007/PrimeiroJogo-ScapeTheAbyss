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
    [SerializeField] private LayerMask ground;
    private TrovaoSkill trovaoSkill;
    // -------------------
    public AudioSource SomPlayer;
    public AudioClip ataque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float displayTime = 0.5f; // tempo que o range ficará visível

    // habilidade


    void Start()
    {
        animacao = GetComponent<Animator>();
        player = GetComponent<Player>();
        trovaoSkill = GetComponent<TrovaoSkill>();
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

    public void SpawnSkill(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = -mainCamera.transform.position.z;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        float distance = Vector2.Distance(transform.position, mouseWorldPos);

        // Limitar a distância ao range máximo
        if (distance > trovaoSkill.skillRange)
        {
            mouseWorldPos = (Vector2)transform.position + direction * trovaoSkill.skillRange;
            distance = trovaoSkill.skillRange;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, ground);
        Debug.DrawRay(transform.position, direction * distance, Color.red, 2f);

        Vector3 finalPoint = mouseWorldPos;

        if (hit.collider != null)
        {
            finalPoint = hit.point;
        }

        finalPoint.y += 5f; // altura do céu

        if (trovaoSkill.CanUse())
        {
            GameObject obj = Instantiate(skillPrefab, finalPoint, Quaternion.identity);

            DanoSkill dano = obj.GetComponent<DanoSkill>();
            dano.Setup(trovaoSkill.damage, trovaoSkill.speed);

            trovaoSkill.TriggerCooldown();
            Debug.Log("Trovao usado!");
        }
    }

}
