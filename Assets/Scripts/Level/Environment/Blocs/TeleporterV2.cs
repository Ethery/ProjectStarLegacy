using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TeleporterV2 : MonoBehaviour {

	public Button prefabDestinationButton;
	public GameObject mainDisplay;
	public Transform contentList;
	public Animator fonduAnime;
	private List<TeleporterV2> listTps = new List<TeleporterV2>();
	private bool opened,ranged,filled,pressed;
	private string to;

	// Use this for initialization
	void Start () {
		to = "";
		TeleporterV2[] tmp = FindObjectsOfType<TeleporterV2>();
		foreach (TeleporterV2 item in tmp)
		{
			if (item != this)
			{
				listTps.Add(item);
			}
		}
		filled = false;
		opened = false;
		ranged = false;
		pressed = false;
		mainDisplay.SetActive(opened);
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
		if (ranged && filled)
		{
			if (!opened && Input.GetButtonDown("Submit"))
			{
				opened = true;
				mainDisplay.SetActive(opened);
				pressed = true;
				Time.timeScale = 0f;
			}
			if (Input.GetButtonUp("Submit"))
				pressed = false;
			if (!pressed && opened && Input.GetButtonDown("Submit"))
			{
				FindObjectOfType<PlayerInputManager>().canMove = false;
				fonduAnime.SetTrigger("start");
				GetComponent<Animator>().SetTrigger("Started");
				FindObjectOfType<PlayerInputManager>().canUse = false;
				opened = false;
				mainDisplay.SetActive(opened);
				Time.timeScale = 1f;
			}
			if (opened && Input.GetButtonDown("Cancel"))
			{
				opened = false;
				mainDisplay.SetActive(opened);
				Time.timeScale = 1f;
			}
		}
		if (opened && EventSystem.current.currentSelectedGameObject != null)
		{
			to = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>(true).text;
		}
		else if (opened && EventSystem.current.currentSelectedGameObject == null)
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
			collision.GetComponent<PlayerInputManager>().canUse = true;
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
			collision.GetComponent<PlayerInputManager>().canUse = false;
		}

	}

	public void Teleport()
	{
		foreach (TeleporterV2 item in listTps)
		{
			if (item.name == to)
			{
				FindObjectOfType<CharacterManager>().transform.position = item.transform.position;
				FindObjectOfType<PlayerInputManager>().canMove = true;
				fonduAnime.SetTrigger("start");
				filled = false;
				break;
			}
		}
	}
	
}
