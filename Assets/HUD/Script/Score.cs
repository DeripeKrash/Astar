using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    //[SerializeField] private float scoreCoeff = 10f;
    [SerializeField] private Text textScore = null;
    [SerializeField] private Combo comboScript = null;
    [SerializeField] private Weapon playerAttack = null;
    [System.NonSerialized] public float score = 0f;
    // Start is called before the first frame update
    void Start()
    {
        playerAttack.EnemyKill += UpdateScore;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(Enemy enemy)
    {
        score += enemy.scoreValue * (comboScript.nbEnemyKilled + 1);
        textScore.text = "Score : " + score;
    }
}
