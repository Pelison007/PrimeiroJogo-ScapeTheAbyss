using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownUI : MonoBehaviour
{
    public TrovaoSkill skill;
    public Player player;

    public Image icon;
    public Image cooldownOverlay;
    public TextMeshProUGUI cooldownText;

    void Start()
    {
        cooldownOverlay.fillAmount = 0;
    }

    void Update()
    {
        // 🔒 SE NÃO DESBLOQUEOU A SKILL
        if (!player.TemTrovao())
        {
            icon.color = new Color(0f, 0f, 0f, 0.6f); // preto semi-transparente
            cooldownOverlay.fillAmount = 0;
            cooldownText.enabled = false;
            return;
        }

        if (!skill.CanUse())
        {
            cooldownOverlay.fillAmount = skill.CooldownPercent;
            icon.color = new Color(0.6f, 0.6f, 0.6f, 1f);

            cooldownText.text = skill.CooldownRemaining.ToString("F1");
            cooldownText.enabled = true;
        }
        else
        {
            cooldownOverlay.fillAmount = 0;
            icon.color = Color.white;

            cooldownText.enabled = false;

        }
    }

}
