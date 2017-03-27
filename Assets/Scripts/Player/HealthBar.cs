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
	public Color healColor;
	public Color damagesColor;

	[SerializeField]
	public Health health;

	public float damage;
	
    void Start () {
		//health = new Health();
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
			correctBar = false;
		health.ActualPv = health.TotalPv;
		updateBar();
	}

    public void updateBar()
    {
		if (health.ActualPv <= 0)
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
            txtHealth.text = health.ActualPv + "/" + health.TotalPv;
        }
    }

    public void setDamages(float damage)
    {
		if (GetComponent<EffectsManager>() != null)
		{
			if (!GetComponent<EffectsManager>().effets[GetComponent<EffectsManager>().Invincible].actif)
			{
				health.ActualPv -= damage;

				afficherFluctuationLife(false, damage);
			}
		}
		else
		{
			Debug.Log(gameObject.name + " has taken"+damage+" damages. But no EffectsManager active");
			health.ActualPv -= damage;

			afficherFluctuationLife(false, damage);
		}
		
		updateBar();
	}

   

    public void setHeal(float pvHeal)
    {
		health.ActualPv += pvHeal;

        afficherFluctuationLife(true, pvHeal);

        if (health.ActualPv > health.TotalPv)
        {
			health.ActualPv = health.TotalPv;
        }

        updateBar();

    }

    public void upgradeTotalHp()
    {
		health.TotalPv += 3;
		health.ActualPv = health.TotalPv;
        updateBar();
    }
	
    public void afficherFluctuationLife(bool isHeal, float value)
    {
        if (dialogue != null)
        {
            // Création d'un objet copie du prefab
            var dialoguef = Instantiate(dialogue) as FluctuationLife;
			dialoguef.transform.SetParent(transform);

			dialoguef.transform.position = transform.position;

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
	private float totalPv;
	public float TotalPv
	{
		get
		{
			return totalPv;
		}

		set
		{
			totalPv = value;
		}
	}

	[SerializeField]
	[XmlAttribute("actual")]
	private float actualPv;
	public float ActualPv
	{
		get
		{
			return actualPv;
		}

		set
		{
			actualPv = value;
		}
	}

	public Health()
	{
		totalPv = 1;
		actualPv = 1;
	}

	public float getPourcentagePv()
	{
		return (ActualPv / TotalPv);
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