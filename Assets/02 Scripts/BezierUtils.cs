using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierUtils
{
    public static Vector3 Bezier(
        Vector3 P_1,
        Vector3 P_2,
        Vector3 P_3,
        Vector3 P_4,
        float value
        )
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 C = Vector3.Lerp(P_3, P_4, value);

        Vector3 D = Vector3.Lerp(A, B, value);
        Vector3 E = Vector3.Lerp(B, C, value);

        Vector3 F = Vector3.Lerp(D, E, value);

        return F;
    }
}
