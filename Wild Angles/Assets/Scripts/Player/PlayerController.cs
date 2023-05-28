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
    private bool canOpenCamera;

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
    private bool canFlash = false;
    [Header("Other")]
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private TextMeshProUGUI dayText;
    [SerializeField]
    private TextMeshProUGUI modeText;
    [Header("Animal Mission Texts")]
    public Text OwlText;
    public Text ButterflyText;
    public Text CrocodileText;
    public Text SnakeText;
    public Text FrogText;
    public Text BugText;
    public Text TigerText;

    public int lastDaySaved;
    [SerializeField]
    public Image batteryIcon1;
    public Image batteryIcon2;
    public Image batteryIcon3;
    [Header("Controllers")]
    public NightVisionController nightVisionController;
    public HealthbarController healthbar;
    public StaminaBarController staminaBar;
    public BatteryBarController batteryBar;
    public PlantController plantController;
    public GameManager gameManager;

    private bool diaryOpen;
    [SerializeField] GameObject diaryUI;
    PlayerController controller;
    public GameObject timeController;
    public RenderTexture rt;
    public List<Texture2D> listaTeste;
    public DiaryController diaryController;

    public GameObject notsleptUi;
    [SerializeField] bool resting;
    [SerializeField] GameObject tentCammera;
    [SerializeField] GameObject playerBody;
    [SerializeField] GameObject companion;
    [Header("Sound")]

    public AudioSource sound;
    public AudioSource walkSound;
    public AudioSource sprintSound;
    [Header("step sounds")]
    public AudioClip stepClip, sprintClip;
    [Header("CameraSound")]

    public AudioClip cameraOutClip, takingPhotoClip;



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
        canOpenCamera = true;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        timeController = GameObject.FindGameObjectWithTag("TimeController");
        //plantController = GameObject.FindGameObjectWithTag("Interactable Plant").GetComponent<PlantController>();
        rb.freezeRotation = true;
        readyToJump = true;
        isFlashing = false;
        startYScale = transform.localScale.y;
        picnum = 0;
        healthbar.UpdateHealthBar(100, health);
        popUpText.gameObject.SetActive(false);
        flashIcon.SetActive(false);
        Camera.onPostRender += OnPostRenderCallback;


    }

    private void Update()
    {
        //CHECK IF IS RESTING
        if (resting)
        {
            timeController.GetComponent<TimeController>().timeMultiplier = 1500;
            health += 1 * Time.deltaTime;
            healthbar.UpdateHealthBar(100, health);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Cursor.lockState = CursorLockMode.None;
                resting = false;
                playerBody.SetActive(true);
                tentCammera.SetActive(false);
                timeController.GetComponent<TimeController>().timeMultiplier = 300;
            }
        }
        //CHECK IF SLEPT IN THE LAST 2 DAYS
        if (timeController.GetComponent<TimeController>().dayCounter - lastDaySaved >= 2)
        {
            health -= 0.7f * Time.deltaTime;
            healthbar.UpdateHealthBar(100, health);
            notsleptUi.SetActive(true);
            if (health <= 0)
            {
                gameManager.RespawnPlayer();
            }
            //AVISAR PLAYER NO UI
        }
        else
        {
            notsleptUi.SetActive(false);
        }
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        UpdateRechargeIcon();

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
        GameObject companion = null;
        if (active && hit.collider.CompareTag("Interactable"))
        {
            canInteract = true;
            interactText.text = "Press F to rest";
            interactText.enabled = true;

        }
        else if (active && hit.collider.CompareTag("Companion"))
        {
            companion = hit.collider.gameObject;
            companion.GetComponent<CompanionTuturial>().canInteract = true;
            interactText.text = "Press F to interact";
            interactText.enabled = true;

        }
        else if (active && hit.collider.CompareTag("Interactable Plant"))
        {
            interactText.enabled = true;
            plantController = hit.collider.gameObject.GetComponent<PlantController>();
            plantController.canInteract = true;
            interactText.text = "Press F to Consume";
        }
        else
        {
            if (plantController != null) plantController.canInteract = false;
            canInteract = false;
            interactText.enabled = false;
            if (companion != null)
            {
                companion.GetComponent<CompanionTuturial>().canInteract = false;
            }
        }

        if (cameraON)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (canFlash)
                {
                    flashIcon.SetActive(false);
                    canFlash = false;
                }
                else
                {
                    flashIcon.SetActive(true);
                    canFlash = true;
                }
            }
            //renderCam.SetActive(true);
            GameObject closest;
            closest = playerCam.GetClosestPhotographable();
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
        }
        else
        {
            flashIcon.SetActive(false);
            //renderCam.SetActive(false);

            foreach (var animal in playerCam.currentAnimalsInTheframe)
            {
                Outline outline = animal.GetComponent<Outline>();
                if (outline != null) outline.enabled = false;
            }
        }
        if (camBattery <= 0)
        {
            StartCoroutine(NoBatteryText());
            flashIcon.SetActive(false);
            cameraUI.SetActive(false);
            cameraON = false;
            if (nightVisionController.isEnabled == true)
            {
                nightVisionController.isEnabled = false;
                nightVisionController.volume.weight = 0;
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
        if (moveDirection.sqrMagnitude > 0.1 && moveSpeed > 10)
        {

            walkSound.enabled = false;
            sprintSound.enabled = true;
        }
        else if (moveDirection.sqrMagnitude > 0.1)
        {

            walkSound.enabled = true;
            sprintSound.enabled = false;
        }
        else
        {

            walkSound.enabled = false;
            sprintSound.enabled = false;

        }

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

        if (Input.GetKeyDown(cameraKey) && canOpenCamera)
        {
            if (!cameraON && camBattery > 0)
            {
                cameraUI.SetActive(true);
                cameraON = true;
                sound.clip = cameraOutClip;
                sound.Play();
                if (canFlash) flashIcon.SetActive(true); else flashIcon.SetActive(false);

            }
            else
            {
                nightVisionController.isEnabled = false;
                nightVisionController.volume.weight = 0;
                cameraUI.SetActive(false);
                cameraON = false;
                sound.clip = cameraOutClip;
                sound.Play();
                playerCam.GetComponent<Camera>().fieldOfView = 60;
            }
            if (camBattery <= 0)
            {
                StartCoroutine(NoBatteryText());
            }

        }
        if (Input.GetKeyDown(KeyCode.G)) companion.GetComponent<FSMNavMeshAgent>().following = true;

        if (Input.GetKeyDown(KeyCode.T)) companion.GetComponent<FSMNavMeshAgent>().homming = true;



        //TAKING THE PIC
        if (Input.GetMouseButtonDown(0) && cameraON)
        {
            StartCoroutine("WaitFrame");
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
            Rest();
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (diaryOpen)
            {
                timeController.GetComponent<TimeController>().timeMultiplier = 300;
                playerCam.enabled = true;
                diaryUI.SetActive(false);
                diaryOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                canOpenCamera = true;
                cameraON = false;


            }
            else
            {
                timeController.GetComponent<TimeController>().timeMultiplier = 0;

                playerCam.enabled = false;
                diaryUI.SetActive(true);
                diaryOpen = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                cameraON = false;
                cameraUI.SetActive(false);
                canOpenCamera = false;

            }
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (timeController.GetComponent<TimeController>().timeMultiplier == 500) timeController.GetComponent<TimeController>().timeMultiplier = 300;
            else if (timeController.GetComponent<TimeController>().timeMultiplier == 300) timeController.GetComponent<TimeController>().timeMultiplier = 500;
        }
    }
    private IEnumerator WaitFrame()
    {
        renderCam.SetActive(true);
        sound.clip = takingPhotoClip;
        sound.Play();
        Debug.Log(canFlash);
        if (canFlash)
        {
            light.SetActive(true);
            isFlashing = true;
        }
        cameraUI.SetActive(false);
        GameObject closest = null;
        closest = playerCam.GetClosestPhotographable();
        yield return new WaitForEndOfFrame();
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
            if (closest.GetComponent<Photographable>().haveBeenSeen == false)
            {
                popUpText.text = "new " + closest.name + " photo added to the diary \nA new mission, description and fact about " + closest.name + " have been added to the diary!";
            }
            else
            {
                popUpText.text = "new " + closest.name + " photo added to the diary!";
            }

            UpdatePageUi(closest, point);
            UpdatePageButton(closest);
            //WHEN TAKING THE PIC ACTIVATE THE IS KNOWN VARIABLE IN PHOTOGRAPHABLE
            diaryController.animalList.Add(new DiaryController.DiaryData(closest.name, tex, false, point));
            if (closest.name.Contains("Crocodile"))
            {

            }
            else if (closest.name.Contains("Owl"))
            {
                GameObject animal = FindParentWithTag(closest, "Animal");
                if (animal.GetComponent<FiniteStateMachine>().currentState.name == "OwlPatrolState" && closest.GetComponent<Photographable>().haveBeenSeen)
                {
                    OwlText.text = "Mission Passed you gained +50 points";
                    points += 50;
                }

            }
            else if (closest.name.Contains("Butterfly"))
            {
                if (playerCam.MoreThenOneInFrame("Butterfly") && closest.GetComponent<Photographable>().haveBeenSeen)
                {
                    ButterflyText.text = "Mission Passed you gained +50 points";
                    points += 50;
                }
            }
            else if (closest.name.Contains("Frog"))
            {
                if (playerCam.MoreThenOneInFrame("ButteFrogrfly") && closest.GetComponent<Photographable>().haveBeenSeen)
                {
                    FrogText.text = "Mission Passed you gained +50 points";
                    points += 50;
                }
            }
            else if (closest.name.Contains("Snake"))
            {
                GameObject animal = FindParentWithTag(closest, "Animal");
                if (animal.GetComponent<FiniteStateMachine>().currentState.name == "SnakeStareState" && closest.GetComponent<Photographable>().haveBeenSeen)
                {
                    SnakeText.text = "Mission Passed you gained +50 points";
                    points += 50;
                }
            }
            else if (closest.name.Contains("Bug"))
            {

            }
            else if (closest.name.Contains("Tiger"))
            {

            }
            else if (closest.name.Contains("White Orchid"))
            {

            }
            else if (closest.name.Contains("Poison Orchid"))
            {

            }
            else if (closest.name.Contains("Cocoa Tree"))
            {

            }
            else if (closest.name.Contains("Banana Tree"))
            {

            }
            closest.GetComponent<Photographable>().haveBeenSeen = true;
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
        camBattery -= 5;
        renderCam.SetActive(false);

    }

    private void OnPostRenderCallback(Camera cam)
    {
        if (cam == Camera.main)
        {
            Debug.Log($"AHALÇSDIEWGELMWEG");

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
        {
            walkSound.enabled = false;
            rb.AddForce(moveDirection.normalized * moveSpeed / airMultiplier, ForceMode.Force);
        }
        else
        {
            walkSound.enabled = false;

        }
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
    private void Rest()
    {
        lastDaySaved = timeController.GetComponent<TimeController>().dayCounter;
        resting = true;
        rechargeAmount = 3;
        tentCammera.SetActive(true);
        transform.position = new Vector3(441.08f, 0, 1207.82f);
        playerBody.SetActive(false);
        diaryController.SavePhotos();
    }
    private IEnumerator CameraUIOn()
    {
        yield return new WaitForSeconds(1f);
        if (cameraON)
        {
            cameraUI.SetActive(true);
        }
        light.SetActive(false);
        isFlashing = false;
        yield return new WaitForSeconds(2f);
        popUpText.gameObject.SetActive(false);

    }
    public void DamagePlayer(float damage)
    {
        health -= damage;
        healthbar.UpdateHealthBar(100, health);

        if (health < 0)
        {
            health = 0;
            gameManager.RespawnPlayer();
        }
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

    private void UpdateRechargeIcon()
    {
        switch (rechargeAmount)
        {
            case 0:
                batteryIcon1.enabled = false;
                batteryIcon2.enabled = false;
                batteryIcon3.enabled = false;
                break;
            case 1:
                batteryIcon1.enabled = true;
                batteryIcon2.enabled = false;
                batteryIcon3.enabled = false;
                break;
            case 2:
                batteryIcon1.enabled = true;
                batteryIcon2.enabled = true;
                batteryIcon3.enabled = false;
                break;
            case 3:
                batteryIcon1.enabled = true;
                batteryIcon2.enabled = true;
                batteryIcon3.enabled = true;
                break;
        }
    }

    private IEnumerator NoBatteryText()
    {
        popUpText.gameObject.SetActive(true);
        popUpText.text = "You have no battery! Quickly, recharge your battery! (Press R to recharge)";
        yield return new WaitForSeconds(5f);
        popUpText.gameObject.SetActive(false);
    }

    private void UpdatePageButton(GameObject animal)
    {
        String name = animal.name;
        GameObject first = FindChildGameObjectByName(diaryController.gameObject, "MainPage");
        GameObject objectName = FindChildGameObjectByName(first, name + "Button");
        TextMeshProUGUI buttonText = FindChildGameObjectByName(objectName, "Text").GetComponent<TextMeshProUGUI>();
        switch (objectName.name)
        {
            case "CrocodileButton":
                buttonText.text = "Crocodile Page";
                break;
            case "OwlButton":
                buttonText.text = "Owl Page";
                break;
            case "FrogButton":
                buttonText.text = "Frog Page";
                break;
            case "BugButton":
                buttonText.text = "Bug Page";
                break;
            case "ButterflyButton":
                buttonText.text = "Butterfly Page";
                break;
            case "SnakeButton":
                buttonText.text = "Snake Page";
                break;
            case "TigerButton":
                buttonText.text = "Tiger Page";
                break;
            case "WhiteOrchidButton":
                buttonText.text = "White Orchid Page";
                break;
            case "PurpleOrchidButton":
                buttonText.text = "Purple Orchid Page";
                break;
            case "CocoaTreeButton":
                buttonText.text = "Cocoa Tree Page";
                break;
            case "BananaTreeButton":
                buttonText.text = "Banana Tree Page";
                break;
            case "HeliconiaButton":
                buttonText.text = "Heliconia Page";
                break;
        }
    }

    private void UpdatePageUi(GameObject animal, int score)
    {
        GameObject an = FindParentWithTag(animal, "Animal");
        if (an != null)
        {
            if (an.CompareTag("Animal"))
            {
                string name = animal.name;
                GameObject objectName = FindChildGameObjectByName(diaryController.gameObject, name + "Page");
                Text mission = FindChildGameObjectByName(objectName, "Mission Text").GetComponent<Text>();
                GameObject description = FindChildGameObjectByName(objectName, "Description Text");
                GameObject score1 = FindChildGameObjectByName(objectName, "Good Rating");
                GameObject score2 = FindChildGameObjectByName(objectName, "Mid Rating");
                GameObject score3 = FindChildGameObjectByName(objectName, "Bad Rating");
                switch (objectName.name)
                {
                    case "CrocodilePage":
                        mission.text = "Mission : Take a photo of a crocodile attacking you";
                        description.SetActive(true);
                        break;
                    case "OwlPage":
                        Debug.Log($"AQUI");
                        mission.text = "Mission : Take a photo of an Owl flying";
                        description.SetActive(true);
                        break;
                    case "ButterflyPage":
                        mission.text = "Mission : Take a photo of a butterfly flying ";
                        description.SetActive(true);
                        break;
                    case "BugPage":
                        mission.text = "Mission : Take a perfect photo of a bug";
                        description.SetActive(true);
                        break;
                    case "FrogPage":
                        mission.text = "Mission : Take a perfect photo of a frog";
                        description.SetActive(true);
                        break;
                    case "SnakePage":
                        mission.text = "Mission : Take a photo of a Snake Staring at You";
                        description.SetActive(true);
                        break;
                }
                if (score == 1)
                {
                    score1.SetActive(true);
                    score2.SetActive(false);
                    score3.SetActive(false);
                }
                else if (score == 2)
                {
                    score2.SetActive(true);
                    score1.SetActive(false);
                    score3.SetActive(false);
                }
                if (score == 3)
                {
                    score3.SetActive(true);
                    score2.SetActive(false);
                    score1.SetActive(false);
                }
            }
        }

    }
    private GameObject FindChildGameObjectByName(GameObject topParentObject, string gameObjectName)
    {
        for (int i = 0; i < topParentObject.transform.childCount; i++)
        {
            if (topParentObject.transform.GetChild(i).name == gameObjectName)
            {
                return topParentObject.transform.GetChild(i).gameObject;
            }

            GameObject tmp = FindChildGameObjectByName(topParentObject.transform.GetChild(i).gameObject, gameObjectName);

            if (tmp != null)
            {
                return tmp;
            }
        }
        return null;
    }
    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
}
