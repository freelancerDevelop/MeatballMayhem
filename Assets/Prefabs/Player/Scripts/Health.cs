using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int health;
    public int currentHealth;
    public bool alive;
    public Slider healthSlider;
    public Bomb bomb;

	// Use this for initialization
	void Start ()
    {
        alive = true;
        health = 100;
        healthSlider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //healthSlider.value = currentHealth;

	    if (health <= 0)
        {
            //Set the player alive bool to false.
            alive = false;
            //Disallow movement or actions.
        }	
	}

    public void TakeDamage()
    {
        currentHealth -= bomb.damage;
    }
}
