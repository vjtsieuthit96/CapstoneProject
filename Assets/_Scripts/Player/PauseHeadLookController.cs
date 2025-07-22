using UnityEngine;
using Invector.vCharacterController;
using Invector;

public class PauseHeadLookController : MonoBehaviour
{
    public vHeadTrack headTrack;
    public Transform lookTarget;
    public vLookTarget Target;
    private bool wasPaused = false;
    public vThirdPersonInput input;

    private void Update()
    {
        if(GameManager.Instance.isPause)
        {
            input.updateIK = true;
        }    

        if (GameManager.Instance.isPause && !wasPaused)
        {
            Debug.Log("Pause bắt đầu – đặt điểm nhìn");
            if (headTrack != null && lookTarget != null)
            {
                input.PlayerLateUpdate();
                headTrack.SetLookTarget(lookTarget);
                lookTarget.gameObject.SetActive(true);
            }
        }
        else if (!GameManager.Instance.isPause && wasPaused)
        {
            Debug.Log("Thoát pause – khôi phục trạng thái nhìn");
            if (headTrack != null)
            {
                headTrack.RemoveLookTarget(lookTarget);
                headTrack.freezeLookPoint = false;
                lookTarget.gameObject.SetActive(false);
            }
        }

        wasPaused = GameManager.Instance.isPause;
    }
}
