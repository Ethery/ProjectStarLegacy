using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectDictionaryItem))]
[RequireComponent(typeof(ToSaveObject))]
public class Lootable : MonoBehaviour {

	public int healAmount = 5;
	public int itemAmount = 2;
    public Lootable parentFuse;
    public ToSaveObject tso;

	// Use this for initialization
	void Start ()
    {
        //name = GetComponent<ObjectDictionaryItem>().nom;
        List<Parameter> l = FindObjectOfType<SavesManager>().prog.levelObjects;
        if (l.Exists((e) =>
        {
            return (e.father == name);
        }))
        {
            l = l.FindAll((e) =>
            {
                return (e.father == name);
            });
            tso = new ToSaveObject(l);
            if ((bool)tso.getObject("looted"))
            {
                Destroy(gameObject);
            }
            transform.position = new Vector3((float)(tso.getObject("position.x")), (float)(tso.getObject("position.y")));
            transform.rotation = Quaternion.Euler((float)tso.getObject("rotation.x"), (float)tso.getObject("rotation.y"), (float)tso.getObject("rotation.z"));
            
        }
        else
        {
            tso = new ToSaveObject();
            tso.addObject(name, "id", name);
            tso.addObject(name, "position.x", transform.position.x);
            tso.addObject(name, "position.y", transform.position.y);
            tso.addObject(name, "rotation.x", transform.rotation.eulerAngles.x);
            tso.addObject(name, "rotation.y", transform.rotation.eulerAngles.y);
            tso.addObject(name, "rotation.z", transform.rotation.eulerAngles.z);
            tso.addObject(name, "itemAmount", itemAmount);
            tso.addObject(name, "looted", false);
        }
        
    }

    private void Update()
    {
        tso.setObject(name, "position.x", transform.position.x);
        tso.setObject(name, "position.y", transform.position.y);
        tso.setObject(name, "rotation.x", transform.rotation.eulerAngles.x);
        tso.setObject(name, "rotation.y", transform.rotation.eulerAngles.y);
        tso.setObject(name, "rotation.z", transform.rotation.eulerAngles.z);
    }

    public void use()
	{
		GameObject.Find("Player").GetComponent<HealthBar>().setHeal(healAmount);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.name == "Player")
		{
			FindObjectOfType<InventoryManager>().addItem(new ItemShort(GetComponent<ObjectDictionaryItem>().nom, itemAmount));
            tso.setObject(name, "looted", true);
            tso.saveThis();
            Destroy(gameObject);
        }
		else if (collision.collider.GetComponent<Lootable>() != null && collision.gameObject.GetComponent<ObjectDictionaryItem>().nom == GetComponent<ObjectDictionaryItem>().nom)
		{
            if (parentFuse == null)
            {
                tso.setObject(name, "looted",true);
                tso.saveThis();
                parentFuse = Instantiate(this);
                collision.collider.GetComponent<Lootable>().parentFuse = parentFuse;
                parentFuse.itemAmount += collision.collider.GetComponent<Lootable>().itemAmount;
                parentFuse.tso.setObject(name, "itemAmount", itemAmount);
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
		}
	}
}
