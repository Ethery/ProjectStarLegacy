using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class TeleporterV2 : Interactable {

	public Button prefabDestinationButton;
	public GameObject mainDisplay;
	public Transform contentList;
	public Animator fonduAnime;
	private List<TeleporterV2> listTps = new List<TeleporterV2>();
	private bool filled,tp;

	// Use this for initialization
	void Start () {
		TeleporterV2[] tmp = FindObjectsOfType<TeleporterV2>();
		foreach (TeleporterV2 item in tmp)
		{
			if (item != this)
			{
				listTps.Add(item);
			}
		}
		filled = false;
		mainDisplay.SetActive(active);
	}

	private void Update()
	{
		if (ranged && !filled)
		{
			for (int i = contentList.childCount - 1; i >= 0; i--)
			{
				Destroy(contentList.GetChild(i).gameObject);
			}
			foreach (TeleporterV2 item in listTps)
			{
				if (item != this)
				{
					Button tmp = Instantiate<Button>(prefabDestinationButton, contentList,false);
					tmp.GetComponentInChildren<Text>().text = item.name;
				}
			}
			filled = true;
		}
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
			for (int i = contentList.childCount-1; i >=0; i--)
			{
				Destroy(contentList.GetChild(i).gameObject);
			}
			filled = false;
		}

	}

	public void Teleport()
	{
        if (!tp)
        {
            foreach (TeleporterV2 item in listTps)
            {
                if (item.name == EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>(true).text)
                {
                    FindObjectOfType<CharacterManager>().transform.position = item.transform.position;
                    FindObjectOfType<PlayerInputManager>().canMove = true;
                    FindObjectOfType<PlayerInputManager>().canUse = true;
                    fonduAnime.SetTrigger("start");
                    filled = false;
                    tp = true;
                    Activate(false);
                    break;
                }
            }
        }
	}

	public override bool Activate(bool a, string key)
	{
		if (ranged && a && key == "Submit")
		{
			active = a;
			if (a)
			{
				init();
			}
			mainDisplay.SetActive(active);
			FindObjectOfType<PlayerInputManager>().canMove = false;
			Time.timeScale = 0f;
            tp = false;
			return true;
		}
		if (!a && key == "Cancel")
		{
			active = a;
			mainDisplay.SetActive(active);
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
            tp = false;
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
			fonduAnime.SetTrigger("start");
			GetComponent<Animator>().SetTrigger("Started");
			return true;
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
