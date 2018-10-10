using UnityEngine;

public class Fired : MonoBehaviour {

    public bool fired;

    // Use this for initialization
    void Start ()
    {
        fired = false;
    }

    public bool DidPlayerFire()
    {
        return fired;
    }

    public void SetFiredTrue()
    {
        fired = true;
    }

    public void SetFiredFalse()
    {
        fired = false;
    }
}
