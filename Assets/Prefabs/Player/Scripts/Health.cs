using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public int startingHealth;
    public bool alive;
    public float currentHealth;
    private Vector3 localScale;
    public Bomb bomb;

	// Use this for initialization
	void Start ()
    {
        startingHealth = 100;
        currentHealth = startingHealth;
        alive = true;
        localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
    {
        localScale.x -= currentHealth;

        if (currentHealth <= 0)

        //Check to see if the player loses all their health
	    if (currentHealth <= 0)
        {
            //Set the player alive bool to false.
            Debug.Log("Player died!");
            Destroy(gameObject);
            alive = false;
        }	
	}

    public void ApplyDamage(int damage)
    {
        //The player takes damage.
        currentHealth -= damage;
        Debug.Log(gameObject + "took " + damage + " !");
    }
}
