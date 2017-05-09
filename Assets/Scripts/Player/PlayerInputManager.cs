using UnityEngine;


public class PlayerInputManager : MonoBehaviour
{
    private CharacterManager m_Character;
	private Interactable[] ints;
	private bool m_Jump;
    private GameObject m_pnj;
    private int directionx = 0;
    private int directiony = 0;

    public bool canMove,canUse;

    private void Awake()
    {
        m_Character = GetComponent<CharacterManager>();

		ints = FindObjectsOfType<Interactable>();
		foreach (Interactable i in ints)
		{
			i.Activate(false);
		}
		canUse = true;
    }

    private void Update()
	{

		#region Verification de l'etat necessaire du bouton
		bool found = false;
		
		foreach (Interactable a in ints)
		{
			if (a.isRanged() && a.needRanged())
			{
				//Debug.Log(a.name);
				transform.FindChild("button").gameObject.SetActive(canUse && a.isRanged());
				found = true;
				break;
			}
		}
		if (!found)
		{
			transform.FindChild("button").gameObject.SetActive(false);
		}
		#endregion

		#region Submit

		if (Input.GetButtonDown("Submit"))
		{
			foreach (Interactable a in ints)
			{
				if (!a.isActive() && a.isRanged() && canUse)
				{
					if (a.Activate(true,"Submit"))
					{
                        //Debug.Log(a.name);
                        return;
					}
				}
				if (a.isActive())
				{
					if (a.nextStep("Submit"))
					{
                        return;
					}
					else
					{
						a.Activate(false);
					}
					
				}
			}
		}
		#endregion

		#region Cancel
		if (Input.GetButtonDown("Cancel"))
		{
			foreach (Interactable a in ints)
			{
				if (a.isActive())
				{
					if (a.previousStep("Cancel"))
					{
						return;
					}
					else
					{
						if (a.Activate(false, "Cancel"))
						{
                            return;
						}
					}
				}
			}
		}
        #endregion

        #region Inventaire

        if (Input.GetButtonDown("Inventory"))
        {
            foreach (Interactable a in ints)
            {
                if (!a.isActive() && canUse)
                {
                    if (a.Activate(true, "Inventory"))
                    {
                        //Debug.Log(a.name);
                        return;
                    }
                }
                if (a.isActive())
                {
                    if (a.Activate(false, "Inventory"))
                    {
                        return;
                    }
                    else
                    {
                        a.Activate(false);
                    }

                }
            }
        }
        #endregion

        if (Input.GetButtonDown("Pause"))
        {
            foreach (Interactable a in ints)
            {
                if (!a.isActive())
                {
                    if (a.Activate(true, "Pause"))
                    {
                        Debug.Log("ok:" + a.name);
                        return;
                    }
                }
            }
        }

		if (Input.GetButtonDown("NextWeapon"))
		{
			m_Character.ChangeWeapon();
			return;
		}


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
        if (m_Character.isGrounded()&& canMove)
        {
            m_Jump = (Input.GetButtonDown("Jump"));
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
