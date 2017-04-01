using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System;


[Serializable]
[XmlRoot("Conversation")]
public class Conversation : List<Phrase>
{
	public Conversation()
	{
		
	}
}

[Serializable]
public class Phrase
{
	[XmlElement("Emotion")]
	public string Emotion;
	[XmlElement("Texte")]
	public string Texte;
	[XmlArray("Reponses")]
	[XmlArrayItem("Reponse")]
	public List<Reponse> Options;

	public Phrase()
	{
		Emotion = "Error";
		Texte = "Error";
		Options = new List<Reponse>();
	}

	public Phrase(string em, string txt)
	{
		Emotion = em;
		Texte = txt;
		Options = new List<Reponse>();
	}

	public Phrase(string em, string txt, List<Reponse> opts)
	{
		Emotion = em;
		Texte = txt;
		Options = opts;
	}
}

[Serializable]
public class Reponse
{
	[XmlElement("TexteReponse")]
	public string Text;
	[XmlElement("Phrase")]
	public Conversation Resultat;

	public Reponse()
	{
		Text = "Error";
		Resultat = new Conversation();
	}

	public Reponse(string txt, Conversation p)
	{
		Text = txt;
		Resultat = p;
	}
}



public class TestDialoguesXML
{
	[SerializeField]
	public Conversation Conv = new Conversation();

	// Use this for initialization
	public void Start()
	{
		WriteExempleFile();
		//load("ExempleConversation.xml");
	}
	
	public void load(string path)
	{
		var serializer = new XmlSerializer(typeof(Conversation));
		var stream = new FileStream(path, FileMode.Open);
		Conv = serializer.Deserialize(stream) as Conversation;
		stream.Close();
	}


	public static void WriteExempleFile()
	{
		Conversation repOui = new Conversation();
		repOui.Add(new Phrase("Enerve", "Tant mieux pour vous"));
		Conversation repNon = new Conversation();
		repNon.Add(new Phrase("Triste", "Dommage"));

		List<Reponse> reps = new List<Reponse>();
		reps.Add(new Reponse("oui", repOui));
		reps.Add(new Reponse("non", repNon));


		Conversation conv = new Conversation();
		conv.Add(new Phrase("Content", "Bonjour"));
		conv.Add(new Phrase("Heureux", "CA VA ?", reps));

		var serializer = new XmlSerializer(typeof(Conversation));
		var stream = new FileStream(Application.dataPath + "/ExempleConversation.xml", FileMode.Create);
		
		serializer.Serialize(stream, conv);
		stream.Close();
		
	}
}
