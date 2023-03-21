using System.Collections;
using System.Collections.Generic;
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

    public TimeController time;

    public GameObject bed;

    public bool canInteract;

    public bool active;


    public Transform cam;

    public float playerActivateDistance;

    public GameObject photoCube;
    public PlayerCam playerCam;

    public TextMeshProUGUI batteryText;

    public float camBattery = 100f;



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
        rb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
        picnum = 0;

    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

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
        if (active && (!hit.collider.CompareTag("Ground") || (hit.collider.CompareTag("Animal"))))
        {
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }
        Debug.Log("Can interact? " + canInteract);

        if (cameraON)
        {
            camBattery -= 1 * Time.deltaTime;
            batteryText.text = "Battery level" + camBattery;
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
                ImportAssetAsSprite();
                playerCam.GetComponent<Camera>().fieldOfView = 60;
            }
        }
        if (Input.GetMouseButtonDown(0) && cameraON)
        {
            if (playerCam.currentHitObject != null)
            {
                ScreenCapture.CaptureScreenshot("Assets/Resources/gamepics/" + playerCam.currentHitObject.name + "/screenshot" + picnum + ".png");
            }
            else
            {
                ScreenCapture.CaptureScreenshot("Assets/Resources/gamepics/screenshot" + picnum + ".png");
            }
            AssetDatabase.Refresh();
            Debug.Log("A screenshot was taken!");
            picnum++;
        }

        if (Input.GetKeyDown(interactKey))
        {
            StartCoroutine(Rest());
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

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

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
    private void ImportAssetAsSprite()
    {
        AssetDatabase.Refresh();
        for (int i = 0; i < picnum; i++)
        {
            TextureImporter importer = AssetImporter.GetAtPath("Assets/Resources/gamepics/screenshot" + i + ".png") as TextureImporter;
            if (importer == null)
            {
                Debug.LogError("Could not TextureImport from path: ");
            }
            else
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
            }
        }
    }

}
