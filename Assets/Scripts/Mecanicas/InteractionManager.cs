using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Input Setup")]
    public PlayerInput playerInput; // Arrastra el PlayerInput aquí
    private InputAction _multipleAction; // La acción de 3 clicks

    [Header("Referencias")]
    public Camera puzzleCamera; 
    public LayerMask puzzleLayer; // Asegúrate que los tornillos tengan este Layer

    void Start()
    {
        // CORRECCIÓN: Usamos FindAction que busca la acción recursivamente en el asset asignado
        // El segundo parámetro 'true' hace que Unity te avise en consola si no la encuentra (evita NullReference silencioso)
        _multipleAction = playerInput.actions.FindAction("Multiple", true);
    
        // Nos suscribimos al evento
        _multipleAction.performed += OnTripleClick;
    }

    private void OnTripleClick(InputAction.CallbackContext context)
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        // Lanzamos el Rayo exactamente donde está el mouse en ese instante
        Ray ray = puzzleCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, puzzleLayer))
        {
            // Buscamos si el objeto tiene el script del tornillo
            ScrewPuzzle screw = hit.collider.GetComponent<ScrewPuzzle>();
            
            if (screw != null)
            {
                // ¡BAM! Ejecutamos la acción de sacar el tornillo
                screw.TriggerUnscrew();
            }
        }
    }
    
    // Importante: Desuscribirse al destruir el objeto para evitar errores
    private void OnDestroy()
    {
        if (_multipleAction != null) _multipleAction.performed -= OnTripleClick;
    }
}