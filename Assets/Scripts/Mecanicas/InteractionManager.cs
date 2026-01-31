using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Input Setup")]
    public PlayerInput playerInput;
    public Camera puzzleCamera;
    public LayerMask puzzleLayer;

    private InputAction _holdAction;
    private InputAction _multipleAction;
    
    private ScrewMechanic _currentTarget; // Lo que estamos mirando/tocando
    private bool _isHolding;

    void Start()
    {
        // Configurar referencias a las acciones exactas de tu archivo
        _holdAction = playerInput.actions.FindAction("Hold", true);
        _multipleAction = playerInput.actions.FindAction("Multiple", true);

        // Suscripción a eventos
        _holdAction.started += ctx => _isHolding = true;
        _holdAction.canceled += ctx => _isHolding = false;
        
        _multipleAction.performed += ctx => TryMultipleClick();
    }

    void Update()
    {
        // Lógica constante para el HOLD
        if (_isHolding)
        {
            RaycastAndInteract(true); 
        }
    }

    // Se llama solo cuando se completan los 3 clicks
    void TryMultipleClick()
    {
        RaycastAndInteract(false);
    }

    void RaycastAndInteract(bool isHoldAction)
    {
        Ray ray = puzzleCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, puzzleLayer))
        {
            ScrewMechanic screw = hit.collider.GetComponent<ScrewMechanic>();
            
            if (screw != null)
            {
                if (isHoldAction)
                {
                    // Si estamos manteniendo click, le decimos al tornillo "Te están apretando"
                    screw.OnHoldInput(true);
                }
                else
                {
                    // Si fue un triple click, le decimos "Te golpearon 3 veces"
                    screw.OnMultipleClickInput();
                }
            }
            
            // Lógica extra para la Salida (Lágrima)
            LevelExit exit = hit.collider.GetComponent<LevelExit>();
            if (exit != null && !isHoldAction) // Asumiendo que entrar al nivel es con click
            {
                exit.EnterNextLevel();
            }
        }
    }
}