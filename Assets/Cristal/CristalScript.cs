using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CristalScript : MonoBehaviour
{
    //[SerializeField] private Transform heart = null;
    public float cristalLifeMax = 100;

    [System.NonSerialized] public float cristalLife = 100;
    [SerializeField] private Material highLife = null;
    [SerializeField] private Material midLife = null;
    [SerializeField] private Material lowLife = null;
    private MeshRenderer cristalMeshRenderer = null;

    [SerializeField] float horizontalSpeed = 0.5f;
    [SerializeField] float verticalSpeed = 0.5f;


    [SerializeField] float highPosition = 0.3f;
    [SerializeField] float lowPosition = 0.1f;
    Vector3 rotY = new Vector3();
    Vector3 posY = new Vector3();
    bool goUp = false;

    void CristalAnimation()
    {
        if (transform.localPosition.y >= highPosition)
        {
            goUp = false;
        }
        else if (transform.localPosition.y < lowPosition)
        {
            goUp = true;
        }

        if (goUp == true)
        {
            transform.Translate(posY * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Translate(-posY * Time.deltaTime, Space.Self);
        }

        transform.Rotate(rotY * Time.deltaTime);
    }

    void CristalTakeDamage()
    {
        SoundManager.instance.PlaySingle(SoundAssets.instance.cristalTakeDamage, 0.1f);
        cristalLife--;

        float healthPercent = (cristalLife * 100f) / cristalLifeMax;

        if (healthPercent > 50f)
        {
            cristalMeshRenderer.material = highLife;
        }
        else if (healthPercent > 25f)
        {
            cristalMeshRenderer.material = midLife;
        }
        else if (healthPercent > 0f)
        {
            cristalMeshRenderer.material = lowLife;
        }
        else if (healthPercent <= 0f)
        {
            cristalLife = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy.ladder != null)
            {
                enemy.ladder.triggerCount--;
            }

            Destroy(other.gameObject);
            CristalTakeDamage();
        }
    }
    void Start()
    {
        rotY.x = 0;
        rotY.y = horizontalSpeed;
        rotY.z = 0;
        posY.y = verticalSpeed * transform.localScale.y;
        posY.x = 0;
        posY.z = 0;
        goUp = true;
        cristalLife = cristalLifeMax;
        cristalMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CristalAnimation();
    }
}
