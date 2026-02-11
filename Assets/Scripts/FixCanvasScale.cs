using UnityEngine;

public class FixCanvasScale : MonoBehaviour
{
    private Vector3 initialScale;

    void Awake()
    {
        // Salva a escala que você definiu (ex: 0.001)
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        if (transform.parent != null)
        {
            // Pega o sinal do X do pai (será 1 ou -1)
            float parentXScale = Mathf.Sign(transform.parent.localScale.x);

            // Aplica a escala inicial, mas compensa o sinal do pai
            // Se o pai for -1, o Canvas multiplica por -1 para voltar ao normal
            transform.localScale = new Vector3(
                initialScale.x * parentXScale,
                initialScale.y,
                initialScale.z
            );
        }
    }
}