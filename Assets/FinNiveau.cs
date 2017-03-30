using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinNiveau : MonoBehaviour {

	bool ranged,moved;

	// Use this for initialization
	void Start () {
		ranged = false;
		moved = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Submit") && ranged && !moved)
		{
			FindObjectOfType<SavesManager>().finished();
			FindObjectOfType<Fading>().FadeTo("Navette");
			moved = true;

		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null && GetComponent<CheckPoint>().saved)
		{
			ranged = true;
			collision.GetComponent<InputManager>().canUse = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null)
		{
			ranged = false;
			collision.GetComponent<InputManager>().canUse = false;
		}
	}
}
