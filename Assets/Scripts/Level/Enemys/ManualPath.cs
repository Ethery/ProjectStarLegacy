using UnityEngine;
using System.Collections.Generic;

public class ManualPath : MonoBehaviour {

    public Color rayColor = Color.white;
    public List<Transform> pathObj = new List<Transform>();
    Transform[] theArray;
    
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform>();
        pathObj.Clear();
        


        foreach(Transform obj in theArray)
        {
            if(obj != this.transform)
            {
                pathObj.Add(obj);
            }
        }
        for(int i=0;i<pathObj.Count;i++)
        {
            Vector3 position = pathObj[i].position;
            if(i>0)
            {
                Vector3 previous = pathObj[i - 1].position;
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawWireSphere(position, 0.3f);
            }
        }
        if (pathObj.Count > 0)
        {
            Vector3 previous = pathObj[pathObj.Count - 1].position;
            Gizmos.DrawLine(previous, transform.position);
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }

}
