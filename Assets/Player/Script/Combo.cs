using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    public float maxComboTime = 0f;
    [SerializeField] private float additionalSpeed = 0f;
    [SerializeField] private Weapon playerAttack = null;

    [System.NonSerialized] public float timer = 0f;
    [System.NonSerialized] public uint nbEnemyKilled = 0;
    [System.NonSerialized] public bool isComboActive = false;

    private CircleMouvement playerMovement = null;

    // Start is called before the first frame update
    void Start()
    {
        playerAttack.EnemyKill += ManageCombo;
        playerMovement = GetComponent<CircleMouvement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isComboActive)
            timer += Time.deltaTime;

        if (timer >= maxComboTime)
        {
            nbEnemyKilled = 0;
            timer = 0f;
            isComboActive = false;
        }
    }

    public void ManageCombo(Enemy enemy)
    {
        if (!isComboActive)
            isComboActive = true;

        nbEnemyKilled++;
        timer = 0f;

        if (nbEnemyKilled == 2)
        {
            playerMovement.SpeedFactor(new Vector2(additionalSpeed, additionalSpeed), maxComboTime);
        }
    }
}
