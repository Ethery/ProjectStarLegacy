using UnityEngine;
using System.Collections;

public class EnemyLifeManager : MonoBehaviour {

	public int totalHealth = 10;
	public int actualHealth;

	Animator anim;

	void Start ()
	{
		anim = GetComponent<Animator>();
		actualHealth = totalHealth;
	}

	public void takeDamage(int amount)
	{
		actualHealth -= amount;
		if (actualHealth <= 0)
		{
			die();
		}
	}

	void heal(int amount)
	{
		actualHealth += amount;
	}

	void die()
	{
		anim.SetBool("Destroyed", true);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<ShotsManager>() != null)
			takeDamage(collision.GetComponent<ShotsManager>().damage);
	}
}
