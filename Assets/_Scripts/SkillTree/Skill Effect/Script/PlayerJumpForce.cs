using UnityEngine;
[CreateAssetMenu(fileName = "PlayerJumpForce", menuName = "Scriptable Objects/Strength/PlayerJumpForce")]
public class PlayerJumpForce : SkillEffect
{
    [Header("Percent")]
    [Range(0f, 1f)]
    public float jumpForce = 0.1f;
    public override void ApplyEffect(CharacterConfigurator configurator)
    {
        configurator.jumpHeight += configurator.jumpHeight * jumpForce;
    }
}
