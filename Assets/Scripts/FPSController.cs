using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using FMODUnity;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

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
    public Animator pepperAnimator;

    // UI 
    [Header("UI")]
    public Image pepperRadial;
    public Image phoneRadial;
    public TextMeshProUGUI q;
    public TextMeshProUGUI e;
    private Color darkGrey = new Color(0.32f, 0.32f, 0.32f, 256);
    private Color lightGrey = new Color(0.71f, 0.71f, 0.71f, 256);
    
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
        moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = (transform.TransformDirection(Vector3.forward) * lookSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection = (transform.TransformDirection(Vector3.left) * lookSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection = (transform.TransformDirection(Vector3.right) * lookSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection = (transform.TransformDirection(Vector3.back) * lookSpeed);
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            //playerRB.velocity = gameObject.transform.forward * walkSpeed;
        }
        if (pointIndex <= spline.Spline.Count - 1)
        {
            Vector3 splineCurrent = spline.Spline[pointIndex].Position;
            Vector3 splineNext = spline.Spline[pointIndex + 1].Position;

            if ((transform.position - splineNext).magnitude < (transform.position - splineCurrent).magnitude)
            {
                pointIndex++;
            }
            //transform.position = Vector3.MoveTowards(transform.position, spline.Spline[pointIndex].Position, walkSpeed * Time.deltaTime);
            //if (transform.position == new Vector3(spline.Spline[pointIndex].Position.x, spline.Spline[pointIndex].Position.y, spline.Spline[pointIndex].Position.z))
            //{
            //    pointIndex += 1;
            //}
            playerAnimator.SetBool("IsWalking", true);
            Debug.Log(pointIndex);
        }
        else {
            playerAnimator.SetBool("IsWalking", false);
        }

        if (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q) && !pepperActive)
        {
            e.color = lightGrey;
            useTick++;
            phoneRadial.fillAmount = useTick / useDuration;
            if (useTick == useDuration)
            {
                phoneActive = true;
                pepperActive = false;
                armAnimator.SetTrigger("TakeOutPhone");
            }
        }
        else if (!Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q) && !phoneActive)
        {
            q.color = lightGrey;
            useTick++;
            pepperRadial.fillAmount = useTick / useDuration;
            if (useTick == useDuration)
            {
                phoneActive = false;
                pepperActive = true;
                pepperAnimator.SetTrigger("TakeOutPepper");
            }
        }
        else
        {
            useTick = 0;
            phoneRadial.fillAmount = useTick / useDuration;
            pepperRadial.fillAmount = useTick / useDuration;
        }

        if (!Input.GetKey(KeyCode.Q) && pepperActive)
        {
            pepperAnimator.SetTrigger("PutAwayPepper");
            q.color = darkGrey;
            pepperActive = false;
            pepperRadial.fillAmount = 0;
            useTick = 0;
        }

        if (!Input.GetKey(KeyCode.E) && phoneActive)
        {
            armAnimator.SetTrigger("PutAwayPhone");
            e.color = darkGrey;
            phoneActive = false;
            phoneRadial.fillAmount = 0;
            useTick = 0;
        }
    }
    
    public void playerFootstep() 
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerFootstep, transform.position);
    }
}
