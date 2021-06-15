using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private CircleMouvement player = null;
    private Enemy enemy = null;

    private void Start()
    {
        enemy = this.GetComponent<Enemy>();
    }
    void Update()
    {
        if (player != null)
        {
            Vector3 playerDistanceFromCenter = new Vector3(player.transform.position.x - player.origin.transform.position.x, 0, player.transform.position.z - player.origin.transform.position.z);
            Vector3 enemyDistanceFromCenter = new Vector3(transform.position.x - player.origin.transform.position.x, 0, transform.position.z - player.origin.transform.position.z);

            float speed = enemy.speed;
            float lateralSpeed = enemy.verticalSpeed;

            float tempX1 = (enemyDistanceFromCenter.x * Mathf.Cos(1 * Time.deltaTime)) - (enemyDistanceFromCenter.z * Mathf.Sin(1 * Time.deltaTime));
            float tempZ1 = (enemyDistanceFromCenter.z * Mathf.Cos(1 * Time.deltaTime)) + (enemyDistanceFromCenter.x * Mathf.Sin(1 * Time.deltaTime));
            Vector3 tempVec1 = new Vector3(tempX1, enemyDistanceFromCenter.y, tempZ1);

            float tempX2 = (enemyDistanceFromCenter.x * Mathf.Cos(-1 * Time.deltaTime)) - (enemyDistanceFromCenter.z * Mathf.Sin(-1 * Time.deltaTime));
            float tempZ2 = (enemyDistanceFromCenter.z * Mathf.Cos(-1 * Time.deltaTime)) + (enemyDistanceFromCenter.x * Mathf.Sin(-1 * Time.deltaTime));
            Vector3 tempVec2 = new Vector3(tempX2, enemyDistanceFromCenter.y, tempZ2);

            Vector3 tempVec;

            if ((tempVec1 - playerDistanceFromCenter).magnitude <= (tempVec2 - playerDistanceFromCenter).magnitude)
            {
                tempVec = tempVec1;
            }

            else
            {
                tempVec = tempVec2;
            }

            tempVec.Normalize();
            tempVec *= playerDistanceFromCenter.magnitude;

            Vector3 direction = tempVec - enemyDistanceFromCenter;

            if (Mathf.Abs(player.transform.position.y - transform.position.y) >= 0.5)
            {
                Vector3 lateralMovement = new Vector3(0, player.transform.position.y - transform.position.y, 0);
                lateralMovement.Normalize();
                direction += lateralMovement * lateralSpeed;
            }

            direction.Normalize();
            direction *= speed * Time.deltaTime;

            Vector3 tempEnemyPos = transform.position;
            Vector3 tempPlayerPos = player.transform.position;
            tempEnemyPos.y = 0;
            tempPlayerPos.y = 0;

            if ((tempEnemyPos - tempPlayerPos).magnitude <= 1.5)
            {
                direction.x = 0;
                direction.z = 0;
            }

            transform.rotation = Quaternion.identity;
            transform.Translate(direction);
            if (direction != Vector3.zero)
                transform.forward = direction;
        }

        else
        {
            Debug.Log("Enemy = Le personnage est nul");
        }
    }
}
