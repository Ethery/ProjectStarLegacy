using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Interactable {

    public GameObject display;
	public Fading fadePrefab;
	private InventoryManager _inventoryManager;
	private CharacterManager _player;
	private SavesManager _saves;
	private Fading fadeObject;
	
	// Use this for initialization
	void Start ()
	{
		_saves = GetComponent<SavesManager>();
		fadeObject = FindObjectOfType<Fading>();
		if (fadeObject == null)
		{
			fadeObject = Instantiate(fadePrefab);
		}
		pause(display.activeSelf);
		nRanged = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (active && EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(display.transform.FindChild("ResumeButton").gameObject);
		}
	}

	void pause(bool on)
	{
		if (on)
		{
			if (Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
				active = on;
				display.SetActive(active);
			}
		}
		else
		{
			active = on;
			display.SetActive(active);
			Time.timeScale = 1f;
		}
	}

	public void resume()
	{
		pause(false);
	}

	public void options()
	{
	
	}

	public void quit(bool save)
	{
		if (save)
		{
			_saves.saveAll(PlayerPrefs.GetInt("SessionID", 0));
		}
		fadeObject.FadeTo("Acceuil");
	}

	public override bool Activate(bool a, string key)
	{
		if (a && key == "Pause")
		{
			active = a;
			pause(active);
			if (active)
				EventSystem.current.SetSelectedGameObject(display.transform.FindChild("ResumeButton").gameObject);
			return true;
		}
		if (!a && key == "Cancel")
		{
			active = a;
			pause(active);
			return true;
		}
		return false;
	}


	public override void Activate(bool a)
	{
		if (a)
		{
			active = a;
			pause(!active);
			if (active)
				EventSystem.current.SetSelectedGameObject(display.transform.FindChild("ResumeButton").gameObject);
		}
		if (!a)
		{
			active = a;
			pause(active);
		}
	}
}
