using UnityEngine;
using System.Collections;

public class EnemyWeaponManager : MonoBehaviour {
    
    
    public string lockPlayer = "up";
    // Comportement des tirs
    // Direction fixe / up: haut / right: droite / down: bas / left: gauche
    // sniper : Direction sniper
    // missile : Tete chercheuse
    // lobe : Lobe Poison

    public Transform target;
    
    private Vector2 m_direction;

    public Transform shotPrefab;

    public float shootingRate = 1f;

    public int shotDamages = 1;

    private float shootCooldown;

    // Use this for initialization
    void Start ()
    {
        shootCooldown = 0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
        if (shootCooldown <= 0f)
        {
            shootCooldown = shootingRate;
            if (lockPlayer == "up")
            {
                Attack(new Vector2(0, 1));
            }
            else if (lockPlayer == "right")
            {
                Attack(new Vector2(1, 0));
            }
			else if (lockPlayer == "down")
            {
                Attack(new Vector2(0, -1));
            }
			else if (lockPlayer == "left")
            {
                Attack(new Vector2(-1, 0));
            }
            else if (lockPlayer == "sniper")
            {
                tirVersJoueur(false);
            }
            else if (lockPlayer == "missile")
            {
                tirVersJoueur(true);
            }
			else if (lockPlayer == "lobelocked")
			{
				poisonTir(false);
			}
			else if (lockPlayer == "loberandom")
			{
				poisonTir(true);
			}
		}
    }

    public void tirVersJoueur(bool lockJoueur)
    {
		if (target != null)
		{
			Vector3 dir = (target.position - transform.position).normalized;

			if (lockJoueur)
				Attack(dir, target.gameObject);
			else
				Attack(dir);
		}
    }

    public void Attack(Vector2 dir)
    {
        // Création d'un objet copie du prefab
        var shotTransform = Instantiate(shotPrefab) as Transform;

        // Position
        shotTransform.position = transform.position;
        // On saisit la m_direction pour le mouvement
        ShotsManager parameters = shotTransform.gameObject.GetComponent<ShotsManager>();
        if (parameters != null)
        {
			parameters.setDamage(shotDamages);

			parameters.m_target = target.gameObject;
			parameters.isTracking = false;
			parameters.m_direction = dir;
        }
    }

    public void Attack(Vector2 dirShot, GameObject target)
    {
        // Création d'un objet copie du prefab
        var shotTransform = Instantiate(shotPrefab) as Transform;

        // Position
        shotTransform.position = transform.position;

        // Propriétés du script
        ShotsManager parameters = shotTransform.gameObject.GetComponent<ShotsManager>();
        
        if (parameters != null)
        {
			parameters.setDamage(shotDamages);

			parameters.m_target = target;
			parameters.isTracking = true;
			parameters.m_direction = dirShot;
        }
    }

    public void poisonTir(bool random)
    {
		if (!random && target != null)
		{
			Vector3 dir = (target.position - transform.position);
			var shotTransform = Instantiate(shotPrefab) as Transform;
			Debug.Log(dir.x);
			shotTransform.GetComponent<PoisonManager>().X = dir.x*20;
			shotTransform.GetComponent<PoisonManager>().setDamage(shotDamages);

			shotTransform.position = transform.position;
		}
		else if(random)
		{
			var shotTransform = Instantiate(shotPrefab) as Transform;
			shotTransform.GetComponent<PoisonManager>().X = Random.Range(-300,300);
			shotTransform.GetComponent<PoisonManager>().setDamage(shotDamages);

			shotTransform.position = transform.position;
		}
    }

	public void setDamages(int value)
	{
		shotDamages = value;
	}
}
