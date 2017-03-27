using UnityEngine;


public class InputManager : MonoBehaviour
{
    private CharacterManager m_Character;
    private bool m_Jump;
    private GameObject m_pnj;
    private int directionx = 0;
    private int directiony = 0;
    public bool canMove;

    private void Awake()
    {
        m_Character = GetComponent<CharacterManager>();
    }

    private void Update()
	{
		// On reset les variables pour lire l'input suivante.
		directionx = 0;
		directiony = 0;
		m_Jump = false;
		//Si le joueur ne doit pas interagir, on bloque toutes lecture d'input.
		if (!canMove)
        {
            m_Character.Move(0, false, false);
            return;
        }

		//Lecture input pour jump(ESPACE)
		if (m_Character.isGrounded() && !transform.FindChild("button").gameObject.activeSelf)
		{
			m_Jump = (Input.GetButtonDown("Jump"));
		}

		

		if (Input.GetButtonDown("NextWeapon"))
		{
			m_Character.ChangeWeapon();
		}
		
        //Lecture input pour s'accroupir (CTRL Gauche)
        bool crouch = false;//Input.GetKey(KeyCode.LeftControl);

        //Lecture input pour marcher (Lecture de la direction sur l'axe horizontal(Q ou D)
        float h = Input.GetAxis("Horizontal");

        // On envoie tous les parametres lus precedement au script de controle du personnage.
        m_Character.Move(h, crouch, m_Jump);

		// Ouverture du menu de selection rapide des armes (qui passe la lecture de la direction des tirs).

		if (m_Character.wheel())
		{
			return;
		}
		

		//Lecture inputs pour tirs (Fleches directionelles)
		if (Input.GetAxis("VerticalFire") > 0)
		{
			directiony = 1;
		}
		else if (Input.GetAxis("VerticalFire") < 0)
		{
			directiony = -1;
		}
		if (Input.GetAxis("HorizontalFire") > 0)
		{
			directionx = 1;
		}
		else if (Input.GetAxis("HorizontalFire") < 0)
		{
			directionx = -1;
		}
		if ((directionx != 0 || directiony != 0))
		{
			m_Character.Fire(Input.GetAxis("HorizontalFire"), Input.GetAxis("VerticalFire"));
		}
    }
}
