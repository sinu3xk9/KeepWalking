using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TrashCanScript : MonoBehaviour
{
    public GameObject trashcan;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            trashcan.transform.eulerAngles = new Vector3(trashcan.transform.eulerAngles.x + 90, trashcan.transform.eulerAngles.y, trashcan.transform.eulerAngles.z + 45);
            trashcan.transform.position = new Vector3(trashcan.transform.position.x, 0.5f, trashcan.transform.position.z);
            Debug.Log("TRASHCAN");
        }
    }
}
