using UnityEngine;

public class AnimVoiceBinder : MonoBehaviour
{
    public Animator animator;
    public CharacterVoiceManager voiceManager;

    [System.Serializable]
    public struct AnimVoice
    {
        public string animStateName;
        public AudioClip voiceClip;
        [Range(0f, 1f)] public float volume;
    }

    public AnimVoice[] voices;

    private string lastState;

    void Update()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        foreach (var v in voices)
        {
            if (stateInfo.IsName(v.animStateName) && lastState != v.animStateName)
            {
                lastState = v.animStateName;
                voiceManager.PlayVoice(v.voiceClip, v.volume);
            }
        }
    }
}
