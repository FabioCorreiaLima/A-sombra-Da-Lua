using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [Header("Referências")]
    public Transform greenBar;
    public Transform redBar;
    public bool decreaseFromRight = true;

    private Vector3 originalScale;
    private float originalXPosition;
    private float maxHealth;

    public void Initialize(float maxHealth)
    {
        this.maxHealth = maxHealth;
        
        if (greenBar != null)
        {
            originalScale = greenBar.localScale;
            originalXPosition = greenBar.localPosition.x;
        }
        
        if (redBar != null)
        {
            redBar.SetAsFirstSibling();
        }
    }

    public void UpdateHealth(float currentHealth)
    {
        if (greenBar == null || redBar == null) return;

        float healthPercentage = currentHealth / maxHealth;
        UpdateGreenBar(healthPercentage);
    }

    private void UpdateGreenBar(float healthPercentage)
    {
        Vector3 newScale = originalScale;
        newScale.x = originalScale.x * healthPercentage;
        greenBar.localScale = newScale;

        if (decreaseFromRight)
        {
            UpdateBarPosition(newScale);
        }
    }

    private void UpdateBarPosition(Vector3 newScale)
    {
        Vector3 newPosition = greenBar.localPosition;
        newPosition.x = originalXPosition - (originalScale.x - newScale.x) / 2;
        greenBar.localPosition = newPosition;
    }
}