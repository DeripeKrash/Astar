using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public bool Used {get; set;}

    // Update is called once per frame
    void Update()
    { 
        if (Used)
            Destroy(gameObject);
    }
}
