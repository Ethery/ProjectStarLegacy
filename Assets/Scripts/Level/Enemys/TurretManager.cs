using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TurretManager : MonoBehaviour {

    public bool activated;
	
    public GameObject[] targets;

    [SerializeField]
    GameObject target;
	public GameObject canon;

    [SerializeField]
    double targetDirection;

	private Animator m_Anim;

	private void Awake()
	{
		m_Anim = transform.parent.GetComponent<Animator>();
	}

	private void Update()
	{
		if (canon.activeSelf && target != null)
		{
			canon.GetComponent<EnemyWeaponManager>().target = target.transform;
			Vector2 dir = (transform.position - target.transform.position).normalized;
			var rad = System.Math.Atan2(dir.y, dir.x); // In radians
			var angle = rad * (180 / System.Math.PI);
			targetDirection = angle-90;
			if (targetDirection < 40f && targetDirection > -40f)
			{
				canon.transform.localEulerAngles = new Vector3(0f, 0f, (float)targetDirection);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (target == null)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				if (other.gameObject == targets[i])
				{
					target = other.gameObject;
					break;
				}
			}
			m_Anim.SetBool("Activated", true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject == target)
		{
			target = null;
			m_Anim.SetBool("Activated", false);
		}
	}
}
