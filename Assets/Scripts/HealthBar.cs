using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar HealthBarr;

    public Slider slider;
    public Image fillImage;

    // --- a barra vai seguir este alvo ---
    public Transform target;      // quem a barra segue
    public Vector3 offset;        // altura

    public Color fullHealthColor = Color.green;
    public Color midHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;

    void AtualizarBarra(float novaVida)
    {
        slider.value = novaVida;
    }
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
    public void SetMaxHealth(float health)
    {
        if (slider == null) Debug.Log("Slider é null!");
        slider.maxValue = health;
        slider.value = health;
        UpdateColor(); // Garante que comece verde
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
        UpdateColor(); // Garante que mude para amarelo/vermelho ao tomar dano
    }
    private void UpdateColor()
    {
        if (fillImage == null) return;

        // Calcula a porcentagem (0 a 1)
        float normalized = slider.value / slider.maxValue;

        if (normalized > 0.5f)
            fillImage.color = fullHealthColor;
        else if (normalized > 0.2f)
            fillImage.color = midHealthColor;
        else
            fillImage.color = lowHealthColor;
    }
}
