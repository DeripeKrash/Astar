using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class AttackAnimation : MonoBehaviour
{
    float startAttack = 0f;

    //public event SoundManager.SoundEvent AttackEvent;

    public delegate void FightEvent(Enemy enemy);

    [SerializeField] float AttackDuration = 1;
    [SerializeField] float slowPlayerDuration = 0.5f;
    [SerializeField] Vector2 slowPlayerFactor = new Vector2();
    [SerializeField] int attackToSlow = 1;

    [SerializeField] bool dashOnKill = true;
    [SerializeField] bool jumpOnKill = false;
    
    int attackSlowed = 0;
    float angle = 0;

    public CircleMouvement player = null;
    [SerializeField] Weapon weapon = null;

    bool playerIsGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = transform.parent.gameObject.GetComponent<CircleMouvement>();
        }

        weapon.EnemyKill += EnemyKilled;
        weapon.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            attackSlowed = 0;
        }

        // Calculate attack angle 

        if (PlayerInputManager.AttackInputDown() && !player.isStuned && startAttack == 0)
        {
            if (player != null)
            {
                if (attackSlowed < attackToSlow)
                {
                    attackSlowed++;
                    playerIsGrounded = player.SpeedFactor(slowPlayerFactor, slowPlayerDuration, true);
                }
            }

            weapon.gameObject.SetActive(true);

            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


            if (axis.magnitude < 0.2)
                axis = Vector2.zero;
            else
                axis.Normalize();

            if (axis == Vector2.zero)
                angle = -1;
            else
                angle = Vector2.Angle(axis, Vector2.up);

            // start Coroutine
  
            if (angle <= 22.25 && angle > 0)
            {
                StartCoroutine(RotateObject(0, 1, AttackDuration));
            }
            else if (angle <= 67.25 && angle > 0)
            {
                StartCoroutine(RotateObject(45, 1, AttackDuration));
            }
            else if (angle <= 112.25)
            {
                StartCoroutine(RotateObject(90, 1, AttackDuration));
            }
            else if (angle <= 157.25)
            {
                StartCoroutine(RotateObject(135, 1, AttackDuration));
            }
            else if (angle <= 202.25 && !playerIsGrounded)
            {
                StartCoroutine(RotateObject(180, 1, AttackDuration));
            }

            startAttack = Time.realtimeSinceStartup;

            // Sound
            SoundManager.instance.PlaySingle(SoundAssets.instance.playerAttack, 1.5f);
        }
        else if (Time.realtimeSinceStartup - startAttack > AttackDuration && startAttack != 0)
        {
            weapon.gameObject.SetActive(false);

            startAttack = 0f;
        }
    }

    IEnumerator RotateObject(float startAngle, float angle, float duration)
    {
        float savedTime = Time.realtimeSinceStartup;

        transform.Rotate(startAngle, 0, 0);

        for (float i = 0; i < angle; )
        {
            float tempTime = Time.realtimeSinceStartup - savedTime;
            savedTime = Time.realtimeSinceStartup;
            float useableAngle = angle / (duration / tempTime);
            transform.Rotate(useableAngle, 0, 0);
            i += useableAngle;
            yield return null;
        }
        transform.rotation = transform.parent.rotation;
    }

    void EnemyKilled(Enemy enemy)
    {
        if (dashOnKill)
            player.AddDash(1, 1);
        
        if (jumpOnKill)
            player.AddJump(1, 1);
    }
}