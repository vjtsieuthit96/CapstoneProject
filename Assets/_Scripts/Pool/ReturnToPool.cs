using UnityEngine;
using System.Collections;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField] private string poolKey = "IcePlane";
    private Material materialInstance;
    private Coroutine fadeSequenceCoroutine;

    public float TimeFadeOn = 3f;
    public float TimeFadeOff = 2f;

    private void OnEnable()
    {
        SetupMaterial();

        if (fadeSequenceCoroutine != null)
            StopCoroutine(fadeSequenceCoroutine);

        fadeSequenceCoroutine = StartCoroutine(FadeSequence());
    }

    private void OnDisable()
    {
        if (fadeSequenceCoroutine != null)
        {
            StopCoroutine(fadeSequenceCoroutine);
            fadeSequenceCoroutine = null;
        }
    }

    private void SetupMaterial()
    {
        if (transform.childCount > 0)
        {
            Renderer childRenderer = transform.GetChild(0).GetComponent<Renderer>();
            if (childRenderer != null)
            {
                materialInstance = childRenderer.material;
            }
        }
    }

    private IEnumerator FadeSequence()
    {
        if (materialInstance == null)
            yield break;

        yield return StartCoroutine(FadeAlpha(materialInstance, 0f, 200f / 255f, TimeFadeOn));
        yield return StartCoroutine(FadeAlpha(materialInstance, 200f / 255f, 0f, TimeFadeOff));
        ReturnToPoolNow();
    }

    private IEnumerator FadeAlpha(Material mat, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = mat.GetColor("_BaseColor");

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            color.a = alpha;
            mat.SetColor("_BaseColor", color);
            time += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        mat.SetColor("_BaseColor", color);
    }

    private void ReturnToPoolNow()
    {
        if (GameObjectPoolManager.Instance != null)
        {
            GameObjectPoolManager.Instance.ReturnObject(poolKey, gameObject);
        }
    }
}