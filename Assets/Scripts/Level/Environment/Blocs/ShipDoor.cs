using UnityEngine;
using System.Collections;
using System;

public class ShipDoor : Interactable {

	public float vitesseDOuverture;
	public Transform objectif;

	public float tempsResteOuvert;

	private Vector2 directionOuverture;

	private Vector2 positionDOrigine;
	private bool opened;

	// Use this for initialization
	void Start () {
		positionDOrigine = transform.position;
		directionOuverture = objectif.position;
	}
	

	IEnumerator openDoor()
	{
		opened = true;
		float t = Vector3.Distance(directionOuverture, transform.position);
		while (t > 0.1)
		{
			t = Vector3.Distance(directionOuverture, transform.position);
			transform.position = Vector2.Lerp(transform.position, directionOuverture, Time.deltaTime * vitesseDOuverture);
			yield return null;
		}
		t = 0f;
		while (t < tempsResteOuvert)
		{
			t += Time.deltaTime;
			yield return null;
		}
		t = Vector3.Distance(positionDOrigine, transform.position);
		while (t > 0.1)
		{
			t = Vector3.Distance(positionDOrigine, transform.position);
			transform.position = Vector2.Lerp(transform.position, positionDOrigine, Time.deltaTime * vitesseDOuverture);
			yield return null;
		}
		opened = false;
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null)
		{
			ranged = !opened;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null || opened)
		{
			ranged = false;
		}
	}

	public override bool Activate(bool a, string key)
	{
		if (a && key == "Submit" && !opened)
		{
			StartCoroutine(openDoor());
			return true;
		}
		return false;
	}

	public override void Activate(bool a)
	{
		if (a)
		{
			StartCoroutine(openDoor());
		}
	}
}
