using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredeterminedPath : MonoBehaviour
{
    public List<GameObject> Objectlist = new List<GameObject>();

    public List<Vector3> list = new List<Vector3>();

    public bool children = false;
    public bool suppressPoints = false;
   

    // Start is called before the first frame update
    void Awake()
    {
        PathPoint point;

        if (children)
        {
            foreach (Transform o in GetComponentInChildren<Transform>())
            {
                point = o.gameObject.GetComponent<PathPoint>();

                if (point != null)
                {
                    list.Add(o.position);
                    if (suppressPoints)
                        point.Used = true;
                }
            }
        }
        foreach (GameObject o in Objectlist)
        {
            point = o.gameObject.GetComponent<PathPoint>();

            list.Add(o.transform.position);
            if (suppressPoints && point != null)
            {
                point.Used = true;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}
