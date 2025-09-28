using UnityEngine;
using System.Collections;

public class SequentialScaler : MonoBehaviour
{
    [Header("Objects")]
    public Transform objectA;
    public Transform objectB;

    [Header("Scale Settings")]
    public float objectAStartScale = 3.75f;
    public float objectAEndScale = 0f;
    public float objectBStartScale = 5f;
    public float objectBEndScale = 23f;

    [Header("Duration Settings")]
    public float objectBDuration = 2f;
    public float objectADuration = 2f;

    [Header("Gate")]
    public GameObject TransportGate;

    private bool isRunning = false;

    private void Awake()
    {
        TransportGate.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isRunning)
        {
            TheEndOfTree();
        }
    }

    public void TheEndOfTree()
    {
        StartCoroutine(ScaleSequence());
    }

    private IEnumerator ScaleSequence()
    {
        isRunning = true;

        if (objectA != null) objectA.localScale = Vector3.one * objectAStartScale;
        if (objectB != null) objectB.localScale = Vector3.one * objectBStartScale;

        if (objectB != null)
            yield return StartCoroutine(ScaleOverTime(objectB, objectBStartScale, objectBEndScale, objectBDuration));

        if (objectA != null)
            yield return StartCoroutine(ScaleOverTime(objectA, objectAStartScale, objectAEndScale, objectADuration));

        if (TransportGate != null)
        {
            TransportGate.SetActive(true);
            objectA.gameObject.SetActive(false );
        }    
        isRunning = false;
    }

    private IEnumerator ScaleOverTime(Transform target, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float current = Mathf.Lerp(start, end, t);
            target.localScale = Vector3.one * current;
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = Vector3.one * end;
    }
}
