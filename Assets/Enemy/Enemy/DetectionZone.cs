using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private float cooldown = 0;
    private float actualCooldown = 0;
    private Enemy enemy = null;

    private void Start()
    {
        enemy = this.GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - actualCooldown >= cooldown && enemy != null)
        {
            enemy.ammunition.transform.position = enemy.transform.position;
            Ammunition ammunition = Instantiate(enemy.ammunition);
            ammunition.playerPos = other.transform.position;
            ammunition.transform.position = transform.position;
            ammunition.gameObject.SetActive(true);
            actualCooldown = Time.time;
        }
    }
}

