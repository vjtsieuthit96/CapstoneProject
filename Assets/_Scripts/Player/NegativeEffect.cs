using System.Collections;
using UnityEngine;

public class NegativeEffect : MonoBehaviour
{
    [SerializeField] private CharacterConfigurator character;
    [SerializeField] private GameObject burnEffect;

    private Coroutine burnCoroutine;
    private bool isBurning;

    private void Awake()
    {
        character = GetComponent<CharacterConfigurator>();
    }
    private void Update()
    {
        Debug.Log(isBurning);
    }
    public void ApplyBurn(float damagePerSecond, float duration)
    {
        if (isBurning) return;
        burnCoroutine = StartCoroutine(BurnDamageOverTime(damagePerSecond, duration));
    }

    private IEnumerator BurnDamageOverTime(float damagePerSecond, float duration)
    {
        isBurning = true;
        float elapsed = 0f;
        if (!burnEffect.activeSelf)        
            burnEffect.SetActive(true);        
        while (elapsed < duration)
        {            
            character.TakeDamage(damagePerSecond);            
            yield return new WaitForSecondsRealtime(1f);
            elapsed += 1f;         
        }
        // End of burn effect
        burnEffect.SetActive(false);        
        burnCoroutine = null;
        isBurning = false;
    }
}
