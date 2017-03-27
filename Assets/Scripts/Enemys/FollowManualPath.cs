using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FollowManualPath : MonoBehaviour {

    public ManualPath path;
    public float speed = 5f;
    public int currentPoint = 0;
    
    public bool MoveAsForce;
    public bool alleretour;

	// Use this for initialization
	void Start () {
        if (path != null)
        {
            transform.position = path.pathObj[currentPoint].position;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (path != null)
        {
            float dist = Vector3.Distance(path.pathObj[currentPoint].position, transform.position);
            //var rotation = Quaternion.LookRotation(path.pathObj[currentPoint].position, transform.position);

            if (MoveAsForce)
            {
                transform.position = Vector3.Lerp(transform.position, path.pathObj[currentPoint].position, Time.fixedDeltaTime * speed);
                if (dist <= 0.2)
                {
                    currentPoint++;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, path.pathObj[currentPoint].position, Time.fixedDeltaTime * speed);
                if (dist == 0)
                {
                    currentPoint++;
                }
            }

            if (currentPoint >= path.pathObj.Count)
            {
                currentPoint = 0;
            }
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.transform.parent = transform;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        coll.transform.parent = null;
    }
}
