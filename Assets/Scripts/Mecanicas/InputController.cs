using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private InputAction hold;
    private InputAction multiple;

    private bool isHolding;

    public Action<InteractionMode> OnClickInteraction;

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

    private void OnDisable()
    {
        hold.performed -= Hold_performed;
        hold.canceled -= Hold_canceled;

        multiple.performed -= Multiple_performed;
    }

    private void Hold_performed(InputAction.CallbackContext obj)
    {
        isHolding = true;

        OnClickInteraction?.Invoke(InteractionMode.Hold);
        Debug.Log("Holded");
    }

    private void Hold_canceled(InputAction.CallbackContext obj)
    {
        if (isHolding)
        {
            isHolding = false;
            OnClickInteraction?.Invoke(InteractionMode.HoldEnd);
            Debug.Log("Holded END");
        }
    }

    private void Multiple_performed(InputAction.CallbackContext obj)
    {
        OnClickInteraction?.Invoke(InteractionMode.Multiple);
        Debug.Log("Multiple");
    }
}

public enum InteractionMode { Hold, HoldEnd, Multiple };