using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	SavesManager _sm;
	public string id;
	public bool activated = false;

	private void Start()
	{
		_sm = FindObjectOfType<SavesManager>();
		id = transform.name.ToUpper();
	}

	private void Update()
	{
		if (activated == true)
		{
			GetComponent<Animator>().SetTrigger("Activated");
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			_sm.canSave = true;
			_sm.setLastCheckpoint(id);
			_sm.saveAll(PlayerPrefs.GetInt("SessionID", 0));
			if (!activated)
			{
				GetComponent<Animator>().SetTrigger("Activated");
				activated = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			_sm.canSave = false;
		}
	}
}
