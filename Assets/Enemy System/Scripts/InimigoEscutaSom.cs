using UnityEngine;

public class InimigoEscutaSom : MonoBehaviour, IEscutaSom
{
    InimigoTipo1 inimigoTipo1;

    void Start()
    {
        inimigoTipo1 = GetComponent<InimigoTipo1>();    
    }

    public void ReageASom(Som som)
    {
        inimigoTipo1.SetaSom(som);
    }
}
