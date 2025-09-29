using UnityEngine;
using Invector.vCamera;
using System.Collections;
using Unity.Properties;

public class CutsceneFunc : MonoBehaviour
{
    public static CutsceneFunc Instance { get; private set; }

    [Header("Custom Camera")]
    [SerializeField] private Camera customCamera;

    public Camera thirdPersonCamera;
    public bool isPlayer = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (customCamera != null)
            customCamera.gameObject.SetActive(false);

        StartCoroutine(FindThirdPersonCamera());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            isPlayer = !isPlayer;
            OnPlayerSpawned();
        }
    }

    private IEnumerator FindThirdPersonCamera()
    {
        while (thirdPersonCamera == null)
        {
            var cam = Camera.main;
            if (cam != null && cam.gameObject.layer == LayerMask.NameToLayer("Camera"))
            {
                thirdPersonCamera = cam;
                //OnPlayerSpawned();
                yield break;
            }
            yield return null;
        }
    }

    public void OnPlayerSpawned()
    {
        if (thirdPersonCamera != null)
            thirdPersonCamera.gameObject.SetActive(isPlayer);

        if (customCamera != null)
            customCamera.gameObject.SetActive(!isPlayer);
    }

    public void SwitchBackToThirdPerson()
    {
        if (thirdPersonCamera != null)
            thirdPersonCamera.enabled = true;

        if (customCamera != null)
            customCamera.enabled = false;
    }
}
