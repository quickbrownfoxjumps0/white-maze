using UnityEngine;
using System.Collections;

public class BlackOverlayFader : MonoBehaviour
{
    public Renderer overlayRenderer;  // Renderer of your black quad
    public float fadeDuration = 1f;   // Duration of fade in/out

    private Coroutine currentFade;

    void Awake()
    {
        SetAlpha(0); // start fully transparent (invisible)
        gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        gameObject.SetActive(true);
        currentFade = StartCoroutine(FadeTo(1));
    }

    public void FadeOut()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeTo(0));
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = overlayRenderer.material.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(targetAlpha);

        if (targetAlpha == 0)
            gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color c = overlayRenderer.material.color;
        c.a = alpha;
        overlayRenderer.material.color = c;
    }
}

