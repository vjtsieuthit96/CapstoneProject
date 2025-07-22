using UnityEngine;

public class HeadTrackTargetWhenPause : MonoBehaviour
{
    public Transform moveTarget;
    public float moveSpeed = 1f;
    public Vector3 moveDirection = Vector3.right;

    private void OnEnable()
    {
        if (!GameManager.Instance.isPause)
            gameObject.SetActive(false);
    }
    private void Update()
    {
        if (!GameManager.Instance.isPause)
        {
            gameObject.SetActive(false);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.unscaledDeltaTime * moveSpeed);
        }
    }
}
