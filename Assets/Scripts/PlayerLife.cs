using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // Variaveis

    float maxLife = 100f;
    float currentLife;

    // Publicos

    public Slider slider;
    public TextMeshProUGUI lifeText;

    // Privados

    [SerializeField] private float barSpeed = 5f;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;
    private float targetFill;
    private Coroutine flashRoutine;
    private Color originalColor;

    private void Start()
    {
        currentLife = maxLife;
        originalColor = lifeText.color;
        AtualizarUI();
    }

    private void Update()
    {
        if (slider.value != targetFill)
            slider.value = Mathf.Lerp(slider.value, targetFill, Time.deltaTime * barSpeed);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        float oldLife = currentLife;

        currentLife = Mathf.Clamp(currentLife - damage, 0, maxLife);

        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashText());

        if (oldLife != currentLife)
            AtualizarUI();
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        float oldLife = currentLife;

        currentLife = Mathf.Clamp(currentLife + amount, 0, maxLife);

        if (oldLife != currentLife)
            AtualizarUI();
    }

    void AtualizarUI()
    {
        targetFill = currentLife / maxLife;
        lifeText.text = Mathf.RoundToInt(targetFill * 100) + "%";
    }

    IEnumerator FlashText()
    {
        lifeText.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        lifeText.color = originalColor;
    }
}
