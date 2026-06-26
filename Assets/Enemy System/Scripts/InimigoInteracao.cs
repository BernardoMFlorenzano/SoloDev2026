using System;
using System.Collections.Generic;
using UnityEngine;

public class InimigoInteracao : MonoBehaviour
{
    //List<IInteragivel> interagiveisEmRange = new List<IInteragivel>();  // Lista de todos os interagiveis em range

    //IInteragivel interagivelAtual = null;

    void FixedUpdate()
    {

    }


    // 
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Porta porta) && !porta.GetEstado())
        {
            Debug.Log("Colisor viu porta");
            porta?.Interagir();
        }
    }

    // 
    void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    
}
