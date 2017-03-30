using UnityEngine;
using System;

public class PlayerWeapon : MonoBehaviour {

	//--------------------------------
	// 1 - Designer variables
	//--------------------------------

	/// <summary>
	/// Prefab du projectile
	/// </summary>
	public Transform shotPrefab;
	public Color color;


	public int shotDamages = 1;


	void Start()
	{
		//anim = GetComponent<Animator>();
	}

	void Update()
	{
	}

	public void Attack(Vector2 dir)
	{
		// Création d'un objet copie du prefab
		var shotTransform = Instantiate(shotPrefab) as Transform;
		shotTransform.GetComponent<SpriteRenderer>().color = color;

		// Position
		shotTransform.position = transform.position;

		// On saisit la m_direction pour le mouvement
		ShotsManager parameters = shotTransform.gameObject.GetComponent<ShotsManager>();
		if (parameters != null)
		{
			//Debug.Log(dir);
			parameters.m_direction = dir;
		}
		var rad = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // In radians
		if (GetComponent<SpriteRenderer>().flipX)
		{
			transform.localRotation = Quaternion.AngleAxis(rad, Vector3.back);
		}
		else
		{
			transform.localRotation = Quaternion.AngleAxis(rad, Vector3.forward);
		}
			
	}
	
}
