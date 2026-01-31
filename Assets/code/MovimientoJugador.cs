using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController controller;
    public Transform planoReferencia;

    [Header("Configuración")]
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // 1. Capturamos el input y lo INVERTIMOS para corregir el sentido
        // Multiplicamos por -1 para que W sea adelante y D sea derecha
        Vector3 inputVector = new Vector3(-moveInput.x, 0f, -moveInput.y);

        if (inputVector.magnitude >= 0.1f)
        {
            // 2. MATRIZ DE ROTACIÓN ISOMÉTRICA
            // Ajustamos el ángulo (45°) sumado a la rotación del plano
            float isometricAngle = 45f;
            Quaternion rotation = Quaternion.Euler(0, isometricAngle + planoReferencia.eulerAngles.y, 0);
            Vector3 moveDir = rotation * inputVector;

            // 3. ROTACIÓN DEL PERSONAJE
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // 4. MOVIMIENTO FINAL
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

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
            controller.Move(Vector3.down * 0.2f * Time.deltaTime);
        }
    }
}