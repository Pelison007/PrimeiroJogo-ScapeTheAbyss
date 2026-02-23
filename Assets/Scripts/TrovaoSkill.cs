using Unity.VisualScripting;
using UnityEngine;

public class TrovaoSkill : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool bateu = false;

    [Header("Configurações da Habilidade")]
    [SerializeField] public float cooldownSkill = 5f;
    [SerializeField] public float speed = 20f;
    [SerializeField] public int damage = 150;
    [SerializeField] public float skillRange = 10f; // alcance máximo da skill

    private float ultimoUso = -Mathf.Infinity;

    public bool CanUse()
    {
        if (Time.time < ultimoUso + cooldownSkill)
            return false;

        return Time.time >= ultimoUso + cooldownSkill;
    }

    public void TriggerCooldown()
    {
        ultimoUso = Time.time;
    }

}
