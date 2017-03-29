using UnityEngine;
using UnityEngine.UI;

public class TalkingManager : MonoBehaviour {

	public TextConvManager convManager;
	public TextAsset textFile;
	private Transform button;
	private bool ranged;

	private void Start()
	{
		button = GameObject.FindGameObjectWithTag("Player").transform.Find("button");
		button.GetComponentInChildren<Text>().text = "E";
	}

	private void Update ()
	{
		button.gameObject.SetActive(ranged);

		if (!convManager.isActive && ranged && Input.GetButtonDown("Submit"))
		{
			convManager.StartNewConv(textFile);
		}
		if (convManager.isActive && Input.GetButtonDown("Cancel"))
		{
			convManager.setActive(false);
		}
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ranged = true;
			button.gameObject.SetActive(ranged);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ranged = false;
			button.gameObject.SetActive(ranged);
		}
	}
}
