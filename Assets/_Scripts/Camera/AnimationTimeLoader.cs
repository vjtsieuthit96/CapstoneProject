using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationTimeLoader : MonoBehaviour
{
    [Header("Cấu hình Animation")]
    public string stateName;         // Tên state trong Animator (vd: "Run")
    public AnimationClip animationClip; // Clip gắn với state đó
    public int targetFrame = 2700;   // Frame muốn nhảy tới (nhập trong Inspector)
    public float frameRate = 60f;    // FPS của clip (thường 30 hoặc 60)

    public Animator animator;

    [ContextMenu("Load Animation To Frame")]
    public void LoadToFrame()
    {
        if (animationClip == null)
        {
            Debug.LogWarning("Chưa gắn AnimationClip!");
            return;
        }

        // Tính thời gian (giây) dựa trên frame
        float targetTime = targetFrame / frameRate;

        // Convert sang normalized time
        float normalizedTime = Mathf.Clamp01(targetTime / animationClip.length);

        // Nhảy đến đúng thời điểm
        animator.Play(stateName, 0, normalizedTime);
        animator.Update(0f); // Cập nhật ngay
    }
}
