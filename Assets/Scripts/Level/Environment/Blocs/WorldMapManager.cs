using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour {

	public Transform buttonPrefab;
	public GameObject mainDisplay;
	public Transform contentList;

	public List<LevelStats> levels;
	private bool opened, ranged, pressed;


	// Use this for initialization
	void Start () {
		levels = FindObjectOfType<SavesManager>().main.levels;
		opened = false;
		ranged = false;
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
		mainDisplay.SetActive(opened);
	}
	
	// Update is called once per frame
	void Update () {
		if (ranged)
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
				FindObjectOfType<Fading>().FadeTo(int.Parse(EventSystem.current.currentSelectedGameObject.name));
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
        if (opened && EventSystem.current.currentSelectedGameObject == null)
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
			collision.GetComponent<PlayerInputManager>().canUse = false;
		}

	}
}
