using UnityEngine;

public class Aim : MonoBehaviour {

    public Vector2 aimDegree;
    public TwitchClient tc;

    public void Start()
    {
        tc = GameObject.Find("GameManager").GetComponentInChildren<TwitchClient>();
    }

    public void SetAimToAngle(int angle)
    {
        aimDegree = (Quaternion.Euler(0, 0, angle) * Vector2.right);
    }
}
