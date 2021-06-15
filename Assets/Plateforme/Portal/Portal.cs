using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal secondPortal = null;
    [SerializeField] bool BlockOnY = false;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<CircleMouvement>().Teleport(secondPortal.transform.position, BlockOnY);
            if (GetComponentInChildren<ParticleSystem>() != null)
            {
                GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
}