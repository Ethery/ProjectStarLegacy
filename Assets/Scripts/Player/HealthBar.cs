using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;


public class HealthBar : MonoBehaviour{

	public GameObject healthBar;
	private Image jauge;
	private Text txtHealth;
	private bool correctBar;
	
	public Color colorHightHpLevel;
	public Color colorMidHpLevel;
	public Color colorLowHpLevel;

    public FluctuationLife dialogue;
    public static Color healColor = Color.green;
	public static Color damagesColor = Color.red;
    
	public Health health;

	public float damage;
	
    void Start () {
		if (healthBar != null)
		{
			jauge = healthBar.transform.FindChild("Vie").GetComponent<Image>();
			txtHealth = healthBar.transform.FindChild("Text").GetComponent<Text>();
			if (jauge == null || txtHealth == null)
			{
				Debug.Log("airBar n'a pas le bon format, le Texte doit s'appeller 'Text' et la jauge 'Air'");
				correctBar = false;
			}
			correctBar = true;
		}
		else
		{
			Debug.Log("healthBar non renseignée");
			correctBar = false;
		}
		health.actualPv = health.totalPv;
		updateBar();
	}

    public void updateBar()
    {
        if (health.actualPv <= 0)
		{
			FindObjectOfType<Fading>().FadeTo(SceneManager.GetActiveScene().name);
			return;
		}

		if (correctBar)
        {
            if (health.getPourcentagePv() >= 0.5f)
            {
                jauge.color = colorHightHpLevel;
            }
            else if (health.getPourcentagePv() < 0.5f && health.getPourcentagePv() >= 0.15f)
            {
				jauge.color = colorMidHpLevel;
            }
            else
            {
				jauge.color = colorLowHpLevel;
            }

			jauge.fillAmount = health.getPourcentagePv();
            txtHealth.text = health.actualPv + "/" + health.totalPv;
		}
    }

    public void setDamages(float damage)
    {
		if (GetComponent<EffectsManager>() != null)
		{
			if (!GetComponent<EffectsManager>().effets[GetComponent<EffectsManager>().Invincible].actif)
            {
                health.actualPv -= damage;

				afficherFluctuationLife(dialogue,transform,false, damage);
			}
		}
		else
		{
			health.actualPv -= damage;
			afficherFluctuationLife(dialogue,transform,false, damage);
		}
		
		updateBar();
	}

   

    public void setHeal(float pvHeal)
    {
		health.actualPv += pvHeal;

        afficherFluctuationLife(dialogue, transform, true, pvHeal);

        if (health.actualPv > health.totalPv)
        {
			health.actualPv = health.totalPv;
        }

        updateBar();

    }

    public void upgradeTotalHp()
    {
		health.totalPv += 3;
		health.actualPv = health.totalPv;
        updateBar();
    }
	
    public static void afficherFluctuationLife(FluctuationLife dialogue,Transform parent, bool isHeal, float value)
    {
        if (dialogue != null)
        {

            // Création d'un objet copie du prefab
            FluctuationLife dialoguef = Instantiate(dialogue,parent,false);
            Debug.Log("fluct");

            if (isHeal)
            {
				dialoguef.color = healColor;
				dialoguef.txt = "+" + value;
            }
            else
            {
				dialoguef.color = damagesColor;
				dialoguef.txt = "-" + value;
            }
        }
    }

	public void save(string fileName)
	{
		health.save(fileName);
	}

	public Health save()
	{
		return health;
	}

	public void load(string fileName)
	{
		health = Health.load(fileName);
		updateBar();
	}

	public void load(Health n_health)
	{
		health = n_health;
		updateBar();
	}
}

[Serializable]
[XmlRoot("PlayerHealth")]
public class Health {

	[SerializeField]
	[XmlAttribute("total")]
	public float totalPv;

	[SerializeField]
	[XmlAttribute("actual")]
	public float actualPv;

	public Health()
	{
		totalPv = 1;
		actualPv = 1;
	}

	public float getPourcentagePv()
	{
		return (actualPv / totalPv);
	}

	public void save(string fileName)
	{
		var serializer = new XmlSerializer(typeof(Health));
		var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.OpenOrCreate);
		if (stream != null)
		{
			serializer.Serialize(stream, this);
			stream.Close();
		}
	}

	public static Health load(string fileName)
	{
		var serializer = new XmlSerializer(typeof(Health));
		var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Open);
		if (stream != null)
		{
			var container = serializer.Deserialize(stream) as Health;
			stream.Close();
			return container;
		}
		return null;
	}
}
