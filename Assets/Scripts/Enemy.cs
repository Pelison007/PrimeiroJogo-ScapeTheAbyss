using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI enemyAI;
    bool vivo = true;
    enum State
    {
        Idle,
        Walk,
        Aggro,
        Attack
    }

    State state;
    bool isAttacking = false;

    [Header("Ground & Wall Detection")]
    public Transform groundCheck;    // objeto filho posicionado nos pés do inimigo
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    bool isGrounded;
    bool isFrontGround; // chão à frente
    public Transform frontCheck;    // na frente, perto do chão
    public Transform wallCheck;       // objeto filho posicionado na frente do inimigo
    public float wallCheckDistance = 0.1f;
    bool isWallAhead;



    [Header("Movement")]
    public float moveSpeed = 2f;
    public float walkDistance = 2f;

    [Header("Detection")]
    public float aggroRange = 5f;
    public float attackRange = 1f;

    Rigidbody2D rb;
    Transform player;
    SpriteRenderer sprite;
    Animator anim;

    float startX;
    float walkTargetX;
    float idleTimer;
    float attackCooldown;

    // som
    public AudioSource audioS;
    public AudioClip Morte;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponentInChildren<Animator>();

        startX = rb.position.x;
        PickNewWalkTarget();

        state = State.Idle;
        idleTimer = Random.Range(1f, 2f);
        state = State.Idle;
    }

    void Update()
    {
        if (!vivo) return;
        attackCooldown -= Time.deltaTime;
        float distanceX = Mathf.Abs(player.position.x - rb.position.x);
        float distanceY = Mathf.Abs(player.position.y - rb.position.y);
        float allowedHeight = 1f; // ajuste conforme altura do inimigo

        switch (state)
        {
            case State.Idle:
                HandleIdle(distanceX, distanceY, allowedHeight);
                break;

            case State.Walk:
                HandleWalk(distanceX, distanceY, allowedHeight);
                break;

            case State.Aggro:
                HandleAggro(distanceX, distanceY, allowedHeight);
                break;

            case State.Attack:
                HandleAttack(distanceX, distanceY, allowedHeight);
                break;
        }
    }
    void CheckGround()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (wallCheck != null)
            isWallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * Mathf.Sign(transform.localScale.x), wallCheckDistance, groundLayer);

        if (frontCheck != null)
            isFrontGround = Physics2D.Raycast(frontCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }
    void CheckWall()
    {
        float dir = Mathf.Sign(transform.localScale.x); // direção que ele está virado
        isWallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * dir, wallCheckDistance, groundLayer);

        // debug
        Debug.DrawRay(wallCheck.position, Vector2.right * dir * wallCheckDistance, Color.blue);
    }


    void FixedUpdate()
    {
        CheckWall();
        CheckGround();
        Move();
    }

    // ---------- STATES ----------

    void HandleIdle(float distanceX, float distanceY, float allowedHeight)
    {
        idleTimer -= Time.deltaTime;

        if (distanceX <= aggroRange && distanceY <= allowedHeight)
        {
            state = State.Aggro;
            return;
        }

        if (idleTimer <= 0)
        {
            PickNewWalkTarget();
            state = State.Walk;
        }

        if (anim != null)
        {
            anim.SetBool("Parado", true);   // animação de Idle
            anim.SetBool("Andando", false); // desliga Walk
        }
    }

    void HandleWalk(float distanceX, float distanceY, float allowedHeight)
    {
        if (distanceX <= aggroRange && distanceY <= allowedHeight)
         {
            state = State.Aggro;
            return;
        }

        if (Mathf.Abs(rb.position.x - walkTargetX) < 0.1f)
        {
            idleTimer = Random.Range(1f, 2f);
            state = State.Idle;
        }

        // Adicione animação Walk
        if (anim != null)
        {
            anim.SetBool("Andando", true);
            anim.SetBool("Parado", false);
        }
    }

    void HandleAggro(float distanceX, float distanceY, float allowedHeight)
    {
        if (distanceX > aggroRange || distanceY > allowedHeight)
        {
            PickNewWalkTarget();
            state = State.Walk;
            return;
        }

        if (distanceX <= attackRange && distanceY <= allowedHeight)
        {
            rb.linearVelocity = Vector2.zero;
            state = State.Attack;
            return;
        }
        // animação Aggro (opcional: pode usar Walk ou uma animação específica)
        if (anim != null)
        {
            anim.SetBool("Andando", true);
        }

        walkTargetX = player.position.x;
    }

    void HandleAttack(float distanceX, float distanceY, float allowedHeight)
    {
        if (isAttacking)
            return;

        if (attackCooldown > 0)
            return;

        // só ataca se o player estiver ao lado
        if (distanceX <= attackRange && distanceY <= allowedHeight)
        {
            float dir = Mathf.Sign(player.position.x - rb.position.x);
            HandleFlip(dir);

            isAttacking = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            Attack();
        }
        else
        {
            // se o player saiu do alcance, volta para Aggro ou Walk
            state = (distanceX <= aggroRange && distanceY <= allowedHeight) ? State.Aggro : State.Walk;
        }
    }

    // ---------- MOVEMENT ----------

    void Move()
    {
        float dir = Mathf.Sign(walkTargetX - rb.position.x);

        // --- PATRULHA ---
        if (state == State.Walk)
        {
            // inverte direção só se não houver chão à frente ou houver parede
            if (!isFrontGround || isWallAhead)
            {
                walkTargetX = rb.position.x - (walkTargetX - rb.position.x);
                dir = Mathf.Sign(walkTargetX - rb.position.x);
                HandleFlip(dir);
            }

            // anda
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            HandleFlip(dir);
            return;
        }

        // --- AGGRO ---
        if (state == State.Aggro)
        {
            // só anda se tiver chão abaixo dos pés
            if (isGrounded)
            {
                dir = Mathf.Sign(player.position.x - rb.position.x);
                rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
                HandleFlip(dir);
            }
            else
            {
                // se não tiver chão, para no lugar
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            return;
        }

        // --- outros estados ---
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }



    void PickNewWalkTarget()
    {
        walkTargetX = startX + Random.Range(-walkDistance, walkDistance);
    }

    void HandleFlip(float dir)
    {
        if (dir != 0)
        {
            Vector3 scale = sprite.transform.parent.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir); // positivo ou negativo
            sprite.transform.parent.localScale = scale;
        }
    }

    // ---------- ATTACK ----------

    void Attack()
    {
        isAttacking = true;
        attackCooldown = 1.5f;

        if (anim != null)
        {
            anim.SetTrigger("Atacando");
            Debug.Log("Enemy atacou!");
        }
        Debug.Log("Enemy não atacou!");


    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        float distance = Mathf.Abs(player.position.x - rb.position.x);

        if (distance <= aggroRange)
            state = State.Aggro;
        else
            state = State.Walk;
    }


    // --- MORTE ---
    public void Die()
    {
        if (!vivo) return;

        vivo = false;
        isAttacking = false;

        if (audioS != null && Morte != null)
        {
            audioS.PlayOneShot(Morte, 1f);
        }

        // Toca animação de morte
        if (anim != null)
            anim.SetTrigger("Morto");

        // Para movimento
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;

        // Desliga colisão física
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Toca animação
        anim.SetTrigger("Morto");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        this.enabled = false;

        GameController.gc.esqueleto++;
        GameController.gc.RefreshScreen();
    }
}
