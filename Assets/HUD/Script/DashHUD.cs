using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashHUD : MonoBehaviour
{
    [SerializeField] private RawImage firstDashImage = null;
    [SerializeField] private RawImage secondDashImage = null;
    [SerializeField] private CircleMouvement player = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int nbDashAvailable = player.nbDash - player.usedDash;
        if (nbDashAvailable == 0)
        {
            if (firstDashImage.enabled == true)
                firstDashImage.enabled = false;
            if (secondDashImage.enabled == true)
                secondDashImage.enabled = false;
        }
        else if (nbDashAvailable == 1)
        {
            if (firstDashImage.enabled == false)
                firstDashImage.enabled = true;
            if (secondDashImage.enabled == true)
                secondDashImage.enabled = false;
        }
        if (nbDashAvailable == 2)
        {
            if (firstDashImage.enabled == false)
                firstDashImage.enabled = true;
            if (secondDashImage.enabled == false)
                secondDashImage.enabled = true;
        }
    }
}
