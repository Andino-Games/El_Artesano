using UnityEngine;

public class WalkFootstepsSMB : StateMachineBehaviour
{
    [Header("Timing (normalized 0..1)")]
    [Range(0f, 1f)] public float step1 = 0.18f;
    [Range(0f, 1f)] public float step2 = 0.68f;
    [Tooltip("Umbral para considerar que el ciclo reinició (normalizedTime%1)")]
    [Range(0.01f, 0.2f)] public float resetWindow = 0.06f;

    [Header("Audio")]
    [Range(0f, 1f)] public float volume = 1f;

    [Tooltip("Si está activo, alterna A/B. Si no, usa PlayerSteps.")]
    public bool useStepAB = true;

    public SoundType stepA = SoundType.StepA;
    public SoundType stepB = SoundType.StepB;
    public SoundType singleSteps = SoundType.PlayerSteps;

    [Header("Gates")]
    [Tooltip("Si está activo, no suena si IsHolding es true.")]
    public bool blockWhenHolding = true;
    [Tooltip("Nombre del bool del Animator para holding (según tu controlador).")]
    public string holdingBoolName = "IsHolding";

    private int _lastStepIndex;     // 0 none, 1 step1 fired, 2 step2 fired
    private float _lastCycleT;      // para detectar reinicio de ciclo más estable
    private PlayerManager _player;  // cache

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _lastStepIndex = 0;
        _lastCycleT = stateInfo.normalizedTime % 1f;

        // Cache del PlayerManager (está en el root del player)
        _player = animator.GetComponentInParent<PlayerManager>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AudioManager.Instance == null) return;

        // Gate 1: player activo
        if (_player != null && !_player.CanMove) return;

        // Gate 2: no sonar si holding (opcional)
        if (blockWhenHolding && !string.IsNullOrEmpty(holdingBoolName))
        {
            if (animator.GetBool(holdingBoolName)) return;
        }

        float t = stateInfo.normalizedTime % 1f;

        // Detecta reinicio de ciclo incluso si entró en el estado a mitad de loop
        // Si t "vuelve atrás" (ej: 0.95 -> 0.02), reinició el ciclo.
        if (t < _lastCycleT)
        {
            _lastStepIndex = 0;
        }

        _lastCycleT = t;

        // También permite reinicio cerca del 0 por estabilidad (opcional)
        if (t <= resetWindow)
        {
            _lastStepIndex = 0;
        }

        // step1
        if (_lastStepIndex < 1 && t >= step1)
        {
            PlayStep(isFirst: true);
            _lastStepIndex = 1;
        }

        // step2
        if (_lastStepIndex < 2 && t >= step2)
        {
            PlayStep(isFirst: false);
            _lastStepIndex = 2;
        }
    }

    private void PlayStep(bool isFirst)
    {
        if (useStepAB)
        {
            AudioManager.PlaySound(isFirst ? stepA : stepB, volume);
        }
        else
        {
            AudioManager.PlaySound(singleSteps, volume);
        }
    }
}
