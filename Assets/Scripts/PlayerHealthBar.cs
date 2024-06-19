using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject healthBar; // Reference to the health bar GameObject

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetActive(false); // Initially hide the health bar
            StartCoroutine(ShowHealthBarAfterDelay(6.0f)); // Start the coroutine to show it after 3 seconds
        }
        else
        {
            Debug.LogWarning("Health bar GameObject reference is not set.");
        }
    }

    private IEnumerator ShowHealthBarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        healthBar.SetActive(true); // Show the health bar
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}
