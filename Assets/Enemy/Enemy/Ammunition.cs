using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour
{
    public bool isAnAmmunationPrototype = false;
    [SerializeField] private float speed = 0;
    [System.NonSerialized] public Vector3 playerPos = default;
    private float spawnTime = 0;
    [SerializeField] private float lifetime = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (this.isAnAmmunationPrototype == true)
        {
            this.isAnAmmunationPrototype = false;
            this.gameObject.SetActive(false);
        }

        else
        {
            this.gameObject.SetActive(true);
        }
        spawnTime = Time.time;
        transform.forward = (playerPos - transform.position).normalized;
        transform.Translate(transform.forward, Space.World);
    }
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        if (Time.time - spawnTime >= lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
