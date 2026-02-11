using UnityEngine;
using System.Collections;
public class GotaSpawn : MonoBehaviour
{
    public GameObject gotaPrefab;
    public Transform player;

    public float width = 0;
    public float height = 12f; // altura acima do jogador onde as gotas aparecem
    public float spawnIntervalo = 0;
    public int gotasPorSpawn = 0;
    Coroutine rotinaChuva;
    private bool pausada;
    private bool audioAtivo;

    [Header("Audio")]
    public AudioSource audioChuva;
    private float volumeOriginal;

    private bool chovendo;
    public enum IntensidadeChuva
    {
        Fraca,
        Media,
        Forte
    }

    void Awake()
    {
        volumeOriginal = audioChuva.volume;
    }

    void AplicarIntensidade(IntensidadeChuva intensidade)
    {
        switch (intensidade)
        {
            case IntensidadeChuva.Fraca:
                spawnIntervalo = 0.4f;
                gotasPorSpawn = 3;
                width = 25f;
                break;

            case IntensidadeChuva.Media:
                spawnIntervalo = 0.25f;
                gotasPorSpawn = 4;
                width = 25f;
                break;

            case IntensidadeChuva.Forte:
                spawnIntervalo = 0.12f;
                gotasPorSpawn = 5;
                width = 25f;
                break;
        }
    }

    Coroutine rotinaAudio;

    public void InicioChuva(IntensidadeChuva intensidade)
    {
        AplicarIntensidade(intensidade);

        if (!chovendo)
        {
            chovendo = true;
            rotinaChuva = StartCoroutine(RotinaChuva());
        }

        if (!audioAtivo)
        {
            audioAtivo = true;
            if (!audioChuva.isPlaying)
                audioChuva.Play();
        }
    }

    public void PararChuva()
    {
        if (chovendo)
        {
            chovendo = false;
            if (rotinaChuva != null)
            {
                StopCoroutine(rotinaChuva);
                rotinaChuva = null;
            }
        }

        if (audioAtivo)
        {
            audioAtivo = false;
            audioChuva.Stop();
        }
    }



    void SpawnUmaGota()
    {
        float x = Random.Range(-width / 2f, width / 2f);
        float yOffset = Random.Range(-0.6f, 0.6f);

        Vector2 pos = new Vector2(
            player.position.x + x,
            player.position.y + height + yOffset
        );

       Instantiate(gotaPrefab, pos, Quaternion.identity);
    }

    IEnumerator RotinaChuva()
    {
        while (chovendo)
        {
            if (pausada)
            {
                yield return null;
                continue;
            }

            for (int i = 0; i < gotasPorSpawn; i++)
            {
                SpawnUmaGota();
                yield return new WaitForSeconds(Random.Range(0.02f, 0.06f));
            }

            yield return new WaitForSeconds(spawnIntervalo);
        }
    }

    public void PausarChuva()
    {
        pausada = true;
    }

    public void RetomarChuva()
    {
        pausada = false;
    }


    IEnumerator FadeOutChuva(float duracao)
    {
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            audioChuva.volume = Mathf.Lerp(volumeOriginal, 0f, tempo / duracao);
            yield return null;
        }

        audioChuva.Stop();
        audioChuva.volume = volumeOriginal;
    }

    IEnumerator FadeInChuva(float duracao)
    {
        audioChuva.volume = 0f;

        if (!audioChuva.isPlaying)
            audioChuva.Play();

        float tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            audioChuva.volume = Mathf.Lerp(0f, volumeOriginal, tempo / duracao);
            yield return null;
        }

        audioChuva.volume = volumeOriginal;
    }

    IEnumerator SpawnSuave()
    {
        for (int i = 0; i < gotasPorSpawn; i++)
        {
            float x = Random.Range(-width / 2f, width / 2f);
            float yOffset = Random.Range(-0.6f, 0.6f);

            Vector2 pos = new Vector2(
                player.position.x + x,
                player.position.y + height + yOffset
            );

            GameObject gota = Instantiate(gotaPrefab, pos, Quaternion.identity);

            Rigidbody2D rb = gota.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.down * Random.Range(2f, 4f);
                rb.gravityScale = 0.4f;
                rb.linearDamping = 0.2f;
            }
            
            Collider2D col = gota.GetComponent<Collider2D>();
            if (col != null)
            {
                StartCoroutine(PararNoChao(gota, rb, col));
            }
            yield return new WaitForSeconds(Random.Range(0.02f, 0.06f));
        }
    }

    IEnumerator PararNoChao(GameObject gota, Rigidbody2D rb, Collider2D col)
    {
        while (gota != null)
        {
            // Raycast curto pra baixo pra detectar chão
            RaycastHit2D hit = Physics2D.Raycast(gota.transform.position, Vector2.down, 0.1f, LayerMask.GetMask("chao"));
            if (hit.collider != null)
            {
                // Achou chão → para a gota
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
                yield return new WaitForSeconds(0.6f); // delay antes de destruir
                Destroy(gota);
                yield break;
            }
            yield return null;
        }
    }

}
