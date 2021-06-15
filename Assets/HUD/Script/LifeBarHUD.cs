using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarHUD : MonoBehaviour
{
    [SerializeField] private Image lifeBar = null;
    [SerializeField] private CristalScript cristal = null;

    private float lifeBarWidth = 0f;
    private Vector2 lifeBarOffSet = Vector2.zero;
    private float previousCristalLife = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lifeBarWidth = lifeBar.rectTransform.rect.size.x;
        lifeBarOffSet = lifeBar.rectTransform.offsetMax;
        previousCristalLife = cristal.cristalLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (cristal.cristalLife != previousCristalLife)
        {
            previousCristalLife = cristal.cristalLife;
            lifeBar.rectTransform.offsetMax = lifeBarOffSet - (new Vector2(lifeBarWidth, 0f) - new Vector2(lifeBarWidth * cristal.cristalLife / cristal.cristalLifeMax, 0f));
        }
    }
}
