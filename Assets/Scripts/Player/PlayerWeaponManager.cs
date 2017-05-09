using UnityEngine;
using System.Collections.Generic;

public class PlayerWeaponManager : MonoBehaviour
{
	public int actualWeapon;
	public List<PlayerWeapon> weapons;
	public bool isActivate;
	public int selected = 0;
	public float projectileSpeed = 1f;
    
	/// <summary>
	/// Temps de rechargement entre deux tirs
	/// </summary>
	public float shootingRate = 0.5f;
	private float shootCooldown;

	void Start()
	{
		shootCooldown = 0f;
		weapons = FindObjectOfType<InventoryManager>().weaponList;
		if (weapons.Count > 0)
		{
			actualWeapon = 0;
		}
	}

	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
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
        else if (weapons.Count == 1 && GetComponentInChildren<PlayerWeapon>() == null)
        {
            actualWeapon = 0;
            selected = actualWeapon;
            var weaponUsed = Instantiate(weapons[actualWeapon].transform, transform, false);
            weaponUsed.gameObject.SetActive(true);
            weaponUsed.name = "currentWeapon";
        }
	}

	public void changeWeapon(int s)
	{
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
}