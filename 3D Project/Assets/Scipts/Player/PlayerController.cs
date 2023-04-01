using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode cameraKey = KeyCode.E;
    public KeyCode interactKey = KeyCode.F;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;
    public MovementState state;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private bool isWalking;
    private bool isRunning;
    Animator anim;
    [Header("Stamina Check")]
    public float playerStamina;

    [Header("Camera Stuff")]
    [SerializeField] GameObject cameraUI;
    public bool cameraON;
    private int picnum;
    private int crocodilePicNum;

    public TimeController time;

    public GameObject bed;

    public bool canInteract;

    public bool active;


    public Transform cam;

    public Camera camera;

    public float playerActivateDistance;

    public GameObject photoCube;
    public PlayerCam playerCam;

    public TextMeshProUGUI batteryText;

    public float camBattery = 100f;

    public float health = 100f;
    [Header("Camera Flash")]
    public bool isFlashing;
    public GameObject light;

    public NightVisionController nightVisionController;

    private bool diaryOpen;

    [SerializeField] GameObject diaryUI;

    PlayerController controller;

    public RenderTexture rt;

    public List<Texture2D> listaTeste;

    public DiaryController diaryControler;




    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        rb.freezeRotation = true;
        readyToJump = true;
        isFlashing = false;
        startYScale = transform.localScale.y;
        picnum = 0;
        crocodilePicNum = 0;

    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, playerActivateDistance);
        if (active && hit.collider.CompareTag("Interactable"))
        {
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }

        if (cameraON)
        {
            camBattery -= 1 * Time.deltaTime;
            if (nightVisionController.isEnabled)
            {
                camBattery -= 2 * Time.deltaTime;
            }
            if (camBattery <= 100 && camBattery >= 75)
            {
                batteryText.text = "Battery - ||||";
            }
            else if (camBattery <= 74 && camBattery >= 50)
            {
                batteryText.text = "Battery - |||";
            }
            else if (camBattery <= 49 && camBattery >= 25)
            {
                batteryText.text = "Battery - ||";
            }
            else if (camBattery <= 24 && camBattery >= 1)
            {
                batteryText.text = "Battery - | \n LOW BATTERY";
            }

        }
        if (cameraON && camBattery <= 0)
        {
            cameraUI.SetActive(false);
            cameraON = false;
            playerCam.GetComponent<Camera>().fieldOfView = 60;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        anim.SetFloat("Speed", moveDirection.sqrMagnitude);
        StaminaChecker(moveDirection.sqrMagnitude);



        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
        if (Input.GetKeyUp(sprintKey))
        {
            anim.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(cameraKey))
        {
            if (!cameraON && camBattery > 0)
            {
                cameraUI.SetActive(true);
                cameraON = true;
            }
            else
            {
                cameraUI.SetActive(false);
                cameraON = false;
                playerCam.GetComponent<Camera>().fieldOfView = 60;
            }
        }
        //TAKING THE PIC
        if (Input.GetMouseButtonDown(0) && cameraON)
        {
            light.SetActive(true);
            cameraUI.SetActive(false);
            GameObject closest = playerCam.getClosestPhotographable();
            if (closest != null)
            {
                int animalTypeId = (int)closest.GetComponent<Photographable>().GetID();

                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();
                RenderTexture.active = null;
                //listaTeste.Add(tex);
                if (closest.name == "Crocodile")
                {
                    diaryControler.crocodilePhotos.Add(tex);
                }
                else if (closest.name == "Owl")
                {
                    diaryControler.owlPhotos.Add(tex);
                }

                /*byte[] bytes = image.EncodeToPNG();
                string path = playerCam.animalList[animalTypeId] + "/screenshot" + playerCam.picCounter[animalTypeId] + ".png";

                File.WriteAllBytes("Assets/Resources/gamepics/" + path, bytes);
                ScreenCapture.CaptureScreenshot("Assets/Resources/gamepics/" + playerCam.animalList[animalTypeId] + "/screenshot" + playerCam.picCounter[animalTypeId] + ".png");*/
                playerCam.picCounter[animalTypeId]++;

            }
            else
            {
                /*var currentRT = RenderTexture.active;
                RenderTexture.active = camera.targetTexture;
                camera.Render();
                Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false);
                image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
                image.Apply();

                byte[] bytes = image.EncodeToPNG();
                string path = "/screenshot" + picnum + ".png";

                File.WriteAllBytes("Assets/Resources/gamepics/" + path, bytes);*/

                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();
                RenderTexture.active = null;
                //listaTeste.Add(tex);

                //ScreenCapture.CaptureScreenshot("Assets/Resources/gamepics/screenshot" + picnum + ".png");
                picnum++;
            }
            Debug.Log("A screenshot was taken!");
            isFlashing = true;
            StartCoroutine(CameraUIOn());
            StartCoroutine(FlashOn());
            camBattery -= 5;
        }

        if (Input.GetKey(interactKey) && bed.GetComponent<BedController>().canInteract)
        {
            Debug.Log($"papapapapa");
            StartCoroutine(Rest());
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (diaryOpen)
            {
                playerCam.enabled = true;
                diaryUI.SetActive(false);
                diaryOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

            }
            else
            {
                playerCam.enabled = false;
                diaryUI.SetActive(true);
                diaryOpen = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
        }
    }


    private void StateHandler()
    {
        // Moce - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        if (grounded && Input.GetKey(sprintKey) && (playerStamina > 0))
        {
            anim.SetBool("isRunning", true);
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            anim.SetBool("isRunning", false);
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

    }
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float forwardsAmount = Vector3.Dot(transform.forward, moveDirection);

        // on ground
        if (grounded)
        {
            if (forwardsAmount < -.5f)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);
            }
            else if (forwardsAmount < .5f)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 7.5f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
        }
        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StaminaChecker(float speed)
    {
        if (speed > 0.01 && grounded && Input.GetKey(sprintKey))
        {
            if (playerStamina > 0) playerStamina = playerStamina - 20 * Time.deltaTime;
        }
        else
        {
            if (playerStamina < 100) playerStamina = playerStamina + 20 * Time.deltaTime;
        }
    }

    private IEnumerator Rest()
    {
        Debug.Log("Skipping time");

        Time.timeScale = 60;

        yield return new WaitForSeconds(10);

        Time.timeScale = 1;
    }
    private IEnumerator CameraUIOn()
    {
        yield return new WaitForSeconds(0);
        cameraUI.SetActive(true);
    }
    private IEnumerator FlashOn()
    {
        yield return new WaitForSeconds(1);
        light.SetActive(false);
        isFlashing = false;
    }




}
