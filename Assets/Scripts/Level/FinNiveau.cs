using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class FinNiveau : Interactable {

	// Use this for initialization
	void Start () {
		ranged = false;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null && GetComponent<CheckPoint>().saved)
		{
			ranged = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null)
		{
			ranged = false;
		}
	}

	public override bool Activate(bool a, string key)
	{
		if (a && key == "Submit")
		{
			FindObjectOfType<SavesManager>().finished();
			FindObjectOfType<Fading>().FadeTo("Navette");
			return true;
		}
		return false;
	}

	public override void Activate(bool a)
	{
		if (a)
		{
			FindObjectOfType<SavesManager>().finished();
			FindObjectOfType<Fading>().FadeTo("Navette");
		}
	}
}
