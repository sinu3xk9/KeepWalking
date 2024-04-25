using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using FMODUnity;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;
using UnityEngine.Rendering.PostProcessing;

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

    public bool canLook = true;

    CharacterController characterController;
    Animator playerAnimator;

    public SplineContainer spline;
    private int pointIndex;
    private bool isPaused = false;

    //Player Item Use Variables
    public float useTick = 0f;
    public float holdTickSpeed;
    public float useDuration;
    private Vector3 splinePrev;
    private Vector3 splineCurrent;
    private Vector3 splineNext;
    private float movementEllipseRadius = 2f;
    public Animator armAnimator;
    public Animator pepperAnimator;
    //States: idle, texting, calling, pepperOut
    public string state = "idle";
    //Bool that allows the player to hold a button passed when the state changes. Once the button is released they can then hit again to deactivate state
    private bool holdingButton = false;



    // UI 
    [Header("UI")]
    public Image pepperRadial;
    public Image phoneRadial;
    public TextMeshProUGUI q;
    public TextMeshProUGUI e;
    private Color darkGrey = new Color(0.32f, 0.32f, 0.32f, 256);
    private Color lightGrey = new Color(0.71f, 0.71f, 0.71f, 256);
    private PostProcessVolume postProcessVolume;

    // Audio
    [Header("Audio")]
    [Range(0.0f, 1.0f)]
    public float stepChance;
    [Range(0.0f, 1.0f)]
    public float stopStepChance;
    private Vector3 feetDifferential = new Vector3(0f, .6f, 0f);
    
    IState usage;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //transform.position = spline.Spline[pointIndex].Position;
        splinePrev = spline.Spline[pointIndex].Position;
        postProcessVolume = transform.Find("PostProcessVolume").GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            updateSplinePoint();
            //Player Movement
            //Waterslide ellipse edition: checks if combined distance to current and next point (ellipse math) is greater than threshold. 
            if (((transform.position - splineNext).magnitude + (transform.position - splineCurrent).magnitude) > ((splineNext - splineCurrent).magnitude + movementEllipseRadius))
            {
                Debug.Log("Outisde Ellipse");
                Debug.Log(splineCurrent.ToString());
                Debug.Log(splineNext.ToString());
                //if outside threshold get the point between current and next spline and move player towards that point
                Vector3 middlePoint = splineCurrent + ((splineNext - splineCurrent) / 6);
                Debug.Log(middlePoint.ToString());
            }

            bool eDown = Input.GetKey(KeyCode.E);
            bool qDown = Input.GetKey(KeyCode.Q);
            switch (state)
            {
                case "idle":
                    //update camera rotation based on player input
                    takeCameraInput();
                    //update character mover based on player input
                    takeMovementInput();
                    //update tick counter and UI depending on held buttons
                    if (eDown && !qDown)
                    {
                        useTick += holdTickSpeed * Time.deltaTime;
                        phoneRadial.fillAmount = useTick / useDuration;
                        q.color = darkGrey;
                        e.color = lightGrey;
                    }
                    else if (!eDown && qDown)
                    {
                        useTick += holdTickSpeed * Time.deltaTime;
                        pepperRadial.fillAmount = useTick / useDuration;
                        q.color = lightGrey;
                        e.color = darkGrey;
                    }
                    else
                    {
                        useTick = 0;
                        phoneRadial.fillAmount = useTick / useDuration;
                        pepperRadial.fillAmount = useTick / useDuration;
                        q.color = darkGrey;
                        e.color = darkGrey;
                    }

                    //check if any buttons complete
                    if (useTick >= useDuration)
                    {
                        if (eDown)
                        {
                            enterTextingState();
                        }
                        else if (qDown)
                        {
                            enterPepperState();
                        }
                        holdingButton = true;
                    }
                    break;
                case "texting":
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        holdingButton = false;
                    }
                    else if (eDown && !holdingButton)
                    {
                        exitTextingState();
                    }
                    break;
                case "calling":
                    //update camera rotation based on player input
                    takeCameraInput();
                    //update character mover based on player input
                    takeMovementInput();
                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        holdingButton = false;
                    }
                    else if (eDown && !holdingButton)
                    {
                        exitCallingState();
                    }
                    break;
                case "pepperOut":
                    //update camera rotation based on player input
                    takeCameraInput();
                    //update character mover based on player input
                    takeMovementInput();
                    if (Input.GetKeyUp(KeyCode.Q))
                    {
                        holdingButton = false;
                    }
                    else if (qDown && !holdingButton)
                    {
                        exitPepperState();
                    }
                    break;
            }
        }
    }
    
    public void playerFootstep() 
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.playerFootstep, transform.position - feetDifferential);
        if(Random.value <= stepChance) {
            StartCoroutine(FollowFootstep());
        }
    }

    public void stopWalking()
    {
        if(Random.value <= stopStepChance) {
            StartCoroutine(FollowFootstep());
        }
    }

    IEnumerator FollowFootstep() {
        var soundPosition = transform.position + splineCurrent;
        soundPosition /= 2;
        soundPosition -= feetDifferential;
        yield return new WaitForSeconds(.5f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.followFootstep, soundPosition);
    }

    private void enterTextingState()
    {
        moveDirection = Vector3.zero;
        armAnimator.SetTrigger("TakeOutPhone");
        playerAnimator.SetBool("IsWalking", false);
        state = "texting";
    }
    private void exitTextingState()
    {
        armAnimator.SetTrigger("PutAwayPhone");
        e.color = darkGrey;
        phoneRadial.fillAmount = 0;
        useTick = 0;
        state = "idle";
    }

    private void enterCallingState()
    {
        armAnimator.SetTrigger("TakeOutPhone");
        state = "calling";
    }
    private void exitCallingState()
    {
        armAnimator.SetTrigger("PutAwayPhone");
        q.color = darkGrey;
        pepperRadial.fillAmount = 0;
        useTick = 0;
        state = "idle";
    }
    private void enterPepperState()
    {
        pepperAnimator.SetTrigger("TakeOutPepper"); ;
        state = "pepperOut";
    }
    private void exitPepperState()
    {
        pepperAnimator.SetTrigger("PutAwayPepper");
        q.color = darkGrey;
        //pepperActive = false;
        pepperRadial.fillAmount = 0;
        useTick = 0;
        state = "idle";
    }

    private void takeCameraInput()
    {
        if (canLook)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            //playerRB.velocity = gameObject.transform.forward * walkSpeed;
        }
    }

    private void takeMovementInput()
    {
        moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += (transform.TransformDirection(Vector3.forward) * walkSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += (transform.TransformDirection(Vector3.left) * walkSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += (transform.TransformDirection(Vector3.right) * walkSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += (transform.TransformDirection(Vector3.back) * walkSpeed);
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (Mathf.Abs(characterController.velocity.x) > .1f || Mathf.Abs(characterController.velocity.z) > .1f)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }
    }

    private void updateSplinePoint()
    {
        if (pointIndex < spline.Spline.Count - 1)
        {
            splineCurrent = spline.Spline[pointIndex].Position;
            splineNext = spline.Spline[pointIndex + 1].Position;

            if ((transform.position - splineNext).magnitude < (transform.position - splineCurrent).magnitude)
            {
                pointIndex++;
                splinePrev = splineCurrent;
            }
            //Debug.Log(pointIndex);
        }
    }

    public void togglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            playerAnimator.SetBool("IsWalking", false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
