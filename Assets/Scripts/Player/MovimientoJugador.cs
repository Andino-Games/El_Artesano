using UnityEngine;
using UnityEngine.InputSystem; // Los 'using' siempre deben ir aqu� arriba

public class MovimientoJugador : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController controller;
    public Transform cam; // �No olvides arrastrar la Main Camera aqu� en el Inspector!
    [SerializeField] private Animator animator;

    [Header("Configuracion")]
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector2 moveInput;

    public Animator Anim => animator;

    // Recibe el input del componente Player Input
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // 1. Creamos el vector de direcci�n basado en WASD
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // 2. Ajustamos el �ngulo para que sea relativo a la c�mara
            // Esto hace que W sea "arriba" en tu pantalla, sin importar la rotaci�n isom�trica
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // 3. Rotaci�n suave del personaje
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 4. Movemos al jugador en esa direcci�n relativa
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed * Time.deltaTime));

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // 5. Aplicar gravedad constante
        AplicarGravedad();
    }

    void AplicarGravedad()
    {
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * (20f * Time.deltaTime));
        }
        else
        {
            controller.Move(Vector3.down * (0.2f * Time.deltaTime)); // Mantiene al player pegado al suelo
        }
    }

    public void TeleportTo(Vector3 position)
    {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }
}