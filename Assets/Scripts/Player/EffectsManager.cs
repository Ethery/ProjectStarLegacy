using UnityEngine;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour {
    
    public int nbActifs;

    public Sprite[] sprites;


    private HealthBar healthBar;
    public GameObject effectBar;
    public GameObject effectPrefab;

    public float ticksInterval = 1f;

    [System.Serializable]
    public struct Effet
    {
        public float duree;
        public float timerEffet;
        public float timerDegats;

        public int degatsDeLEffet;

        public bool actif;
        public bool applique;

        public Sprite affichage;
        public GameObject blocUI;

        public Effet(Sprite op)
        {
            duree = 1f;
            actif = false;
            applique = false;
            degatsDeLEffet = 1;
            timerEffet = 0f;
            timerDegats = 0f;
            affichage = op;
            blocUI = null;
        }
    };

    public Effet[] effets;

    public const int
        feu = 0,
        noyade = 1,
        poison = 2,
        invincible = 3,
        aimant = 4,
        jumpUp = 5,
        jumpDown = 6,
        speedUp = 7,
        speedDown = 8,
        renvoiDeBalles = 9;
    
    public int Feu
    {
        get
        {
            return feu;
        }
    }
    public int Noyade
    {
        get
        {
            return noyade;
        }
    }
    public int Poison
    {
        get
        {
            return poison;
        }
    }
    public int Invincible
    {
        get
        {
            return invincible;
        }
    }
    public int Aimant
    {
        get
        {
            return aimant;
        }
    }
    public int JumpUp
    {
        get
        {
            return jumpUp;
        }
    }
    public int JumpDown
    {
        get
        {
            return jumpDown;
        }
    }
    public int SpeedUp
    {
        get
        {
            return speedUp;
        }
    }
    public int SpeedDown
    {
        get
        {
            return speedUp;
        }
    }
    public int RenvoiDeBalles
    {
        get
        {
            return renvoiDeBalles;
        }
    }
    


    // Use this for initialization
    void Start ()
    {
        effets = new Effet[10];
        
        for (int i = 0; i < sprites.Length; i++)
        {
            switch(sprites[i].name)
            {
                case "feu":
                    effets[feu] = new Effet(sprites[i]);
                    break;
                case "noyade":
                    effets[noyade] = new Effet(sprites[i]);
                    break;
                case "poison":
                    effets[poison] = new Effet(sprites[i]);
                    break;
                case "invincible":
                    effets[invincible] = new Effet(sprites[i]);
                    break;
                case "aimant":
                    effets[aimant] = new Effet(sprites[i]);
                    break;
                case "jumpUp":
                    effets[jumpUp] = new Effet(sprites[i]);
                    break;
                case "jumpDown":
                    effets[jumpDown] = new Effet(sprites[i]);
                    break;
                case "speedUp":
                    effets[speedUp] = new Effet(sprites[i]);
                    break;
                case "speedDown":
                    effets[speedDown] = new Effet(sprites[i]);
                    break;
                case "renvoiDeBalles":
                    effets[renvoiDeBalles] = new Effet(sprites[i]);
                    break;
                default:
                    break;
            }

        }
        for (int i = 0; i < effets.Length; i++)
        { 
            effets[i].blocUI = Instantiate(effectPrefab) as GameObject;
            effets[i].blocUI.GetComponent<RectTransform>().SetParent(effectBar.transform,false);
            effets[i].blocUI.GetComponent<Image>().sprite = effets[i].affichage;
            effets[i].blocUI.SetActive(false);
        }

        healthBar = GetComponent<HealthBar>();
        nbActifs = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
		nbActifs = 0;
        for (int i = 0; i < effets.Length; i++)
        {
            //Gestion des timers de tous les effets.
            if (effets[i].applique) //tant que l'effet est appliqué, On le set actif et on reset le timer au max
            {
                effets[i].timerEffet = effets[i].duree;
                effets[i].actif = true;
            }
            else//des que l'effet n'est plus appliqué
            {
                if (effets[i].timerEffet > 0)//on decremente le timer de l'effet et il est toujours actif pendant ce temps.
                {
                    effets[i].timerEffet -= Time.deltaTime;
                }
                if (effets[i].timerEffet <= 0)//des que l'effet arrive a terme, Il n'est plus actif et les degats ne sont plus applicables.
                {
                    effets[i].actif = false;
                    effets[i].timerDegats = 0.0001f;
                    applyEndEffect(i);
                }
            }

            if (effets[i].timerDegats > 0 && effets[i].actif)//Tant que l'effet est actif, on decremente le timer des ticks de degats.
            {
                effets[i].timerDegats -= Time.deltaTime;
            }
            //gestion effets

            effets[i].blocUI.transform.GetChild(0).GetComponent<Image>().fillAmount = effets[i].timerEffet / effets[i].duree;
            if (effets[i].timerDegats <= 0)// On parcours tous les effets et dés que son delais de degats est a terme, on applique son effet et on reset le timer.
            {
                switch (i)
                {
                    case feu:
                        effets[i].timerDegats = ticksInterval;
                        healthBar.setDamages(effets[i].degatsDeLEffet);
                        break;
                    case noyade:
                        effets[i].timerDegats = ticksInterval;
                        if (GetComponent<AirBar>().actualAir > 0)
                        {
                            GetComponent<AirBar>().loseOxygen(effets[i].degatsDeLEffet);
                        }
                        else
                        {
                            healthBar.setDamages(effets[i].degatsDeLEffet);
                        }
                        break;
                    case poison:
                        effets[i].timerDegats = ticksInterval;
                        healthBar.setDamages(effets[i].degatsDeLEffet);
                        break;
                    case invincible:
                        break;
                    case aimant:
                        break;
                    case jumpUp:
                        GetComponent<CharacterManager>().SetJumpForce(GetComponent<CharacterManager>().GetJumpForce() + effets[i].degatsDeLEffet);
                        break;
                    case jumpDown:
                        GetComponent<CharacterManager>().SetJumpForce(GetComponent<CharacterManager>().GetJumpForce() - effets[i].degatsDeLEffet);
                        break;
                    case speedUp:
                        GetComponent<CharacterManager>().SetMaxSpeed(GetComponent<CharacterManager>().GetMaxSpeed() * effets[i].degatsDeLEffet);
                        break;
                    case speedDown:
                        GetComponent<CharacterManager>().SetMaxSpeed(GetComponent<CharacterManager>().GetMaxSpeed() / effets[i].degatsDeLEffet);
                        break;
                    case renvoiDeBalles:
                        break;
                    default:
                        break;
                }
            }
            
            //Decompte des actifs
            if(effets[i].actif)
            {
                nbActifs++;
                effets[i].blocUI.SetActive(true);
				
				effets[i].blocUI.transform.localPosition = new Vector2(-(nbActifs * 64) + 32, 0);
			}
            else if(!effets[i].actif&&effets[i].blocUI.activeSelf)
            {
                effets[i].blocUI.SetActive(false);
                applyEndEffect(i);
            }
        }
    }

    public void activateEffect(int id, float time, int degats)
    {
        effets[id].applique = true;
        effets[id].duree = time;
        effets[id].degatsDeLEffet = degats;
    }

    public void forceDesactivateEffect(int id)
    {
        effets[id].applique = false;
        effets[id].actif = false;
        applyEndEffect(id);
        effets[id].timerDegats = 0.0001f;
        effets[id].timerEffet = 0f;
    }

    public void disableEffect(int id)
    {
        effets[id].applique = false;
    }
    
    public void ableEffect(int id,float time)
    {
		Debug.Log("effect n°" + id + " has been abled for "+time+"s");
        effets[id].actif = true;
        effets[id].duree = time;
        effets[id].timerEffet = time;
    }

    public void applyEndEffect(int id)
    {
        switch (id)
        {
            case feu:
                break;
            case noyade:
                GetComponent<AirBar>().resetAir();
                break;
            case poison:
                break;
            case invincible:
                break;
            case aimant:
                break;
            case jumpUp:
                GetComponent<CharacterManager>().SetJumpForce(GetComponent<CharacterManager>().GetJumpForce() - effets[id].degatsDeLEffet);
                break;
            case jumpDown:
                GetComponent<CharacterManager>().SetJumpForce(GetComponent<CharacterManager>().GetJumpForce() + effets[id].degatsDeLEffet);
                break;
            case speedUp:
                GetComponent<CharacterManager>().SetMaxSpeed(GetComponent<CharacterManager>().GetMaxSpeed() / effets[id].degatsDeLEffet);
                break;
            case speedDown:
                GetComponent<CharacterManager>().SetMaxSpeed(GetComponent<CharacterManager>().GetMaxSpeed() * effets[id].degatsDeLEffet);
                break;
            case renvoiDeBalles:
                break;
            default:
                break;
        }
    }
}
