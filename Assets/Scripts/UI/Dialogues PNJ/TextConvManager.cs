using UnityEngine;
using System.Xml.Serialization;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class TextConvManager : MonoBehaviour {

	private int currentPhrase;
	public bool updated;
    public List<Conversation> cheminConversation;
	private Conversation currentConv;
	public GameObject display;
	public Transform buttonGroup;
    public string namePnj;

    private GameObject lastSelectedGameObject;
	private bool isTyping = false;
	private bool cancelTyping = false;
    private string endedParsing = "";
	[Range(0,50f)]
	public float typeSpeed;
    private int multipleDisplay,numberOfLines;

	private bool active;

    public List<Sprite> sp;

	// Use this for initialization
	void Start () {
		/*TestDialoguesXML test = new TestDialoguesXML();
		test.Start();*/
		for (int i = 0; i < buttonGroup.childCount; i++)
		{
			buttonGroup.GetChild(i).name = i.ToString();
		}
	}
	
	void Update ()
	{
        //Si le dialogue n'a pas �t� update et est actif
        if (!updated && active)
        {
            UpdateTexts();
            return;
		}
        if(active && EventSystem.current.currentSelectedGameObject == null && currentConv[currentPhrase].Options.Count != 0 && lastSelectedGameObject != null )
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
        }
        if(active && currentConv[currentPhrase].Options.Count != 0)
        {
            lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        }		
	}

    public IEnumerator ParseText(Phrase phrase)
    {
        string tempLine = phrase.Texte;

        #region Arrays
        int startArray = tempLine.IndexOf("[array:");
        int endStartArray = -1;
        if (startArray != -1)
            endStartArray = tempLine.IndexOf("]", startArray) + 1;
        int endArray = tempLine.IndexOf("[endarray]");

        while ((startArray != -1 && endStartArray != -1 && endArray != -1))
        {
            string str = tempLine.Substring(startArray + 7, endStartArray - startArray - 8);
            #region Check AvisEquipage
            if (Regex.Match(str, "AvisEquipage[0,9]*").Success)
            {
                Debug.Log("found AvisEquipage[0,9]*");
                Dictionary<object, object> avis = FindObjectOfType<SavesManager>().main.getAvisAsDictionnary();
                if (!currentConv[currentPhrase].temp)
                {
                    numberOfLines = int.Parse(tempLine.Substring(startArray + 19, endStartArray - startArray - 20));
                    string rep = tempLine.Substring(endStartArray, endArray - endStartArray);
                    string theEnd = tempLine.Substring(endArray+10);
                    tempLine = tempLine.Remove(startArray);
                    int i = 0, j = 0;
                    foreach (KeyValuePair<object, object> obj in avis)
                    {
                        if (i >= numberOfLines && j * numberOfLines <= avis.Count)
                        {

                            j++;
                            string leReste = "";
                            if ((avis.Count - (j * numberOfLines)) >= numberOfLines)
                            {
                                leReste = "[array:AvisEquipage" + numberOfLines + "]" + rep + "[endarray]";
                            }
                            else if ((avis.Count - (j * numberOfLines)) > 0)
                            {
                                leReste = "[array:AvisEquipage" + (avis.Count - (j * numberOfLines)) + "]" + rep + "[endarray]"+theEnd;
                            }


                            if (leReste != "")
                            {
                                if (currentConv.Count > currentPhrase + j)
                                {
                                    if (currentConv[currentPhrase + j].Texte != leReste)
                                    {
                                        currentConv.Add(new Phrase(currentConv[currentPhrase].Emotion, leReste, true, true));
                                    }
                                }
                                else
                                {
                                    currentConv.Add(new Phrase(currentConv[currentPhrase].Emotion, leReste, true, true));
                                }
                            }
                        }
                        if (i < numberOfLines)
                        {
                            string form = string.Format(rep, obj.Key, obj.Value);
                            tempLine = tempLine.Insert(startArray, form);
                            startArray += form.Length;
                            i++;
                        }
                    }
                    multipleDisplay = 1;
                }
                else
                {
                    int tmp = int.Parse(tempLine.Substring(startArray + 19, endStartArray - startArray - 20));
                    string rep = tempLine.Substring(endStartArray, endArray - endStartArray);
                    tempLine = tempLine.Remove(startArray, endArray - startArray + 10);

                    int i = 0;
                    foreach (KeyValuePair<object, object> obj in avis)
                    {
                        if (i < ((numberOfLines * (multipleDisplay)) + tmp))
                        {
                            if (i >= (numberOfLines * (multipleDisplay)))
                            {
                                string form = string.Format(rep, obj.Key, obj.Value);
                                tempLine = tempLine.Insert(startArray, form);
                                startArray += form.Length;
                            }
                            i++;
                        }
                        else
                        {
                            multipleDisplay++;
                            break;
                        }
                    }
                }
            }
            #endregion
            #region CheckReste
            else
            {
                switch (str)
                {
                    case "AvisEquipage":
                        Debug.Log("found AvisEquipage[0,9]*");
                        Dictionary<object, object> avis = FindObjectOfType<SavesManager>().main.getAvisAsDictionnary();
                        string rep = tempLine.Substring(endStartArray, endArray - endStartArray);
                        tempLine = tempLine.Remove(startArray, endArray - startArray + 10);
                        foreach (KeyValuePair<object, object> obj in avis)
                        {
                            string form = string.Format(rep, obj.Key, obj.Value);
                            tempLine = tempLine.Insert(startArray, form);
                        }
                        break;
                    default:
                        tempLine = tempLine.Replace(tempLine.Substring(startArray, endStartArray - startArray), "Erreur de balise array dans le fichier texte.(" + tempLine.Substring(startArray + 5, endStartArray - startArray - 7) + ")");
                        break;
                }
            }
            #endregion
            if (endArray >= tempLine.Length)
                endArray = tempLine.Length - 1;
            
            startArray = tempLine.IndexOf("[array:", endArray);
            if (startArray != -1)
                endStartArray = tempLine.IndexOf("]", startArray) + 1;

            endArray = tempLine.IndexOf("[endarray]", endArray);
            endedParsing = tempLine;
            yield return endedParsing;
        }
        #endregion

        #region Gestion Cles
        int startKey = tempLine.IndexOf("[key:");
        int endKey = -1;
        if (startKey != -1)
            endKey = tempLine.IndexOf("]", startKey) + 1;
        while ((startKey != -1 && endKey != -1))
        {
            switch (tempLine.Substring(startKey + 5, endKey - startKey - 6))
            {
                case "NomVaisseau":
                    tempLine = tempLine.Replace(tempLine.Substring(startKey, endKey - startKey), MainGameManager.NOM_VAISSEAU);
                    break;
                case "ObjectifCourant":
                    tempLine = tempLine.Replace(tempLine.Substring(startKey, endKey - startKey), FindObjectOfType<SavesManager>().OBJECTIF_COURANT);
                    break;
                default:
                    tempLine = tempLine.Replace(tempLine.Substring(startKey, endKey - startKey), "Erreur de balise key dans le fichier texte.(" + tempLine.Substring(startKey + 5, endKey - startKey - 6) + ")");
                    break;
            }
            if (endKey >= tempLine.Length)
                endKey = tempLine.Length - 1;

            startKey = tempLine.IndexOf("[key:", endKey);
            if (startKey != -1)
                endKey = tempLine.IndexOf("]", startKey) + 1;
            endedParsing = tempLine;
            yield return endedParsing;
        }
        #endregion

        endedParsing = tempLine;
        yield return endedParsing;
    }

	private IEnumerator TextScroll(Text theText)
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
        //while (endedParsing == "-1") { }

        while (isTyping && !cancelTyping && (letter < endedParsing.Length - 1))
		{
			theText.text += endedParsing[letter];
			letter++;
			yield return new WaitForSecondsRealtime(1/typeSpeed);
		}
		theText.text = endedParsing;
        endedParsing = "-1";
		
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
		if (active)
		{
            StartCoroutine(ParseText(currentConv[currentPhrase]));
			StartCoroutine(TextScroll(display.transform.FindChild("Texte").GetComponent<Text>()));
			updated = true;
		}
	}

	public void StartNewConv(TextAsset fileName,string PNJName)
	{
		var serializer = new XmlSerializer(typeof(Conversation));
		using (var reader = new System.IO.StringReader(fileName.text))
		{
            cheminConversation.Clear();
			cheminConversation.Add(serializer.Deserialize(reader) as Conversation);
            currentConv = cheminConversation[0];
            namePnj = PNJName;
            currentPhrase = 0;
            updated = false;
            Activate(true);
        }
	}

	public bool Activate(bool a, string key)
	{
		if (a)
		{
			active = a;
			display.gameObject.SetActive(active);
			Time.timeScale = 0f;
			return true;
		}
		if (!a && key == "Cancel")
		{
			active = a;
			FindObjectOfType<SavesManager>().main.avis.Find(((e) =>
			{
				return (e.name == namePnj);
			})).avis -= 10;
			FindObjectOfType<DisplayAvisHUD>().toUpdate = true;
			display.gameObject.SetActive(active);
			Time.timeScale = 1f;
			return true;
		}
		return false;
	}

	public void Activate(bool a)
	{
		if (a)
		{
			active = a;
			display.gameObject.SetActive(active);
            Time.timeScale = 0f;
		}
		if (!a)
		{
			active = a;
			display.gameObject.SetActive(active);
			Time.timeScale = 1f;
            foreach (TalkingManager talk in FindObjectsOfType<TalkingManager>())
            {
                talk.Activate(false);
            }
		}
		
	}

	public bool nextStep(string key)
	{
		if (key == "Submit")
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
					if (currentConv[currentPhrase].Options[number].Resultat.Count == 0)
					{
						//Si la reponse nous renvoie au dialogue precedent.
						if (currentConv[currentPhrase].Options[number].back)
						{
							currentConv = cheminConversation[cheminConversation.Count - 1];
							cheminConversation.RemoveAt(cheminConversation.Count - 1);
						}
						else
						{
							//On désactive la fenêtre.
							Activate(false);
						}
					}
					// Sinon
					// \/

					cheminConversation.Add(currentConv);
					// la nouvelle conv est le resultat de la r�ponse selectionn�e
					currentConv = currentConv[currentPhrase].Options[number].Resultat;

					// On reset la phrase de la conv.
					currentPhrase = 0;
				}
				// Si la phrase n'a pas d'options
				else
				{
					// Si la conversation � une phrase suivante.
					if ((currentPhrase + 1) < currentConv.Count)
					{
						//On passe a la phrase suivante
						currentPhrase++;
						updated = false;
					}
					//Sinon
					else
					{
						if (currentConv[currentPhrase].back)
						{
							currentConv = cheminConversation[cheminConversation.Count - 1];
							cheminConversation.RemoveAt(cheminConversation.Count - 1);
							currentPhrase = 0;
						}
						else
						{
							//On desactive la fenetre.
							Activate(false);
						}
					}
				}
				updated = false;
			}
			else if (isTyping && !cancelTyping)
			{
				cancelTyping = true;
			}
			return true;
		}
		return false;
	}
}
