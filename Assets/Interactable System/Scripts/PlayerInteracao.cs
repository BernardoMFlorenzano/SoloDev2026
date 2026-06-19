using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteracao : MonoBehaviour
{
    List<IInteragivel> interagiveisEmRange = new List<IInteragivel>();  // Lista de todos os interagiveis em range

    public static event Action<int, Transform> OnInteracao; // Evento disparado apos um objeto ser interagido

    IInteragivel interagivelAtual = null;

    // Listener de eventos de input
    private void OnEnable()
    {
        InputPlayerManager.OnInteractInput += Interage;
    }
    private void OnDisable()
    {
        InputPlayerManager.OnInteractInput -= Interage;
    }

    void FixedUpdate()
    {
        if (interagiveisEmRange.Count > 0)
        {
            IInteragivel interagivelBusca = null;
            float distancia = 100f;
            float distanciaAux;
            foreach (IInteragivel interagivel in interagiveisEmRange)
            {
                distanciaAux = Vector2.Distance(transform.position, interagivel.transform.position);
                if (distanciaAux <= distancia)
                {
                    distancia = distanciaAux;
                    interagivelBusca = interagivel;
                }
            }

            if (interagivelBusca != null)
            {
                if (interagivelAtual != null)
                {
                    interagivelAtual.EscondeInput();
                }
                interagivelBusca.MostraInput();

                interagivelAtual = interagivelBusca;
            }
        }
        else if (interagivelAtual != null)
        {
            interagivelAtual.EscondeInput();
            interagivelAtual = null;
        }
    }

    // Função disparada ao ser detectado input, que escolhe qual objeto no range vai ser interagido e/ou dispara evento de interação
    public void Interage()
    {
        if (interagivelAtual != null && interagivelAtual.PodeInteragir())
        {
            interagivelAtual?.Interagir();

            OnInteracao?.Invoke(interagivelAtual.GetTipo(), interagivelAtual.transform);
        }

        else
        {
            OnInteracao?.Invoke(-1, null);
            Debug.Log("Nenhum interagivel valido em range");
        }
    }


    // Interagiveis que entram na área são adicionados a lista
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteragivel interagivel))
        {
            interagiveisEmRange.Add(interagivel);
        }
    }

    // Interagiveis que entram na área são removidos da lista
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteragivel interagivel))
        {
            interagiveisEmRange.Remove(interagivel);
        }
    }

    
}
