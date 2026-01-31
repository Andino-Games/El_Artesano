using UnityEngine;
using UnityEngine.InputSystem; // Los 'using' siempre deben ir aquí arriba

public class MovimientoJugador : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController controller;
    public Transform cam; // ¡No olvides arrastrar la Main Camera aquí en el Inspector!

    [Header("Configuración")]
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector2 moveInput;

    // Recibe el input del componente Player Input
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // 1. Creamos el vector de dirección basado en WASD
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // 2. Ajustamos el ángulo para que sea relativo a la cámara
            // Esto hace que W sea "arriba" en tu pantalla, sin importar la rotación isométrica
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // 3. Rotación suave del personaje
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 4. Movemos al jugador en esa dirección relativa
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // 5. Aplicar gravedad constante
        AplicarGravedad();
    }

    void AplicarGravedad()
    {
        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
        else
        {
            controller.Move(Vector3.down * 0.2f * Time.deltaTime); // Mantiene al player pegado al suelo
        }
    }
}