using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class BossIA : MonoBehaviour
{
    public float detectionRange = 10f;
    public float moveSpeed = 3f;

    bool inRangePerto;
    bool inRangeLonge;
    //cura ao desagregar
    public int curaAoDesagregar = 20;
    private bool estaAgarrado = false;

    public CombateBOSS combateBoss;
    private EnemyHealtBOSS bossHealth;

    private Rigidbody2D rb;
    private Transform player;
    Animator anim;
    private Vector2 startPosition;
    private bool chasingPlayer = false;
    private bool facingRight = true;
    private float minDistance = 0.5f; // ajuste conforme necessário

    void Start()
    {
        bossHealth = GetComponent<EnemyHealtBOSS>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        
        //procura player pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        combateBoss = GetComponent<CombateBOSS>();
        if (combateBoss == null)
            Debug.LogWarning("combateBoss não encontrado!");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        chasingPlayer = distance <= detectionRange;

        inRangePerto = distance <= combateBoss.attackRangePerto;
        inRangeLonge = distance > combateBoss.attackRangePerto && distance <= combateBoss.attackRangeLonge;


        // Ataque de perto tem prioridade
        if (inRangePerto)
        {
            anim.SetBool("BossAndando", false);
            combateBoss.AttackMelee(); // chama o ataque de perto do script separado
        }

        else if (inRangeLonge) // só ataca longe se não estiver no range de perto
        {
            anim.SetBool("BossAndando", false);
            combateBoss.AttacMagia(); // chama o ataque de longe do script separado
        }

        if (!chasingPlayer)
        {
           bossHealth.CurarAoDesagregar();
           estaAgarrado = true;
        }
    }
    void FixedUpdate()
    {
        if (player == null)
        {
            anim.SetBool("BossAndando", false);
            return;
        }

        // Não se move se estiver atacando
        if (combateBoss.isAttacking)
        {
            return;
        }

        if (chasingPlayer)
        {
            MoveTowards(player.position);
        }
        else
        {
            // Volta para posição inicial
            if (Vector2.Distance(rb.position, startPosition) > 0.05f)
            {
                MoveTowards(startPosition);
            }  
            else
                anim.SetBool("BossAndando", false);
        }
    }


    void MoveTowards(Vector2 target)
    {
        if (Mathf.Abs(target.x - rb.position.x) < 0.05f)
            return;

        // Calcula a distância horizontal entre boss e player
        float distanceX = Mathf.Abs(target.x - rb.position.x);

        // Se estiver dentro da distância mínima, não se move
        if (distanceX < minDistance)
        {
            anim.SetBool("BossAndando", false);
            return;
        }


        float directionX = Mathf.Sign(target.x - rb.position.x);

        Vector2 newPosition = rb.position + Vector2.right * directionX * moveSpeed * Time.fixedDeltaTime;

        if (directionX > 0 && !facingRight)
        {
            Flip();
        }
        if (directionX < 0 && facingRight)
        {
            Flip();
        }

        rb.MovePosition(newPosition);
        anim.SetBool("BossAndando", true);   // animação de Idle
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void RecuperarVida()
    {
        if (bossHealth != null)
        {
            bossHealth.Heal(curaAoDesagregar);
            Debug.Log("Boss curou ao desagragar!");
        }
        else
        {
            Debug.LogWarning("BossHealth não atribuído no BossIA!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, combateBoss.attackRangePerto);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, combateBoss.attackRangeLonge);
    }

}

