using System.Collections.Generic;
using UnityEngine;

public static class Controls
{
    public static Dictionary<string, KeyCode> keys {  get; private set; }
    public static float lookSensitvity;

    public static void Init()
    {
        keys = new Dictionary<string, KeyCode>();

        keys.Add("Forward", KeyCode.W);
        keys.Add("Left", KeyCode.A);
        keys.Add("Right", KeyCode.D);
        keys.Add("Backward", KeyCode.S);
        keys.Add("Jump", KeyCode.Space);

        keys.Add("Dash", KeyCode.LeftShift);
        keys.Add("Switch Dimension", KeyCode.Q);

        lookSensitvity = 1f;
    }
}