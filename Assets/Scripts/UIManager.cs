using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameStateManager gsm;
    public RoundManager rm;
    public Text roundText;
    public Text phaseText;
    public Text timerText;

	// Use this for initialization
	void Start ()
    {
        roundText.GetComponent<Text>().enabled = false;
        phaseText.GetComponent<Text>().enabled = false;
        timerText.GetComponent<Text>().enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (gsm.inprogress)
        {
            roundText.GetComponent<Text>().enabled = true;
            phaseText.GetComponent<Text>().enabled = true;
            timerText.GetComponent<Text>().enabled = true;
            roundText.text = ("Round: " + rm.currentRound.ToString());
            phaseText.text = ("Phase: " + rm.phase.ToString());
            timerText.text = ("[ " + Mathf.RoundToInt(rm.timeLeft) + " ]".ToString());
        }
	}
}
