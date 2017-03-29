using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Xml;

public class TextConvManager : MonoBehaviour {

	private int currentPhrase;
	public bool isActive, updated;
	public Conversation fileConversation;
	private List<Phrase> currentConv;
	public GameObject display;
	public Transform buttonGroup;

	private bool isTyping = false;
	private bool cancelTyping = false;
	[Range(0,0.5f)]
	public float typeSpeed;

	public List<Sprite> sp;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < buttonGroup.childCount; i++)
		{
			buttonGroup.GetChild(i).name = i.ToString();
		}
		setActive(false);
	}
	
	void Update ()
	{
		//Si le dialogue n'a pas �t� update et est actif
		if (!updated && isActive)
		{
			//on update le texte
			UpdateTexts();
		}

		//Si on appuie le bouton d'action et que le dialogue est actif
		if (Input.GetButtonUp("Submit") && isActive)
		{
			//Si la coroutine d'ecriture est en route
			if (!isTyping)
			{
				// Si laphrase en cours a des options.
				if (currentConv[currentPhrase].Options.Count != 0)
				{
					int number;
					// On recupere l'id de la reponse
					int.TryParse(EventSystem.current.currentSelectedGameObject.name, out number);

					//Si l'options selectionn�e n'a pas de resultat (mets fin a la conversation)
					Debug.Log(currentConv[currentPhrase].Options[number].Resultat.Count);
					if (currentConv[currentPhrase].Options[number].Resultat.Count == 0)
					{
						//On desactive la fenetre.
						setActive(false);
					}
					
					// Sinon
					// \/

					// la nouvelle conv est le resultat de la r�ponse selectionn�e
					currentConv = currentConv[currentPhrase].Options[number].Resultat;
					
					// On reset la phrase de la conv.
					currentPhrase = 0;
					// On dis que l'affichage doit �tre update
				}
				// Si la phrase n'a pas d'options
				else
				{
					// Si la conversation � une phrase suivante.
					if (currentPhrase + 1 < currentConv.Count)
					{
						//On passe a la phrase suivante
						currentPhrase++;
						updated = false;
					}
					//Sinon
					else
					{
						setActive(false);
					}
				}
				updated = false;
			}
			else if (isTyping && !cancelTyping)
			{
				cancelTyping = true;
			}
		}
	}

	private IEnumerator TextScroll(Text theText, string lineOfText)
	{
		for (int i = 0; i < buttonGroup.childCount; i++)
		{
			buttonGroup.GetChild(i).gameObject.SetActive(false);
		}
		int letter = 0;

		theText.text = "";

		isTyping = true;
		cancelTyping = false;
		for (int i = 0; i < sp.Count; i++)
		{
			if (sp[i].name == currentConv[currentPhrase].Emotion)
			{
				display.transform.FindChild("Vignette").GetComponent<Image>().sprite = sp[i];
				break;
			}
		}

		while (isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
		{
			theText.text += lineOfText[letter];
			letter++;
			yield return new WaitForSecondsRealtime(typeSpeed);
		}
		theText.text = lineOfText;
		
		//Si la phrase actuelle a des options possibles.
		if (currentConv[currentPhrase].Options.Count != 0)
		{
			for (int i = 0; i < buttonGroup.childCount; i++)
			{
				if (i < currentConv[currentPhrase].Options.Count)
				{
					buttonGroup.GetChild(i).gameObject.SetActive(true);
					buttonGroup.GetChild(i).GetComponentInChildren<Text>().text = currentConv[currentPhrase].Options[i].Text;
				}
				else
				{
					buttonGroup.GetChild(i).gameObject.SetActive(false);
				}
			}
			EventSystem.current.SetSelectedGameObject(buttonGroup.GetChild(0).gameObject);
		}
		else if (currentConv[currentPhrase].Options.Count == 0)
		{
			for (int i = 0; i < buttonGroup.childCount; i++)
			{
				buttonGroup.GetChild(i).gameObject.SetActive(false);
			}
		}
		else if (currentConv[currentPhrase].Options.Count > buttonGroup.childCount)
		{
			Debug.Log("Trop d'options reglez le script OMG");
		}
		isTyping = false;
		cancelTyping = false;
	}

	public void UpdateTexts()
	{
		if (isActive)
		{
			StartCoroutine(TextScroll(display.transform.FindChild("Texte").GetComponent<Text>(), currentConv[currentPhrase].Texte));
			updated = true;
		}
	}

	public void setActive(bool active)
	{
		isActive = active;
		if (active)
		{
			display.gameObject.SetActive(isActive);
			currentConv = fileConversation.phrases;
			Time.timeScale = 0f;
		}
		else
		{
			display.gameObject.SetActive(isActive);
			Time.timeScale = 1f;
		}
	}

	public void StartNewConv(TextAsset fileName)
	{
		var serializer = new XmlSerializer(typeof(Conversation));
		using (var reader = new System.IO.StringReader(fileName.text))
		{
			fileConversation = serializer.Deserialize(reader) as Conversation;
		}
		setActive(true);
		currentPhrase = 0;
	}
}
