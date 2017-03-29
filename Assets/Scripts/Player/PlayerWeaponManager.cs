using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponManager : MonoBehaviour
{
	public int actualWeapon;
	public List<PlayerWeapon> weapons;
	public GameObject[] weaponsSprites;
	public bool isActivate;
	public int selected = 0;
	public Animator roue;
	public float projectileSpeed = 1f;
	//public Transform weaponUsed;


	//--------------------------------
	// 2 - Rechargement
	//--------------------------------

	/// <summary>
	/// Temps de rechargement entre deux tirs
	/// </summary>
	public float shootingRate = 0.5f;
	private float shootCooldown;

	void Start()
	{
		shootCooldown = 0f;
		weapons = FindObjectOfType<InventoryManager>().weaponList;
		weaponsSprites = GameObject.FindGameObjectsWithTag("weaponSprite");
		if (weapons.Count > 0)
		{
			actualWeapon = 0;
		}
		if (weapons.Count > 0)
		{
			for (int i = 0; i < weaponsSprites.Length; i++)
			{
				if (i < weapons.Count)
				{
					weaponsSprites[i].GetComponent<SpriteRenderer>().sprite = null;
				}
				else
				{
					weaponsSprites[i].GetComponent<SpriteRenderer>().sprite = weapons[i - 2].GetComponent<SpriteRenderer>().sprite;
				}
			}
		}
	}

	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
		//roue.transform.GetChild(0).Find("Selecteur").localRotation = Quaternion.Euler(0,0,selected*-90);
		
		if (Input.GetButton("OpenWeaponsWheel"))
		{
			openWeaponsInt(true);
			if (Input.GetAxisRaw("VerticalFire") > 0)
			{
				selected = 3;
			}
			else if (Input.GetAxisRaw("VerticalFire") < 0)
			{
				selected = 1;
			}
			if (Input.GetAxisRaw("HorizontalFire") > 0)
			{
				selected = 0;
			}
			else if (Input.GetAxisRaw("HorizontalFire") < 0)
			{
				selected = 2;
			}
		}
		else
		{
			openWeaponsInt(false);
		}
	}

	public void Attack(Vector2 dir)
	{
		if (shootCooldown <= 0f)
		{
			shootCooldown = shootingRate;

			if (GetComponentInChildren<PlayerWeapon>() != null)
			{
				GetComponentInChildren<PlayerWeapon>().Attack(dir);
			}
		}
	}

	public void changeWeapon()
	{
		Debug.Log("PENIS");
		if (weapons.Count > 1)
		{
			if (GetComponentInChildren<PlayerWeapon>() != null)
				Destroy(GetComponentInChildren<PlayerWeapon>().gameObject);
			if (actualWeapon + 1 < weapons.Count)
			{
				actualWeapon++;
			}
			else
			{
				actualWeapon = 0;
			}
			selected = actualWeapon;
			var weaponUsed = Instantiate(weapons[actualWeapon].transform, transform, false);
			weaponUsed.gameObject.SetActive(true);
			weaponUsed.name = "currentWeapon";
		}
	}

	public void changeWeapon(int s)
	{
		Debug.Log("PENIS");
		if (s >= 0 && s < weapons.Count)
		{
			if (GetComponentInChildren<PlayerWeapon>() != null)
				Destroy(GetComponentInChildren<PlayerWeapon>().gameObject);
			actualWeapon = s;
			var weaponUsed = Instantiate(weapons[actualWeapon].transform, transform, false);
			weaponUsed.gameObject.SetActive(true);
			weaponUsed.name = "currentWeapon";
		}
	}

	public void openWeaponsInt(bool a)
	{
		roue.SetBool("opened",a);

		if (roue.GetBool("opened") && !isActivate)
		{
			isActivate = true;
			selected = actualWeapon;
			Time.timeScale = 0f;
		}
		else if (!roue.GetBool("opened") && isActivate)
		{
			changeWeapon(selected);
			isActivate = false;
			Time.timeScale = 1f;
		}
	}

	public void flipWeapons(bool right,bool first)
	{
		if (GetComponentInChildren<PlayerWeapon>() != null)
		{
			GetComponentInChildren<PlayerWeapon>().GetComponent<SpriteRenderer>().flipX = !right;
			if (first && right)
			{
				GetComponentInChildren<PlayerWeapon>().transform.localPosition = new Vector3(GetComponentInChildren<PlayerWeapon>().transform.localPosition.x, GetComponentInChildren<PlayerWeapon>().transform.localPosition.y, GetComponentInChildren<PlayerWeapon>().transform.localPosition.z);
			}
			else if(first && !right)
			{
				GetComponentInChildren<PlayerWeapon>().transform.localPosition = new Vector3(-GetComponentInChildren<PlayerWeapon>().transform.localPosition.x, GetComponentInChildren<PlayerWeapon>().transform.localPosition.y, GetComponentInChildren<PlayerWeapon>().transform.localPosition.z);
			}
			else{
				GetComponentInChildren<PlayerWeapon>().transform.localPosition = new Vector3(-GetComponentInChildren<PlayerWeapon>().transform.localPosition.x, GetComponentInChildren<PlayerWeapon>().transform.localPosition.y, GetComponentInChildren<PlayerWeapon>().transform.localPosition.z);
			}
		}
	}
}