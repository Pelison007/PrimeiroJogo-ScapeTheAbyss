using System.Collections;
using UnityEngine;

public class Porta : MonoBehaviour
{
    [Header("Cooldown da Porta")]
    public float cooldownEntrada = 2f;

    public enum TipoPorta
    {
        FaseEspecifica,
        ProximaFase,
        VoltarFase,
        UltimaFase
    }

    [Header("Checkpoint")]
    public Transform pontoRespawn;

    [Header("Configuração da Porta")]
    public TipoPorta tipoPorta;
    public int faseDestino;
    public float delayTP = 1.5f;

    [Header("Áudio")]
    public AudioClip finish;

    private bool emTransicao = false;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void SondNextLevel()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && finish != null)
        {
            audio.clip = finish;
            audio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (emTransicao) return;
        if (!collision.CompareTag("Player")) return;

        emTransicao = true;

        // 🔒 trava física da porta
        if (col != null)
            col.enabled = false;

        SondNextLevel();
        MostrarTexto();

        StartCoroutine(DelayTP(collision));
    }

    void MostrarTexto()
    {
        if (GameController.gc == null) return;

        switch (tipoPorta)
        {
            case TipoPorta.UltimaFase:
                StartCoroutine(MostrarTextoTemporario(GameController.gc.UltimaFaseText?.gameObject));
                break;

            case TipoPorta.ProximaFase:
                StartCoroutine(MostrarTextoTemporario(GameController.gc.NextLevelText?.gameObject));
                break;

            case TipoPorta.VoltarFase:
                StartCoroutine(MostrarTextoTemporario(GameController.gc.VoltarFaseText?.gameObject));
                break;
        }
    }

    IEnumerator MostrarTextoTemporario(GameObject texto, float tempo = 2.5f)
    {
        if (texto == null) yield break;

        texto.SetActive(true);
        yield return new WaitForSeconds(tempo);
        texto.SetActive(false);
    }

    IEnumerator DelayTP(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        Transform playerTransform = collision.transform;

        if (player != null)
            player.DesativaInput();

        yield return new WaitForSeconds(delayTP);

        if (FaseManager.fm != null)
        {
            Debug.Log(FaseManager.fm);
            switch (tipoPorta)
            {
                case TipoPorta.ProximaFase:
                    FaseManager.fm.ProximaFase(playerTransform);
                    break;

                case TipoPorta.VoltarFase:
                    FaseManager.fm.FaseAnterior(playerTransform);
                    break;

                case TipoPorta.FaseEspecifica:
                    FaseManager.fm.IrParaFase(faseDestino, playerTransform);
                    break;

                case TipoPorta.UltimaFase:
                    Debug.Log("Última fase!");
                    FaseManager.fm.ProximaFase(playerTransform);
                    break;
            }

            if (player != null && pontoRespawn != null)
                player.AtualizaRespawn(pontoRespawn.position);
                
        }

        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.Player = playerTransform;
            camFollow.TeleportTo(playerTransform.position);
        }

        if (player != null)
            player.AtivaInput();

        // ⏱ cooldown real
        yield return new WaitForSeconds(cooldownEntrada);

        if (col != null)
            col.enabled = true;

        emTransicao = false;
    }
}
