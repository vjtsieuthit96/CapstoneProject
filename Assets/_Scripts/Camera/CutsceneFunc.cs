using UnityEngine;
using Invector.vCamera;
using System.Collections;
using Unity.Properties;

public class CutsceneFunc : MonoBehaviour
{
    public static CutsceneFunc Instance { get; private set; }
    public GameObject Boss;
    [Header("Custom Camera")]
    [SerializeField] private Camera customCamera;

    public Camera thirdPersonCamera;
    public bool isPlayer = false;

    private void Awake()
    {
        isPlayer = true;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //if (customCamera != null)
        //    customCamera.gameObject.SetActive(false);

        StartCoroutine(FindThirdPersonCamera());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            setaShortCut();
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
                yield break;
            }
            yield return null;
        }
    }

    public void setCurrentCamera(Camera NewCamera)
    {
        customCamera = NewCamera;
    }    

    public void OnCutScene()
    {
        isPlayer = false;
        if (thirdPersonCamera != null)
            thirdPersonCamera.gameObject.SetActive(isPlayer);

        if (customCamera != null)
            customCamera.gameObject.SetActive(!isPlayer);
    }

    public void OffCutScene()
    {
        isPlayer = true;
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

    public void setaShortCut()
    {
        Boss.SetActive(true);
        PathDrawer.Instance.SetTarget(Boss.transform);
    }

}
