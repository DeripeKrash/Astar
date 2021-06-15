using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SpeedBoost : MonoBehaviour
{
    [SerializeField] private float boostTime  = 0f;
    [SerializeField] private float speedBoost = 0f;

    [Header("Respawn")]
    [SerializeField] private bool canRespawn = false;
    [SerializeField] private float respawnTime = 0f;

    private MeshRenderer meshRenderer = null;
    private bool isSpeedBoost = false;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRespawn && isSpeedBoost)
        {
            if (Time.timeScale != 0)
                timer += Time.unscaledDeltaTime;
            if (timer >= respawnTime)
            {
                timer = 0f;
                meshRenderer.enabled = true;
                isSpeedBoost = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSpeedBoost && other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<CircleMouvement>().SpeedFactor(new Vector2(speedBoost, speedBoost), boostTime);
            meshRenderer.enabled = false;
            isSpeedBoost = true;

            if (!canRespawn)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
