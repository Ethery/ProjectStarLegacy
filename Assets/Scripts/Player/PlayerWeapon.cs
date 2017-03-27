using UnityEngine;
using System.Collections;

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
		var rad = Mathf.Atan2(dir.y, dir.x); // In radians
		var angle = rad * (180 / Mathf.PI);

		float currentAngle = gameObject.GetComponent<Transform>().rotation.eulerAngles.z;

		if ((float)angle - currentAngle != 0)
		{
			transform.Rotate(0, 0, (float)angle - currentAngle);
		}
	}
	
}
