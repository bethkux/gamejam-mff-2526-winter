using UnityEngine;

public class ScalePulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    public float pulseScale = 1.2f;   // how big it gets
    public float duration = 0.25f;    // time up, time down

    private Vector3 originalScale;
    private bool pulsed = false;

    void OnEnable()
    {
        if (!pulsed)
        {
            originalScale = transform.localScale;
            StartCoroutine(Pulse());
            pulsed = true;
        }
    }

    private System.Collections.IEnumerator Pulse()
    {
        Vector3 targetScale = originalScale * pulseScale;
        float t = 0f;

        // scale up
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        t = 0f;

        // scale down
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
