using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyLifeManager : MonoBehaviour {

    public Transform dialogue;
    public int totalHealth = 10;
	public int actualHealth;
    public GameObject mainGameObject;

    public ToSaveObject tso;

	Animator anim;
    

    void Start ()
	{
		anim = GetComponent<Animator>();
		actualHealth = totalHealth;

        List<Parameter> l = FindObjectOfType<SavesManager>().prog.levelObjects;
        if (l.Exists((e) =>
        {
            return (e.father == mainGameObject.name);
        }))
        {
            l = l.FindAll((e) =>
            {
                return (e.father == mainGameObject.name);
            });
            tso = new ToSaveObject(l);
            if ((bool)tso.getObject("dead"))
            {
                Destroy(mainGameObject);
            }
        }
        else
        {
            tso = new ToSaveObject();
            tso.addObject(mainGameObject.name, "id", name);
            tso.addObject(mainGameObject.name, "dead", false);
        }
        
    }

	public void takeDamage(int amount)
	{
		actualHealth -= amount;
        afficherFluctuationLife(false, amount);
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
        {
            Destroy(mainGameObject);
        }
        tso.setObject(mainGameObject.name, "dead", true);
        tso.saveThis();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<ShotsManager>() != null)
			takeDamage(collision.GetComponent<ShotsManager>().damage);
	}

    public void afficherFluctuationLife(bool isHeal, float value)
    {
        if (dialogue != null)
        {
            // Création d'un objet copie du prefab
            var dialogueTransform = Instantiate(dialogue,transform,false) as Transform;


            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            dialogueTransform.transform.position = new Vector2(x, y);

            if (isHeal)
            {
                dialogueTransform.GetComponent<Text>().color = Color.green;
                dialogueTransform.GetComponent<Text>().text = "+" + value;
            }
            else
            {
                dialogueTransform.GetComponent<Text>().color = Color.red;
                dialogueTransform.GetComponent<Text>().text = "-" + value;
            }
        }
    }
}
