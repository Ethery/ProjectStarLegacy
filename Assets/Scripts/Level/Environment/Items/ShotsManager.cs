using UnityEngine;
using System;

/// <summary>
/// Comportement des tirs
/// </summary>
public class ShotsManager : MonoBehaviour
{
    public string[] transparentTags;
    
    public int damage;

    public float m_speed = 1;

    public GameObject m_target;

    public Vector2 m_direction;

    public double rotation = 0f;

    public bool isTracking = false;

    private Vector2 m_movement;

    public GameObject particules;


    void Start()
    {
        Destroy(gameObject, 10);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<HealthBar>() != null)
        {
            other.GetComponent<HealthBar>().setDamages(damage);
        }
        if(other.GetComponent<EnemyLifeManager>() != null)
        {
            other.GetComponent<EnemyLifeManager>().takeDamage(damage);
        }
        explode();
    }

    private void explode()
    {
        GameObject effet = (GameObject)Instantiate(particules, transform.position, Quaternion.identity);
        Destroy(effet, 1f);
        Destroy(gameObject);
    }

    public void setDamage(int domm)
    {
        damage = domm;
    }
    
    //-------------------------------------------------------------------------------------------
        
    private void Update()
    {
        if (isTracking && m_target != null)
        {
            updateDirection();
        }
        var rad = Math.Atan2(m_direction.y, m_direction.x); // In radians
        var angle = rad * (180 / Math.PI);

        float currentAngle = gameObject.GetComponent<Transform>().rotation.eulerAngles.z;

        if ((float)angle - currentAngle != 0)
        {
            transform.Rotate(0, 0, (float)angle - currentAngle);
        }

        // 2 - Calcul du mouvement
        m_movement = m_direction * m_speed;
    }

    private void FixedUpdate()
    {
        // Application du mouvement
        GetComponent<Rigidbody2D>().velocity = m_movement;
    }

    private void updateDirection()
    {
		m_direction = (m_target.transform.position - transform.position).normalized;
    }

    public float GetM_speed()
    {
        return m_speed;
    }

    public void SetM_speed(float newM_speed)
    {
        m_speed = newM_speed;
    }
}