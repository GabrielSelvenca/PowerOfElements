using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    float maxLife = 100f;
    float currentLife;
    public Slider slider;
    public TextMeshProUGUI lifeText;

    private void Start()
    {
        currentLife = maxLife;
        AtualizarUI();
    }

    void TakeDamage(float damage)
    {
        currentLife -= damage;
        currentLife = Mathf.Clamp(currentLife, 0, maxLife);
        AtualizarUI();
    }

    void AtualizarUI()
    {
        float percent = currentLife / maxLife;
        slider.value = percent;
        lifeText.text = Mathf.RoundToInt(percent * 100) + "%";
    }
}
