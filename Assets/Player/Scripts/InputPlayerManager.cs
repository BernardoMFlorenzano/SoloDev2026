using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputPlayerManager : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;
    public static event Action OnInteractInput;
    public static event Action OnAttackInput;
    public static event Action OnLungeInput;
    public static event Action<bool> OnSprintInput;
    public static event Action<bool> OnCrouchInput;
    public static event Action OnPauseInput;

    [SerializeField] GameController gameController;

    Vector2 direcao;


    public void OnMove(InputValue value)
    {
        if (!VerificaInput())
        {
            return;
        }

        direcao = value.Get<Vector2>();
        direcao = direcao.normalized;
        OnMoveInput?.Invoke(direcao);
    }

    public void OnInteract()
    {
        if (!VerificaInput())
        {
            return;
        }

        OnInteractInput?.Invoke();
    }

    public void OnAttack()
    {
        if (!VerificaInput())
        {
            return;
        }

        OnAttackInput?.Invoke();
    }

    public void OnLunge()
    {
        if (!VerificaInput())
        {
            return;
        }

        OnLungeInput?.Invoke();
    }

    public void OnSprint(InputValue value)
    {
        if (!VerificaInput())
        {
            return;
        }

        if (value.isPressed)
            OnSprintInput?.Invoke(true);
        else 
            OnSprintInput?.Invoke(false);
    }

    public void OnCrouch(InputValue value)
    {
        if (!VerificaInput())
        {
            return;
        }

        if (value.isPressed)
            OnCrouchInput?.Invoke(true);
        else
            OnCrouchInput?.Invoke(false);
    }

    public void OnCancel()
    {
        OnPauseInput?.Invoke();
    }


    bool VerificaInput()
    {
        if (gameController)
        {
            if (!gameController.inputAtivo)
            {
                return false;
            }
        }

        return true;       
    }


    

}
