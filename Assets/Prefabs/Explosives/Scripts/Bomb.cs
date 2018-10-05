using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public int damage = 25;
    public GameObject Explosive;
    public Aim aim;
    public TwitchClient tc;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Explode()
    {
        //On impact with terrain or another player, change the sprite to explode

        //Deal damage to whatever the bomb was touching

        //Destroy the bomb after the explosion
        Destroy(this.gameObject);
    }

    public void ThrowBomb(int forceValueInt)
    {
        //Instantiate the explosive on the player's transform.
        var bomb = Instantiate(Explosive, transform.position, Quaternion.identity) as GameObject;
        //Add force to the bomb by using the aim degree as a vector, and the forceValueInt 
        //that the player specified in the command
        bomb.GetComponent<Rigidbody2D>().AddForce(aim.aimDegree * forceValueInt, ForceMode2D.Impulse);
        Debug.Log("Throwing bomb!");
    }
}
