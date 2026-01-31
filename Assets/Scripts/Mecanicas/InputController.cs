using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private InputAction hold;
    private InputAction multiple;

    private bool isHolding;


    private void Awake()
    {
        PlayerInput input = GetComponent<PlayerInput>();

        if (input != null)
        {
            hold = input.actions["Hold"];
            multiple = input.actions["Multiple"];
        }

        hold.performed += Hold_performed;
        hold.canceled += Hold_canceled;

        multiple.performed += Multiple_performed;
    }

    private void Hold_canceled(InputAction.CallbackContext obj)
    {
        if (isHolding)
        {
            isHolding = false;
            Debug.Log("Holded END");
        }
    }

    private void Hold_performed(InputAction.CallbackContext obj)
    {
        isHolding = true;
        Debug.Log("Holded");
    }

    private void Multiple_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Multiple");
    }
}
