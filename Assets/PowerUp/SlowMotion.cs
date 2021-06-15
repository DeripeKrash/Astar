using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SlowMotion : MonoBehaviour
{
    [SerializeField] private float slowMotionDuration = 0f;
    [SerializeField] private float slowMotionFactor   = 1f;

    [Header("Respawn")]
    [SerializeField] private bool canRespawn = false;
    [SerializeField] private float respawnTime = 0f;

    private MeshRenderer meshRenderer = null;
    private float timer = 0f;
    private bool isSlowed = false;

    private bool isEndSlow = false;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (canRespawn && slowMotionDuration > respawnTime)
        {
            Debug.LogError("slowMotionDuration > respawnTime");
        }
    }

    void Update()
    {
        if (isSlowed)
        {
            if (Time.timeScale != 0)
                timer += Time.unscaledDeltaTime;

            if (!isEndSlow && timer >= slowMotionDuration)
            {
                 Time.timeScale = 1f;
                 if (!canRespawn)
                     Destroy(this.gameObject);
                 isEndSlow = true;
            }
            if (canRespawn && timer >= respawnTime)
            {
                isSlowed = false;
                meshRenderer.enabled = true;
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isSlowed && other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = slowMotionFactor;
            isSlowed       = true;
            meshRenderer.enabled = false;
            isEndSlow = false;
        }
    }
}
