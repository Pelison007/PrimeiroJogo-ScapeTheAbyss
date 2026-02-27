using System.Collections;
using UnityEngine;

public class Porta : MonoBehaviour
{
    [Header("Cooldown da Porta")]
    public float cooldownEntrada = 2f;

    [Header("Spawn da Próxima Fase")]
    public bool entraPelaDireita;
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
        //MostrarTexto();

        StartCoroutine(DelayTP(collision));
    }

    IEnumerator DelayTP(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        Transform playerTransform = collision.transform;

        if (player != null)
            player.DesativaInput();

        if (FaseManager.fm != null)
        {
            string mensagem = "";

            switch (tipoPorta)
            {
                case TipoPorta.ProximaFase:
                    mensagem = "Indo para a próxima fase...";
                    break;

                case TipoPorta.VoltarFase:
                    mensagem = "Voltando...";
                    break;

                case TipoPorta.FaseEspecifica:
                    mensagem = "Entrando na fase " + faseDestino + "...";
                    break;

                case TipoPorta.UltimaFase:
                    mensagem = "Boss FINAL!";
                    break;
            }

            // 💬 MOSTRA IMEDIATAMENTE
            FaseManager.fm.MostrarMensagem(mensagem);

            // ⏳ AGORA espera antes de teleportar
            yield return new WaitForSeconds(delayTP);

            switch (tipoPorta)
            {
                case TipoPorta.ProximaFase:
                    FaseManager.fm.ProximaFase(playerTransform);
                    break;

                case TipoPorta.VoltarFase:
                    FaseManager.fm.FaseAnterior(playerTransform);
                    break;

                case TipoPorta.FaseEspecifica:
                    FaseManager.fm.IrParaFase(
                        faseDestino,
                        playerTransform,
                        entraPelaDireita
                    );
                    break;

                case TipoPorta.UltimaFase:
                    FaseManager.fm.ProximaFase(playerTransform);
                    break;
            }

            if (player != null && pontoRespawn != null)
                player.AtualizaRespawn(pontoRespawn.position);
        }

        if (player != null)
            player.AtivaInput();

        yield return new WaitForSeconds(cooldownEntrada);

        if (col != null)
            col.enabled = true;

        emTransicao = false;

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
