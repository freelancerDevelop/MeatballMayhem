using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour {

    public Vector2 aimDegree;
    public TwitchClient tc;

    private void Start()
    {
        
    }

    public void SetAimToAngle(int angle)
    {
        aimDegree = (Quaternion.Euler(0, 0, angle) * Vector2.right);
    }
}
