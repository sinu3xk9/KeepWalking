using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed;
    public float lookSpeed;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    [SerializeField] Transform[] Points;
    private int pointIndex;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        transform.position = Points[pointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (pointIndex <= Points.Length - 1 && Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.MoveTowards(transform.position, Points[pointIndex].transform.position, walkSpeed * Time.deltaTime);
            if (transform.position == Points[pointIndex].transform.position)
            {
                pointIndex += 1;
            }
        }
    }
}
