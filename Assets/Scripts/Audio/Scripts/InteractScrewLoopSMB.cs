using UnityEngine;

public class InteractScrewLoopSMB : StateMachineBehaviour
{
    [Header("Audio")]
    public SoundType loopSound = SoundType.Mechanical;
    [Range(0f, 1f)] public float volume = 1f;
    [Tooltip("Fade out al salir del estado (0 = corte instantáneo)")]
    [Range(0f, 0.3f)] public float fadeOut = 0.05f;

    [Header("Gate")]
    public bool requireIsHolding = true;
    public string holdingBoolName = "IsHolding";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (requireIsHolding && !string.IsNullOrEmpty(holdingBoolName))
        {
            if (!animator.GetBool(holdingBoolName)) return;
        }

        AudioManager.PlayLoopSfx(loopSound, volume);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.StopLoopSfx(fadeOut);
    }
}

