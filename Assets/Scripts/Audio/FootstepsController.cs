using UnityEngine;

public class FootstepsController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    [Header("Steps")]
    [SerializeField] private float stepDistance = 1.4f;
    [SerializeField] private float minSpeed = 0.2f;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    private Vector3 _lastPos;
    private float _accumulated;

    private void OnEnable()
    {
        _lastPos = transform.position;
        _accumulated = 0f;
    }

    private void Update()
    {
        if (controller == null) return;
        if (!controller.isGrounded) return;

        float speed = controller.velocity.magnitude;
        if (speed < minSpeed) return;

        Vector3 pos = transform.position;
        _accumulated += Vector3.Distance(pos, _lastPos);
        _lastPos = pos;

        if (_accumulated >= stepDistance)
        {
            _accumulated = 0f;
            AudioManager.PlaySound(SoundType.PlayerSteps, volume);
        }
    }
}