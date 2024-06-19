using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlayController : MonoBehaviour
{
    public Image overlayImage;
    public Color damageColor = new Color(3f, 0f, 0f, 0.5f); // Light red with some transparency
    public Color healColor = new Color(0f, 1f, 0f, 0.5f); // Light green with some transparency
    public float fadeDuration = 1f;

    private void Start()
    {
        if (overlayImage == null)
        {
            Debug.LogError("Damage overlay is not assigned.");
        }
        else
        {
            // Ensure the overlay starts fully transparent
            overlayImage.color = new Color(damageColor.r, damageColor.g, damageColor.b, 0f);
        }
    }

    public void ShowDamageEffect()
    {
        StartCoroutine(FadeOverlay(damageColor));
    }

    public void ShowHealEffect()
    {
        StartCoroutine(FadeOverlay(healColor));
    }

    private IEnumerator FadeOverlay(Color effectColor)
    {
        float elapsed = 0f;
        overlayImage.color = effectColor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(effectColor.a, -1f, elapsed / fadeDuration);
            overlayImage.color = new Color(effectColor.r, effectColor.g, effectColor.b, alpha);
            yield return null;
        }

        // Ensure the overlay is fully transparent at the end
        overlayImage.color = new Color(effectColor.r, effectColor.g, effectColor.b, 0f);
    }
}
