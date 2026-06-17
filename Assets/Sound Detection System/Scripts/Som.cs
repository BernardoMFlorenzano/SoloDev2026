using UnityEngine;

public class Som
{

    public enum TipoSom {Default = -1, Desconhecido, Perigo}

    public Som(Vector3 _pos, float _range)
    {
        pos = _pos;
        range = _range;
    }

    public TipoSom tipoSom;

    public readonly Vector3 pos;

    public readonly float range;
}
