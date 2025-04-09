using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // Váriaveis

    float maxLife = 100f;
    float currentLife;
    public bool isInvincible = false;

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
        UpdateUI();
    }

    private void Update()
    {
        if (slider.value != targetFill)
            slider.value = Mathf.Lerp(slider.value, targetFill, Time.deltaTime * barSpeed);
        
        if (currentLife <= 0)
        {
            SceneManager.LoadScene("DeathScene");
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        float oldLife = currentLife;

        currentLife = Mathf.Clamp(currentLife - damage, 0, maxLife);

        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashText());

        if (oldLife != currentLife)
            UpdateUI();
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        float oldLife = currentLife;

        currentLife = Mathf.Clamp(currentLife + amount, 0, maxLife);

        if (oldLife != currentLife)
            UpdateUI();
    }

    public void ActivateInvincibility(float durantion)
    {
        if (!isInvincible)
            StartCoroutine(InvincibilityRoutine(durantion));
    }

    public void HealFull()
    {
        currentLife = maxLife;
        UpdateUI();
    }

    IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        lifeText.color = Color.yellow;

        yield return new WaitForSeconds(duration);

        isInvincible = false;
        lifeText.color = originalColor;
    }

    void UpdateUI()
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
