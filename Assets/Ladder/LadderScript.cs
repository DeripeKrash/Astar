using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    public Transform startPoint = null;
    public Transform endPoint = null;

    [SerializeField] private CapsuleCollider arenaCollider = null;
    [SerializeField] private float defaultLife = 0;
    [System.NonSerialized] public float life = 0;

    [SerializeField] private float respawnCooldown = 0;
    [SerializeField] private float onLadderSpeed = 1;
    [SerializeField] private float blinkCooldown = 1;
    [SerializeField] private Material normalMat = null;
    [SerializeField] private Material onLadderMat = null;

    private bool blinked = false;
    private List<MeshRenderer> childRendererList = new List<MeshRenderer>();

    private Vector3 temp = default;
    private float tempP1 = 0;
    private float longueur = 0;
    private Vector3 rotation = default;
    private Vector3 direction = default;
    private float actualCooldown = 0;
    private float actualCooldown2 = 0;
    private bool disable = false;

    [System.NonSerialized] public float triggerCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy.isDead)
                return;

            if (enemy.type != 3)
            {
                other.GetComponent<ReadPath>().enabled = false;
                other.transform.position = startPoint.position;
                enemy.ladder = this;

                if (enemy.type == 2)
                {
                    if (other.GetComponentInChildren<DetectionZone>() != null)
                        other.GetComponentInChildren<DetectionZone>().gameObject.SetActive(false);
                }

                triggerCount++;
            }
        }

        else if (other.CompareTag("Weapon"))
        {
            takeDamage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (life <= 0)
            {
                if (arenaCollider != null)
                {
                    if (enemy.type != 3)
                    {
                        enemy.OnFusRoDah(other, null);
                        Physics.IgnoreCollision(other, arenaCollider, true);
                    }
                }
            }

            else
            {
                if (enemy.type != 3)
                {
                    Vector3 newDirection = direction * onLadderSpeed * Time.deltaTime;
                    if (enemy.type == 2)
                    {
                        enemy.transform.forward = newDirection;
                        enemy.transform.Rotate(new Vector3(0, 90, 0));
                    }

                    else if (newDirection != new Vector3(0, 0, 0))
                    {
                        other.transform.forward = newDirection;
                    }

                    other.transform.Translate(newDirection, Space.World);
                }
            }

            if (Time.time - actualCooldown2 >= blinkCooldown)
            {
                actualCooldown2 = Time.time;

                if (!blinked)
                {
                    foreach (MeshRenderer child in childRendererList)
                    {
                        child.material = onLadderMat;
                    }

                    blinked = true;
                }

                else
                {
                    foreach (MeshRenderer child in childRendererList)
                    {
                        child.material = normalMat;
                    }

                    blinked = false;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy.type != 3)
            {
                triggerCount--;
            }
        }
    }

    private bool isEmpty()
    {
        return triggerCount == 0;
    }
    public void takeDamage()
    {
        if (Time.time - actualCooldown >= respawnCooldown)
        {
            if (startPoint.GetComponentInChildren<ParticleSystem>() != null)
                startPoint.GetComponentInChildren<ParticleSystem>().Play();
            life--;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        this.transform.position = startPoint.position;

        temp = endPoint.position - startPoint.position;

        tempP1 = Mathf.Sqrt((temp.x * temp.x) + (temp.z * temp.z));
        longueur = Mathf.Sqrt((tempP1 * tempP1) + (temp.y * temp.y));

        rotation.x = 0;
        rotation.y = -1 * (Mathf.Atan2(temp.z, temp.x)) * 180 / Mathf.PI;
        rotation.z = (Mathf.Atan2(temp.y, tempP1)) * 180 / Mathf.PI;

        this.transform.localScale = new Vector3(longueur, 1, 1);
        temp /= 2;
        this.transform.position += temp;

        this.transform.Rotate(rotation);

        direction = (endPoint.position - startPoint.position);
        direction.Normalize();
        life = defaultLife;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>())
            {
                childRendererList.Add(child.GetComponent<MeshRenderer>());
            }
        }

        for (int i = 0; i < childRendererList.Count; i++)
        {
            childRendererList[i].material = normalMat;
        }
    }

    private void Update()
    {

        if (life <= 0 && disable == false)
        {
            actualCooldown = Time.time;

            foreach (MeshRenderer child in childRendererList)
            {
                child.enabled = false;
            }

            disable = true;
        }

        else if (Time.time - actualCooldown >= respawnCooldown)
        {
            if (disable)
            {
                disable = false;
                life = defaultLife;

                foreach (MeshRenderer child in childRendererList)
                {
                    child.enabled = true;
                }
            }
        }

        if (isEmpty())
        {
            foreach (MeshRenderer child in childRendererList)
            {
                child.material = normalMat;
            }

            blinked = false;
        }
    }
}
