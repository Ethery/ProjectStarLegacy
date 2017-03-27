using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponManager : MonoBehaviour
{
	public int actualWeapon;
	[SerializeField]
	private List<PlayerWeapon> weapons;
	private GameObject[] weaponsSprites;
	public bool isActivate;
	public int selected = 0;
	private Transform player;
	public Animator roue;
	public float projectileSpeed = 1f;


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
		player = FindObjectOfType<CharacterManager>().transform;
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
		roue.transform.GetChild(0).Find("Selecteur").localRotation = Quaternion.Euler(0,0,selected*-90);
		
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

			if (player.GetComponentInChildren<PlayerWeapon>() != null)
				player.GetComponentInChildren<PlayerWeapon>().Attack(dir);
		}
	}

	public void changeWeapon()
	{
		if (weapons.Count > 1)
		{
			if (player.Find("currentWeapon") != null)
				GameObject.Destroy(player.Find("currentWeapon").gameObject);
			if (actualWeapon + 1 < weapons.Count)
			{
				actualWeapon++;
			}
			else
			{
				actualWeapon = 0;
			}
			selected = actualWeapon;
			var newWeapon = (Transform)Instantiate(weapons[actualWeapon].transform, player, false);
			newWeapon.gameObject.SetActive(true);
			newWeapon.name = "currentWeapon";
		}
	}

	public void changeWeapon(int s)
	{
		Debug.Log(s + " max: " + weapons.Count);
		if (s >= 0 && s < weapons.Count)
		{
			if (player.Find("currentWeapon") != null)
				GameObject.Destroy(player.Find("currentWeapon").gameObject);
			actualWeapon = s;
			var newWeapon = (Transform)Instantiate(weapons[actualWeapon].transform, player, false);
			newWeapon.gameObject.SetActive(true);
			newWeapon.name = "currentWeapon";
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

	public void flipWeapons(bool right)
	{
		if (weapons.Count > 0 )
		{
			weapons[selected].GetComponent<SpriteRenderer>().flipX = !right;
			weapons[selected].GetComponent<Transform>().localPosition = new Vector3(-weapons[selected].GetComponent<Transform>().localPosition.x, weapons[selected].GetComponent<Transform>().localPosition.y, weapons[selected].GetComponent<Transform>().localPosition.z);
		}
	}
}