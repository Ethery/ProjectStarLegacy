using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectDictionaryItem))]
public class Lootable : MonoBehaviour {

	public int healAmount = 5;
	public int itemAmount = 2;
    public Lootable parentFuse;

	// Use this for initialization
	void Start () {
        name = GetComponent<ObjectDictionaryItem>().nom;

    }

	public void use()
	{
		GameObject.Find("Player").GetComponent<HealthBar>().setHeal(healAmount);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.name == "Player")
		{
			FindObjectOfType<InventoryManager>().addItem(GetComponent<ObjectDictionaryItem>().nom, itemAmount);
			Destroy(gameObject);
		}
		else if (collision.collider.GetComponent<Lootable>() != null && collision.gameObject.GetComponent<ObjectDictionaryItem>().nom == GetComponent<ObjectDictionaryItem>().nom)
		{
            if (parentFuse == null)
            {
                parentFuse = Instantiate(this);
                collision.collider.GetComponent<Lootable>().parentFuse = parentFuse;
                parentFuse.itemAmount += collision.collider.GetComponent<Lootable>().itemAmount;
                Destroy(collision.gameObject);
                parentFuse.name = "Lootable";
                Destroy(gameObject);
            }
		}
	}
}
