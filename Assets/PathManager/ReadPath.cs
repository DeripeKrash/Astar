using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadPath : MonoBehaviour
{
    public PredeterminedPath pred;

    public List<Vector3> PersonalList = new List<Vector3>();

    private int indice = 0;

    private float speed = 1;
    public float range = 1;

    public bool Repeat = true;

    private bool stop = false;

    public bool follow = true;

    // Start is called before the first frame update
    void Start()
    {
        if (pred == null)
        {
            Debug.LogWarning("Path not find");
            return;
        }

        foreach (Vector3 v in pred.list)
        {
            PersonalList.Add(v);
        }

        speed = this.GetComponent<Enemy>().speed;
    }

    public void Read(PredeterminedPath _pred)
    {
        indice = 0;
        stop = false;
        pred = _pred;

        PersonalList.Clear();

        if (_pred == null)
        {
            Debug.LogWarning("Path not find");
            return;
        }

        foreach (Vector3 v in _pred.list)
        {
            PersonalList.Add(v);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!follow || stop || PersonalList.Count <= 0)
            return;

        if ((PersonalList[indice] - transform.position).sqrMagnitude < range * range)
        {
            indice++;
            if (indice == PersonalList.Count)
            {
                if (Repeat)
                    indice = 0;
                else
                    stop = true;
            }
        }
        else
        {
            if (transform.gameObject.GetComponent<Enemy>().type == 2)
            {
                transform.right = -1 * (PersonalList[indice] - transform.position).normalized * speed * Time.deltaTime;
            }

            else
            {
                if (((PersonalList[indice] - transform.position).normalized * speed * Time.deltaTime) != new Vector3(0,0,0))
                {
                    transform.forward = (PersonalList[indice] - transform.position).normalized * speed * Time.deltaTime;
                }
            }

            transform.Translate((PersonalList[indice] - transform.position).normalized * speed * Time.deltaTime, Space.World);
        }
    }

    public void MovePath(Vector3 mouvement)
    {
        for (int i = 0; i < PersonalList.Count; i++)
        {

            PersonalList[i] = PersonalList[i] + mouvement;
        }
    }
}
