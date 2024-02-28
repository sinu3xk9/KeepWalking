using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using FMODUnity;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class FPSController : MonoBehaviour
{
    //Player Movement Variables
    public Camera playerCamera;
    public float walkSpeed;
    public float lookSpeed;
    public float lookXLimit = 45f;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;
    Animator playerAnimator;

    public SplineContainer spline;
    private int pointIndex;

    //Player Item Use Variables
    public float useTick = 0;
    public float useDuration = 75;
    public bool phoneActive = false;
    public bool pepperActive = false;
    public Animator armAnimator;

    IState usage;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        transform.position = spline.Spline[pointIndex].Position;
    }

    // Update is called once per frame
    void Update()
    {
        //Player Movement
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        if (pointIndex <= spline.Spline.Count - 1 && Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.MoveTowards(transform.position, spline.Spline[pointIndex].Position, walkSpeed * Time.deltaTime);
            if (transform.position == new Vector3(spline.Spline[pointIndex].Position.x, spline.Spline[pointIndex].Position.y, spline.Spline[pointIndex].Position.z))
            {
                pointIndex += 1;
            }
            playerAnimator.SetBool("IsWalking", true);
        }
        else {
            playerAnimator.SetBool("IsWalking", false);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q))
        {
            useTick++;
            if (useTick == useDuration)
            {
                phoneActive = true;
                pepperActive = false;
                armAnimator.SetTrigger("AnswerCall");
            }
        }
        else if (!Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q))
        {
            useTick++;
            if (useTick == useDuration)
            {
                phoneActive = false;
                pepperActive = true;
                armAnimator.SetTrigger("GetOutPepperSpray");
            }
        }
        else
        {
            useTick = 0;
            phoneActive = false;
            pepperActive = false;
        }
    }
    
    public void playerFootstep() 
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerFootstep, transform.position);
    }
}
