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
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode changeFocusModeKey = KeyCode.Q;
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
    [SerializeField] TextMeshProUGUI popUpText;

    public bool cameraON;
    private int picnum;
    public TimeController time;
    public GameObject bed;
    public bool canInteract;
    public bool canInteractPlant;
    public bool active;
    public Transform cam;
    public Camera camera;
    public GameObject renderCam;
    public float playerActivateDistance;
    public GameObject photoCube;
    public PlayerCam playerCam;
    public TextMeshProUGUI interactText;
    public float camBattery = 100f;
    public float health = 100f;
    public int rechargeAmount = 3;
    [Header("Camera Flash")]
    public bool isFlashing;
    public GameObject light;
    public GameObject flashIcon;
    public bool canFlash = false;
    [Header("Other")]
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI dayText;
    [SerializeField]
    private TextMeshProUGUI modeText;
    public TextMeshProUGUI OwlText;
    public int lastDaySaved;
    [Header("Controllers")]
    public NightVisionController nightVisionController;
    public HealthbarController healthbar;
    public StaminaBarController staminaBar;
    public BatteryBarController batteryBar;

    private bool diaryOpen;
    [SerializeField] GameObject diaryUI;
    PlayerController controller;
    public GameObject timeController;
    public RenderTexture rt;
    public List<Texture2D> listaTeste;
    public DiaryController diaryController;
    public enum PhotographMode { CloseFocus, LongFocus };
    public PhotographMode photographMode;
    public int points;
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
        timeController = GameObject.FindGameObjectWithTag("TimeController");
        rb.freezeRotation = true;
        readyToJump = true;
        isFlashing = false;
        startYScale = transform.localScale.y;
        picnum = 0;
        dayText.enabled = false;
        timeText.enabled = false;
        healthbar.UpdateHealthBar(100, health);
        popUpText.gameObject.SetActive(false);
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
        {
            rb.drag = groundDrag;
        }
        else
        {
            moveDirection.y -= 9.81f * Time.deltaTime;
            rb.drag = 0;
        }


        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, playerActivateDistance);
        if (active && hit.collider.CompareTag("Interactable"))
        {
            canInteract = true;
            interactText.enabled = true;

        }
        else if (active && hit.collider.CompareTag("Interactable Plant"))
        {
            canInteractPlant = true;
            interactText.enabled = true;
        }
        else
        {
            canInteract = false;
            canInteractPlant = false;
            interactText.enabled = false;
        }

        if (cameraON)
        {
            renderCam.SetActive(true);
            GameObject closest;
            if (photographMode == PhotographMode.CloseFocus)
            {
                closest = playerCam.GetClosestPhotographable();
            }
            else
            {
                closest = playerCam.GetClosestPhotographableAngle();
            }
            if (closest != null)
            {
                foreach (var animal in playerCam.currentAnimalsInTheframe)
                {
                    if (animal != closest)
                    {
                        Outline outline = animal.GetComponent<Outline>();
                        if (outline != null) outline.enabled = false;
                    }
                    else
                    {
                        Outline outline = animal.GetComponent<Outline>();
                        if (outline != null) outline.enabled = true;
                    }

                }
            }

            canFlash = true;
            camBattery -= 1 * Time.deltaTime;
            if (nightVisionController.isEnabled)
            {
                camBattery -= 2 * Time.deltaTime;
            }
            if (camBattery <= 0)
            {
                cameraUI.SetActive(false);
                cameraON = false;
                playerCam.GetComponent<Camera>().fieldOfView = 60;
            }
            batteryBar.UpdateBatteryBar(100, camBattery);

            if (Input.GetKey(interactKey) && canFlash == true)
            {
                canFlash = false;
                light.SetActive(true);
                flashIcon.SetActive(true);
            }
            else
            {
                canFlash = true;
                //light.SetActive(false);
                flashIcon.SetActive(false);
            }
        }
        else
        {
            renderCam.SetActive(false);
            /*GameObject closest;
            if (photographMode == PhotographMode.CloseFocus)
            {
                closest = playerCam.GetClosestPhotographable();
            }
            else
            {
                closest = playerCam.GetClosestPhotographableAngle();
            }
            closest.GetComponent<Outline>().enabled = false;
            Debug.Log(closest.name);*/
            foreach (var animal in playerCam.currentAnimalsInTheframe)
            {
                Outline outline = animal.GetComponent<Outline>();
                if (outline != null) outline.enabled = false;
            }
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
            if (playerStamina >= 20)
            {
                Jump();

            }
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
                nightVisionController.isEnabled = false;
                nightVisionController.volume.weight = 0;
                cameraUI.SetActive(false);
                cameraON = false;
                playerCam.GetComponent<Camera>().fieldOfView = 60;
            }
        }

        if (Input.GetKeyDown(changeFocusModeKey))
        {

            if (photographMode == PhotographMode.CloseFocus)
            {
                photographMode = PhotographMode.LongFocus;
            }
            else
            {
                photographMode = PhotographMode.CloseFocus;
            }
            modeText.text = "Mode: " + photographMode;
        }

        //TAKING THE PIC
        if (Input.GetMouseButtonDown(0) && cameraON)
        {

            light.SetActive(true);
            flashIcon.SetActive(true);
            cameraUI.SetActive(false);
            GameObject closest = null;
            if (photographMode == PhotographMode.CloseFocus)
            {
                closest = playerCam.GetClosestPhotographable();
            }
            else
            {
                closest = playerCam.GetClosestPhotographableAngle();
            }

            if (closest != null)
            {
                int point = GradePhoto(closest);
                if (point == 1)
                {
                    points += 50;
                }
                else if (point == 2)
                {
                    points += 25;
                }
                else if (point == 3)
                {
                    points += 5;
                }
                int animalTypeId = (int)closest.GetComponent<Photographable>().GetID();
                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false, true);
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();
                RenderTexture.active = null;
                popUpText.gameObject.SetActive(true);
                popUpText.text = "new " + closest.name + " photo added to the diary";
                //listaTeste.Add(tex);
                //WHEN TAKING THE PIC ACTIVATE THE IS KNOWN VARIABLE IN PHOTOGRAPHABLE
                if (closest.name.Contains("Crocodile"))
                {
                    diaryController.crocodilePhotos.Add(tex);
                }
                else if (closest.name.Contains("Owl"))
                {
                    diaryController.owlPhotos.Add(tex);
                    //Debug.Log(closest.GetComponentInParent<FiniteStateMachine>().currentState);
                    //IF (THE STATE IS CORRECT)
                    /*if (closest.GetComponentInParent<GameObject>().GetComponentInParent<FiniteStateMachine>().  currentState.name == "OwlPatrolState")
                    {
                        OwlText.text = "Mission Passed you gained +50 points";
                        points += 50;
                    }*/
                }
                else if (closest.name.Contains("Butterfly"))
                {
                    diaryController.butterflyPhotos.Add(tex);
                }
                else if (closest.name.Contains("Frog"))
                {
                    diaryController.frogPhotos.Add(tex);
                }
                else if (closest.name.Contains("Snake"))
                {
                    diaryController.snakePhotos.Add(tex);
                }
                else if (closest.name.Contains("Bug"))
                {
                    diaryController.bugPhotos.Add(tex);
                }
                else if (closest.name.Contains("Baby Tiger"))
                {
                    diaryController.babyTigerPhotos.Add(tex);
                }
                else if (closest.name.Contains("White Orchid"))
                {
                    diaryController.whiteOrchidPhotos.Add(tex);
                }
                else if (closest.name.Contains("Poison Orchid"))
                {
                    diaryController.purpleOrchidPhotos.Add(tex);
                }
                else if (closest.name.Contains("Cocoa Tree"))
                {
                    diaryController.cocoaTreePhotos.Add(tex);
                }
                else if (closest.name.Contains("Banana Tree"))
                {
                    diaryController.bananaTreePhotos.Add(tex);
                }
            }
            else
            {
                RenderTexture.active = rt;
                Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();
                RenderTexture.active = null;
                picnum++;
            }
            isFlashing = true;
            StartCoroutine(CameraUIOn());
            StartCoroutine(FlashOn());
            camBattery -= 5;
        }
        if (Input.GetKeyDown(reloadKey))
        {

            if (rechargeAmount <= 0)
            {
                rechargeAmount = 0;
                print("No more recharges!");
            }
            else
            {
                camBattery = 100f;
                rechargeAmount--;
            }

        }

        if (Input.GetKeyDown(interactKey) && canInteract)
        {

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            dayText.enabled = true;
            timeText.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            dayText.enabled = false;
            timeText.enabled = false;
        }
        //CHEATS
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerStamina = 1000000000;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            camBattery = 100000000000;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            health = 100000000000;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            points += 10001;
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
            rb.AddForce(moveDirection.normalized * moveSpeed / airMultiplier, ForceMode.Force);
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
        playerStamina -= 20;
        if (playerStamina < 0)
        {
            playerStamina = 0;
        }
        staminaBar.UpdateStaminaBar(100, playerStamina);
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
            if (playerStamina < 100 && grounded) playerStamina = playerStamina + 20 * Time.deltaTime;
        }
        staminaBar.UpdateStaminaBar(100, playerStamina);
    }

    private IEnumerator Rest()
    {
        lastDaySaved = timeController.GetComponent<TimeController>().dayCounter;
        timeController.GetComponent<TimeController>().enabled = false;
        timeController.GetComponent<TimeController>().currentTime += timeController.GetComponent<TimeController>().restTime;
        yield return new WaitForSeconds(0.5f);
        //Debug.Log(timeController.GetComponent<TimeController>().currentTime.TimeOfDay);
        timeController.GetComponent<TimeController>().enabled = true;

        if (timeController.GetComponent<TimeController>().currentTime.TimeOfDay <= timeController.GetComponent<TimeController>().midDayTime)
        {
            timeController.GetComponent<TimeController>().dayCounter++;
        }
        rechargeAmount = 3;

    }
    private IEnumerator CameraUIOn()
    {
        yield return new WaitForSeconds(1f);
        cameraUI.SetActive(true);
        popUpText.gameObject.SetActive(false);
    }
    private IEnumerator FlashOn()
    {
        yield return new WaitForSeconds(1);
        light.SetActive(false);
        flashIcon.SetActive(false);
        isFlashing = false;
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;
        healthbar.UpdateHealthBar(100, health);
    }

    public void HealPlayer(int heal)
    {
        health += heal;
        if (health >= 100)
        {
            health = 100;
        }
        healthbar.UpdateHealthBar(100, health);
    }
    private int GradePhoto(GameObject animal)
    {

        Vector3 direction = transform.position - animal.transform.position;
        float angle = Vector3.Angle(direction, animal.transform.parent.forward);
        Vector3 targetDir = animal.transform.position - transform.position;
        float angle2 = Vector3.Angle(targetDir, transform.forward);

        if (angle < 30 && angle2 < 15)
        {
            return 1;
        }
        else if (angle < 30 || angle2 < 15)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}
