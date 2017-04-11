using UnityEngine;
using System.Collections;

public class EnemyLifeManager : MonoBehaviour {

	public int totalHealth = 10;
	public int actualHealth;
    public GameObject mainGameObject;

	Animator anim;

    public FluctuationLife dialogue;

    void Start ()
	{
		anim = GetComponent<Animator>();
		actualHealth = totalHealth;
	}

	public void takeDamage(int amount)
	{
		actualHealth -= amount;
        HealthBar.afficherFluctuationLife(dialogue, this.transform, false, amount);
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
        if (anim != null)
            anim.SetBool("Destroyed", true);
        else
            Destroy(mainGameObject);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<ShotsManager>() != null)
			takeDamage(collision.GetComponent<ShotsManager>().damage);
	}
}
