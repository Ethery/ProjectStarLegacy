using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : Interactable {

	public Transform buttonPrefab;
	public GameObject mainDisplay;
	public Transform contentList;

	public List<LevelStats> levels;
	private bool pressed;


	// Use this for initialization
	void Start () {
		levels = FindObjectOfType<SavesManager>().main.levels;
		pressed = false;
		foreach (LevelStats lv in levels)
		{
			if (lv.nom != SceneManager.GetActiveScene().name&&lv.id>=3)
			{
				Transform button = Instantiate(buttonPrefab, contentList,false);
				button.name = lv.id.ToString();
				if (lv.nom == "")
				{
					button.FindChild("Nom").GetComponent<Text>().text = "Inconnu";
				}
				else
				{
					button.FindChild("Nom").GetComponent<Text>().text = lv.nom;
				}
				if (lv.finished)
				{
					button.FindChild("Visite").GetComponent<Text>().text = "Exploré a 100%";
				}
				else
				{
					button.FindChild("Visite").GetComponent<Text>().text = "Non exploré";
				}
			}
		}
		mainDisplay.SetActive(active);
	}
	
	// Update is called once per frame
	void Update () {
        if (active && EventSystem.current.currentSelectedGameObject == null)
		{
			if (contentList.childCount > 0)
			{
				EventSystem.current.SetSelectedGameObject(contentList.GetChild(0).gameObject);
			}
		}
	}
	
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<CharacterManager>() != null)
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
			active = a;
			mainDisplay.SetActive(a);
			FindObjectOfType<PlayerInputManager>().canMove = false;
			Time.timeScale = 0f;
			return true;
		}
		if (!a && key == "Cancel")
		{
			active = a;
			mainDisplay.SetActive(a);
			FindObjectOfType<PlayerInputManager>().canMove = true;
			Time.timeScale = 1f;
			return true;
		}
		return false;
	}

	public override void Activate(bool a)
	{
		active = a;
		mainDisplay.SetActive(a);
		FindObjectOfType<PlayerInputManager>().canMove = !a;
		if (a)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public override bool nextStep(string key)
	{
		if (key == "Submit")
		{
			FindObjectOfType<PlayerInputManager>().canMove = false;
			FindObjectOfType<PlayerInputManager>().canUse = false;
			FindObjectOfType<Fading>().FadeTo(int.Parse(EventSystem.current.currentSelectedGameObject.name));
			Activate(false);
			return false;
		}
		return true;
	}

	public override bool previousStep(string key)
	{
		if (key == "Cancel")
		{
			Activate(false, key);
		}
		return true;

	}
}
