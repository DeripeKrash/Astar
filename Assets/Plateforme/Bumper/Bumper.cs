using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Bumper : MonoBehaviour
{
    [SerializeField] private Vector2 direction = new Vector2();
    [SerializeField] bool keepSpeed = true;
    [SerializeField] float duration = 1;
    [SerializeField] float minimalFactor = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        if (direction == Vector2.zero)
            Debug.Log("DIRECTION == ZERO");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<CircleMouvement>().Bumper(direction, duration, minimalFactor, keepSpeed);
        }
    }
}