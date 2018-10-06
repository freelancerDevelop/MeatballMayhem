using UnityEngine;

public class Fired : MonoBehaviour {

    public bool fired;
    public bool alreadyFired;

    // Use this for initialization
    void Start ()
    {
        fired = false;
        alreadyFired = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (fired == true)
        {
            //The player already fired, so disallow firing until next round.
            alreadyFired = true;
        }
        else if (fired == false)
        {
            alreadyFired = false;
        }
        else
        {
            //Do nothing because we're not in the right phase.
        }
	}
}
