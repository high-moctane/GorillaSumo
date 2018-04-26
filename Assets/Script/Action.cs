using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public float forceX, forceZ;

    public Action(float x, float z)
    {
        forceX = Mathf.Clamp(x, Prefs.forceMin, Prefs.forceMax);
        forceZ = Mathf.Clamp(z, Prefs.forceMin, Prefs.forceMax);
    }

    public override string ToString()
    {
        return "(" + forceX.ToString() + ", " + forceZ.ToString() + ")";
    }
}
