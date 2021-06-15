using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int type = 0;
    public Ammunition ammunition = default;
    public bool isAnEnemyPrototype = false;
    public float scoreValue = 0;
    public float speed = 0;
    public float verticalSpeed = 0;
    public bool isDead = false;
    public float cooldownBeforeDestruction = 0;
    public float actualCooldown = 0;

    public float forceEjection = 500;
    public float forceTorque = 500;
    public LadderScript ladder = null;

    public delegate void deathEnemy();
    public event deathEnemy EventBeforeDeath;

    public void OnDestroy()
    {
        this.EventBeforeDeath?.Invoke();
    }
    void Start()
    {
        if (this.isAnEnemyPrototype == true)
        {
            this.isAnEnemyPrototype = false;
            this.gameObject.SetActive(false);
        }

        else
        {
            this.gameObject.SetActive(true);
        }

        actualCooldown = Time.time;
    }
    private void Update()
    {
        if (isDead == true && Time.time - actualCooldown >= cooldownBeforeDestruction)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnFusRoDah(Collider enemy, CharacterController player)
    {
        if (!isDead)
        {
            if (type != 3)
            {
                GetComponent<ReadPath>().enabled = false;
            }

            else
            {
                GetComponent<Move>().enabled = false;
            }

            Rigidbody body = GetComponent<Rigidbody>();

            body.isKinematic = false;
            body.useGravity = true;

            if (type == 2)
            {
                if (GetComponentInChildren<DetectionZone>() != null)
                    GetComponentInChildren<DetectionZone>().gameObject.SetActive(false);
            }

            GetComponentInChildren<ParticleSystem>().Play();
            isDead = true;

            if (player != null)
            {
                body.AddForce((enemy.transform.position - player.transform.position) * forceEjection);
                body.AddTorque((enemy.transform.position - player.transform.position) * forceTorque);
                Physics.IgnoreCollision(enemy, player, true);
                Physics.IgnoreCollision(enemy, player.GetComponent<Collider>(), true);
            }

            actualCooldown = Time.time;
        }
    }
}
