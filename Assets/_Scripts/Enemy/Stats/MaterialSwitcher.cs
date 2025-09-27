using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [Header("Object Materials")]
    public Material normalObjectMaterial;
    public Material specialObjectMaterial;

    [Header("Particle Materials")]
    public Material normalParticleMaterial;
    public Material specialParticleMaterial;

    [Header("References")]
    public Renderer objectRenderer;
    public ParticleSystem particleSystem;

    private Renderer particleRenderer;

    private Coroutine flashRoutine;

    private bool isFlashing = false;
    private bool isDying = false;

    [SerializeField] private MonsterStats TreeStats;

    void Start()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();
        if (particleSystem != null) particleRenderer = particleSystem.GetComponent<Renderer>();

        ApplyNormal();
    }

    void Update()
    {
        CheckTree();
    }

    public void CheckTree()
    {
        float healthPercent = TreeStats.GetCurrentHealth() / TreeStats.GetMaxHealth();

        if (healthPercent <= 0.2f && !isDying)
        {
            TreeDying();
        }
        else if (healthPercent > 0.2f && isDying)
        {
            isDying = false;
            ApplyNormal();
        }
    }

    public void HurtofTree()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashSpecial(0.5f));
    }

    public void TreeDying()
    {
        isDying = true;
        ApplySpecial();
    }

    private System.Collections.IEnumerator FlashSpecial(float duration)
    {
        isFlashing = true;
        ApplySpecial();
        yield return new WaitForSeconds(duration);
        isFlashing = false;

        if (!isDying)
            ApplyNormal();

        flashRoutine = null;
    }

    private void ApplyNormal()
    {
        if (objectRenderer != null)
            objectRenderer.material = normalObjectMaterial;

        if (particleRenderer != null)
            particleRenderer.material = normalParticleMaterial;
    }

    private void ApplySpecial()
    {
        if (objectRenderer != null)
            objectRenderer.material = specialObjectMaterial;

        if (particleRenderer != null)
            particleRenderer.material = specialParticleMaterial;
    }
}
