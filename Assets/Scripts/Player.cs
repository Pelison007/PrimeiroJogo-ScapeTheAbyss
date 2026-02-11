using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player :MonoBehaviour
{
    
    private Vector2 direcao;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animacao;
    private GameController gcPlayer;
    private PlayerHealth playerLife;

    [Header("Item Guarda-Chuva")]
    public Collider2D guardaChuvaCollider; // arraste o ColliderGuadaChuva aqui no Inspector
    private bool hasUmbrella = false;   // TEM o item
    public bool GuardaChuvaAberto = false;
    private ItemPickup nearbyItem;
    private bool aguardandoConfirmacaoGuardaChuva = false;
    public GameObject mensagemGuardaChuva;



    // sttatus player 
    public float MoveSpeed = 5f;
    public float MoveCorrendo = 2f;
    public float forcaPulo = 5f;
    private float velocidadeAtual = 0f;
    // ------------
    // variaveis
    private bool Correndo = false;
    private bool podePular = true;
    private bool DoublePulo = true;
    public bool NoChao = true;
    private bool facingRight = true;
    private int contatosChao = 0;
    // animações
    private int CorrendoHash = Animator.StringToHash("Correndo");
    private int AndandoHash = Animator.StringToHash("Andando");
    private int NoChaoHash = Animator.StringToHash("NoChao");
    private int AndandoGuardaChuvaHash = Animator.StringToHash("AndandoGuardaChuva");
    private PlayerInput playerInput;
    //variavel para som
    public AudioSource audioS;
    public AudioClip[] Sounds;
    // respawn
    [Header("Respawn")]
    public Vector2 ultimoRespawn; // <--- aqui é declarado
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerLife = GetComponent<PlayerHealth>();
    }
    void Start()
    {
        ultimoRespawn = transform.position; // começa no ponto inicial da cena
        gcPlayer = GameController.gc;
        gcPlayer.Spirit = 0;
        gcPlayer.esqueleto = 0;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animacao = GetComponent<Animator>();

        GameController.gc.RefreshScreen();
    }
    public void DesativaInput()
    {
        if (playerInput != null) playerInput.enabled = false;
    }
    public void AtivaInput()
    {
        if (playerInput != null) playerInput.enabled = true;
    }
    // pegar o guarda-chuva
    public void GetUmbrella()
    {
        hasUmbrella = true;
        Debug.Log("Player agora TEM o guarda-chuva ☂️ | hasUmbrella = " + hasUmbrella);
    }


    // para usar som nos outras classes
    public void PlayerSound(int index)
    {
        if (index < 0 || index >= Sounds.Length) return;
        audioS.PlayOneShot(Sounds[index]);
    }
    // Update is called once per frame
    public void OnMove(InputAction.CallbackContext context)
    {
        direcao = context.ReadValue<Vector2>(); // recebe direção
        if (GuardaChuvaAberto)
        {
            animacao.SetBool(AndandoGuardaChuvaHash, direcao.x != 0); // animação de andar quando recebe uma direção com guarda chuva anerto
        }
        else
        {
            animacao.SetBool(AndandoHash, direcao.x != 0); // animação de andar quando recebe uma direção
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) Correndo = true;
        if (context.canceled) Correndo = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {

        if (!context.performed) return;
        if (!playerLife.vivo) return;
        if (!podePular && !DoublePulo) return;

       if (podePular)
        {
            audioS.PlayOneShot(Sounds[2]);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);

            if (GuardaChuvaAberto)
            {
                animacao.SetBool(NoChaoHash, false);
            }
            else
            {
                animacao.SetBool(NoChaoHash, false);
            }
            podePular = false;
            DoublePulo = true;
        }           
        else if (!podePular && DoublePulo)
        {
            audioS.PlayOneShot(Sounds[2]);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
            if (GuardaChuvaAberto)
            {
                animacao.SetBool(NoChaoHash, false);
            }
            else
            {
                animacao.SetBool(NoChaoHash, false);
            }
            podePular = false;
            DoublePulo = false;
        }
    }


    private void FixedUpdate()
    {

        // Queda com guarda chuva
            float maxFallSpeed = -3f;

            if (GuardaChuvaAberto && rb.linearVelocity.y < maxFallSpeed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
            }

        // ----------
        velocidadeAtual = Correndo ? MoveCorrendo : MoveSpeed;
        rb.linearVelocity = new Vector2 (direcao.x * velocidadeAtual, rb.linearVelocity.y);
        if (direcao.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direcao.x < 0 && facingRight)
        {
            Flip();
        }
        animacao.SetBool(CorrendoHash, Correndo && direcao.x != 0);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // ISSO AQUI "DESVIRA" A BARRA:
        // Se o pai virou para -1, a barra vira para -1 também. 
        // Multiplicando a escala da barra por -1 de novo, ela volta ao normal.
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D ponto in collision.contacts)
        {
            if (ponto.normal.y > 0.7f && rb.linearVelocity.y <= 0f)
            {
                // acabou de aterrissar
                if (!NoChao)
                {
                    DoublePulo = true;
                }

                NoChao = true;
                podePular = true;
                if (GuardaChuvaAberto)
                {
                    animacao.SetBool(NoChaoHash, true);
                }
                else
                {
                    animacao.SetBool(NoChaoHash, true);
                }

                return;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            animacao.SetBool(NoChaoHash, true);
        }

        if (collision.gameObject.CompareTag("Platform"))
        {
            gameObject.transform.parent = collision.transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        contatosChao = Mathf.Max(0, contatosChao - 1);

        if (contatosChao == 0)
        {
            animacao.SetBool(NoChaoHash, false);
            transform.SetParent(null);
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            gameObject.transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spirit"))
        {
            audioS.PlayOneShot(Sounds[0]);
            Destroy(collision.gameObject);
            gcPlayer.Spirit++;
            
        }

        if (collision.gameObject.CompareTag("Life"))
        {
            audioS.PlayOneShot(Sounds[1]);
            Destroy(collision.gameObject);
            gcPlayer.lifes++;
        }
    }

    public void AtualizaRespawn(Vector2 pos)
    {
        ultimoRespawn = pos;
    }




    // USAR GUARDA CHUVA

    public void OnUseUmbrella(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (!hasUmbrella) return; // garante que só continue se o jogador tiver guarda-chuva

        if (!GuardaChuvaAberto)
        {
            // Abrir
            GuardaChuvaAberto = true;
            animacao.SetBool("GuardaChuvaAberto", true);
            animacao.SetTrigger("AbrirGuardaChuva");
        }
        else
        {
            // Fechar
            GuardaChuvaAberto = false;
            animacao.SetBool("GuardaChuvaAberto", false);
            animacao.SetTrigger("FechandoGuardaChuva");
        }
    }


    public void OnUmbrellaAnimationComplete()
    {
        GuardaChuvaAberto = !GuardaChuvaAberto;
        animacao.SetBool("GuardaChuvaAberto", GuardaChuvaAberto);
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (nearbyItem != null)
        {
            nearbyItem.PickUp(this);
        }
        if (CompareTag("GuardaChuva"))
        {
            DesativaInput();
        }
    }
    public void OnConfirmar(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!aguardandoConfirmacaoGuardaChuva) return;

        // despausa
        Time.timeScale = 1f;

        aguardandoConfirmacaoGuardaChuva = false;

        if (mensagemGuardaChuva != null)
        {
            mensagemGuardaChuva.SetActive(false);
            AtivaInput();
        }
            
    }

    public void AguardarConfirmacaoGuardaChuva()
    {
        aguardandoConfirmacaoGuardaChuva = true;
    }

    // pegar items

    public void SetNearbyItem(ItemPickup item)
    {
        nearbyItem = item;
    }

    public void ClearNearbyItem(ItemPickup item)
    {
        if (nearbyItem == item)
            nearbyItem = null;
    }

}

