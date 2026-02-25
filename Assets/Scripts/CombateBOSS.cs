using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class CombateBOSS : MonoBehaviour
{
    [Header("Ataque de Perto")]
    public float attackRangePerto = 2f;
    public float attackCooldownPerto = 1f;
    private float lastAttackPerto;
    public int danoMelle = 20;

    [Header("Ataque de Longe")]
    public float attackRangeLonge = 5f;
    public float attackCooldownLonge = 3f;
    private float lastAttackLonge;
    public float velocidadeMagia = 5f;
    [SerializeField]public int danoMagia= 40;
    public Transform SwpawnProjetilGuspe;
    public GameObject PrefabGuspe;

    public bool isAttacking = false;
    private Transform player;
    private Animator anim;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
    }

    // Chamada pela IA para atacar
    public void AttackMelee()
    {
        if (player == null || isAttacking || Time.time < lastAttackPerto + attackCooldownPerto) return;

        lastAttackPerto = Time.time;
        isAttacking = true;
        anim.SetTrigger("BossAtkPerto");
        AudioManager.instance.Play("AtkPertoBoss");

        // **Não aplicar dano aqui!**
        Debug.Log("Boss começou ataque de perto!");
    }

    // Chamada via Animation Event
    public void ApplyMeleeDamage()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRangePerto)
        {
            Health health = player.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(danoMelle);
                Debug.Log("Dano de perto aplicado no frame certo!");
            }
        }
    }

    public void AttacMagia()
    {
        if (player == null || isAttacking || Time.time < lastAttackLonge + attackCooldownLonge) return;


        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRangeLonge)
        {
            lastAttackLonge = Time.time;
            isAttacking = true;
            anim.SetTrigger("BossAtkMagia");
            AudioManager.instance.Play("AtkLongeBoss");
            Debug.Log("Lançou Guspe");

            // Instancia o projétil da magia
            GameObject projetil = Instantiate(PrefabGuspe, SwpawnProjetilGuspe.position, Quaternion.identity);


            // Calcula direção em direção ao player
            Vector2 direction = (player.position - SwpawnProjetilGuspe.position).normalized;

            // Vira horizontalmente dependendo da direção X
            if (direction.x < 0)
                projetil.transform.localScale = new Vector3(-1, 1, 1); // vira para esquerda
            else
                projetil.transform.localScale = new Vector3(1, 1, 1);  // fica normal (direita)

            // Pega o script correto do projétil
            ProjetilGuspee projectileScript = projetil.GetComponent<ProjetilGuspee>();

            // Inicializa com valores do Boss
            projectileScript.Initialize(direction, velocidadeMagia, danoMagia);


            Debug.Log("Boss atacou de longe!");
        }
    }

    // Chamado no final da animação via Animation Event
    public void EndAttack()
    {
        isAttacking = false;
    }


}
