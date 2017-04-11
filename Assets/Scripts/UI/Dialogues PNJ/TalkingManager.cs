using UnityEngine;
using System;
using System.Xml.Serialization;

public class TalkingManager : MonoBehaviour {
    
	public TextAsset textFile;
    public bool HasAvis;

    public AvisPNJ avis;
	private bool ranged,canTalk;
    
	private void Update ()
	{
		if (!FindObjectOfType<TextConvManager>().isActive && ranged && Input.GetButtonDown("Submit"))
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
                    if(FindObjectOfType<DisplayAvisHUD>()!= null)
                    {
                        FindObjectOfType<DisplayAvisHUD>().toUpdate = true;
                    }
                }
            }
            FindObjectOfType<TextConvManager>().StartNewConv(textFile,gameObject.name);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ranged = true;
            FindObjectOfType<PlayerInputManager>().canUse = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ranged = false;
            FindObjectOfType<PlayerInputManager>().canUse = false;
        }
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
