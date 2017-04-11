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
	public Conversation() : base()
	{
        this.Clear();
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
    
    [XmlElement("Retour")]
    public bool back;
    [XmlIgnore]
    public bool temp;

    public Phrase()
	{
		Emotion = "Error";
		Texte = "Error";
		Options = new List<Reponse>();
        temp = false;
	}

    public Phrase(string emotion, string txt)
    {
        Emotion = emotion;
        Texte = txt;
        Options = new List<Reponse>();
    }
    public Phrase(string emotion, string txt,bool tmp,bool b)
    {
        Emotion = emotion;
        Texte = txt;
        Options = new List<Reponse>();
        temp = tmp;
        back = b;
    }

    public Phrase(string emotion, string txt, List<Reponse> opts)
	{
		Emotion = emotion;
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
    [XmlElement("Retour")]
    public bool back;

    public Reponse()
    {
        Text = "Error";
        Resultat = new Conversation();
        back = false;
    }

    public Reponse(string txt, Conversation p,bool a)
    {
        Text = txt;
        Resultat = p;
        back = a;
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
        repOui[0].Options.Add(new Reponse("BLEH", repNon,true));

        List<Reponse> reps = new List<Reponse>();
		reps.Add(new Reponse("oui", repOui,true));
		reps.Add(new Reponse("non", repNon,false));


		Conversation conv = new Conversation();
		conv.Add(new Phrase("Content", "Bonjour"));
		conv.Add(new Phrase("Heureux", "CA VA ?", reps));

		var serializer = new XmlSerializer(typeof(Conversation));
		var stream = new FileStream(Application.dataPath + "/ExempleConversation.xml", FileMode.Create);
		
		serializer.Serialize(stream, conv);
		stream.Close();
		
	}
}
