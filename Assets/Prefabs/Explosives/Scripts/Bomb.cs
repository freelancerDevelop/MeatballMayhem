using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject Explosive;
    public Aim aim;
    public TwitchClient tc;

    // Use this for initialization
    void Start()
    {
        tc = GetComponentInParent<TwitchClient>();
        aim = GetComponent<Aim>();
    }

    public void ThrowBomb(int forceValueInt)
    {
        //Instantiate the explosive on the player's transform.
        GameObject bombInstance = Instantiate(Explosive, transform.position, Quaternion.identity);
        //Add force to the bomb by using the aim degree as a vector, and the forceValueInt 
        //that the player specified in the command
        bombInstance.GetComponent<Rigidbody2D>().AddForce(aim.aimDegree * forceValueInt * 2, ForceMode2D.Impulse);
    }
}