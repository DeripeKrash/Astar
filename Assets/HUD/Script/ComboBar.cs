using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboBar : MonoBehaviour
{
    [SerializeField] private Canvas comboCanvas  = null;
    [SerializeField] private Image  comboBar     = null;
    [SerializeField] private Combo  comboScript  = null;
    [SerializeField] private Weapon playerWeapon = null;
    [SerializeField] private Text   comboCoeff   = null;

    private float comboBarWidth = 0f;
    private Vector2 comboBarOffSet = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        comboBarWidth = comboBar.rectTransform.rect.size.x;
        comboBar.rectTransform.offsetMax = comboBar.rectTransform.offsetMax;
        comboBarOffSet = comboBar.rectTransform.offsetMax;
        playerWeapon.EnemyKill += ResetComboBar;
    }

    // Update is called once per frame
    void Update()
    {
        if (comboScript.isComboActive)
        {
            if (!comboCanvas.isActiveAndEnabled)
            {
                comboCanvas.gameObject.SetActive(true);
            }
            comboBar.rectTransform.offsetMax = comboBarOffSet - new Vector2((comboScript.timer * comboBarWidth) / comboScript.maxComboTime, 0f);
            comboCoeff.text = "x" + (1 + comboScript.nbEnemyKilled);
        }
        else if (!comboScript.isComboActive)
        {
            if (comboCanvas.isActiveAndEnabled)
            {
                comboCanvas.gameObject.SetActive(false);
                ResetComboBar(null);
            }
        }
    }

    public void ResetComboBar(Enemy enemy)
    {
        comboBar.rectTransform.offsetMax = comboBarOffSet;
        comboCoeff.text = "x0";
    }
}
