using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth: Health
{

    // variaveis para pegar os compnonentes 
    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D col;
    public Player player;
    private Health health;
    // ------booleano de vivo e morto ----
    public bool vivo = true;
    // crua
    public float cura= 26;
    public float duracaoCura = 5f;

    public float intervaloCura = 3f; // tempo entre curas automáticas
    private float proximaCura = 0f;  // momento da próxima cura

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(vidaMaxima);
            Debug.Log("Barra configurada com: " + vidaMaxima);
        }
    }
    void Start()
    {
        GameController.gc.RefreshScreen();
    }


    // Update is called once per frame
    void Update()
    {
        GameController.gc.RefreshScreen();

        // Checa se já passou o intervalo de cura
        if (Time.time >= proximaCura)
        {
            HealOverTime(cura, duracaoCura); // chama a cura gradual
            proximaCura = Time.time + intervaloCura; // agenda próxima cura
        }
    }

    public override void TakeDamage(int dano)
    {
        base.TakeDamage(dano);
        if (healthBar != null)
        {
            healthBar.SetHealth(vidaAtual);
        }
    }

    protected override void morto()
    {
        if (vivo)
        {
            LoseLife(); // chama função de perder vida
        }
    }

    public override void Heal(float amount)
    {
        vidaAtual += amount;

        if (vidaAtual > vidaMaxima)
            vidaAtual = vidaMaxima;

        if (healthBar != null)
            healthBar.SetHealth(vidaAtual);
    }


    public void LoseLife()
    {
        if (vivo)
        {
            // DESATIVA todos os comandos (Move, Jump, etc)
            if (player != null)
                player.DesativaInput();
            vivo = false;
            player.PlayerSound(4);
            rb.linearVelocity = Vector2.zero; // velociddade vai pra 0
            rb.bodyType = RigidbodyType2D.Kinematic; // para nao ser afetado GRAVIDADE e outras forças
            col.enabled = false; // desativa colisor
            anim.SetTrigger("Morto"); // animação de morto
            player.enabled = false; // desativa player
            GameController.gc.SetLives(-1);


            if (GameController.gc.lifes >= 0)
            {
                Invoke("Respawn", 1.5f);
            }
            else
            {
                Invoke("LoadGameOver", 1f);
                GameController.gc.RetirarScreen();
            }
        }
    }

    void Respawn()
    {
        vivo = true;
        // Volta a vida ao máximo e atualiza a barra
        health.vidaAtual = health.vidaMaxima;
        if (healthBar != null)
            healthBar.SetHealth(health.vidaAtual);

        //respawn no ponto definido
        transform.position = player.ultimoRespawn;

        rb.bodyType = RigidbodyType2D.Dynamic; // reativa gravidade do player
        rb.linearVelocity = Vector2.zero; // velocidade vai pra zero
        col.enabled = true; // reativa colisor do player
        player.enabled = true; // reativa o player
        anim.ResetTrigger("Morto"); // para animação de morte
        anim.Play("AnimParado"); // play animação parado

        // REATIVA os comandos do Player
        if (player != null)
            player.AtivaInput();
        player.PlayerSound(5); // som de respawn
    }

    public void LoadGameOver()
    {
        GameController.gc.lifes = 3;
        healthBar.gameObject.SetActive(false);

        SceneManager.LoadScene("GameOver");
    }
    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
