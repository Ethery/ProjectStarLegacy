using UnityEngine;
using System;
using System.Xml.Serialization;

public class TalkingManager : Interactable {
    
	public TextAsset textFile;
    public bool HasAvis;

    public AvisPNJ avis;

	private void Start()
	{
		active = false;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ranged = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ranged = false;
        }
	}

	public override void Activate(bool a)
	{
        if (!a)
        {
            active = false;
        }
	}

	public override bool Activate(bool a,string key)
	{
		if (a && key == "Submit")
		{
			active = a;
			init();
			return true;
		}
		if(!a && key == "Cancel")
		{
			active = a;
			return FindObjectOfType<TextConvManager>().Activate(false,"Cancel");
		}
		return false;
	}

	public override bool nextStep(string key)
	{
		return FindObjectOfType<TextConvManager>().nextStep(key);
	}

	public override void init()
	{
		if (HasAvis)
		{
			avis = FindObjectOfType<SavesManager>().main.avis.Find(((e) =>
			{
				return (e.name == gameObject.name);
			}));
			if (avis == null)
			{
				avis = new AvisPNJ(gameObject.name, 50);
				FindObjectOfType<SavesManager>().main.avis.Add(avis);
				if (FindObjectOfType<DisplayAvisHUD>() != null)
				{
					FindObjectOfType<DisplayAvisHUD>().toUpdate = true;
				}
			}
		}
		FindObjectOfType<TextConvManager>().StartNewConv(textFile, gameObject.name);
	}
}

public class AvisPNJ
{
    public string name;
    public int avis;

    public AvisPNJ()
    {
        name = null;
        avis = -1;
    }

    public AvisPNJ(string n, int a)
    {
        name = n;

        avis = -1;
        if (name != null && a <= 100 && a >= 0)
        {
            avis = a;
        }
    }
}
