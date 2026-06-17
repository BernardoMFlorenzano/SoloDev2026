using System.Collections.Generic;
using UnityEngine;

public static class GameSonsManager
{
    public static void PropagaSom(Som som)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(som.pos, som.range);

        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].TryGetComponent(out IEscutaSom escutaSom))
            {
                escutaSom.ReageASom(som);
            }
        }
    }
}
