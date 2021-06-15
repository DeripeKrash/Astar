using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text lifeText = null;
    [SerializeField] private CristalScript crystalStats = null;
    static public float score = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (crystalStats == null)
        {
            Debug.LogError("crystalStats is null");
            return;
        }

        if (lifeText != null)
        {
            if (lifeText.text != null)
            {
                lifeText.text = crystalStats.cristalLife.ToString() + '/' + crystalStats.cristalLifeMax;
            }
        }
    }
}
