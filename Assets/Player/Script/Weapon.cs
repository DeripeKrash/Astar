
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour
{
    public event AttackAnimation.FightEvent EnemyKill;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (!enemy.isDead)
            {
                enemy.OnFusRoDah(collider, this.gameObject.GetComponentInParent<CharacterController>());
                EnemyKill?.Invoke(enemy);

                // Sound
                SoundManager.instance.PlaySingle(SoundAssets.instance.enemyDestroyed, 0.5f);
            }
        }
    }
}
