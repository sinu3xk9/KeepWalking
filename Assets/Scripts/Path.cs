using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] Points;
    [SerializeField] private float moveSpeed;
    private int pointIndex;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Points[pointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pointIndex <= Points.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed * Time.deltaTime);
            if (transform.position == Points[pointIndex].transform.position )
            {
                pointIndex += 1;
            }


        }
    }
}
