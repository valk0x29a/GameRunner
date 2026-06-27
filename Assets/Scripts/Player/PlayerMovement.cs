using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float walkSpeed;
    public float runSpeed;

    public float accelerationReducer;

    [DebugVariable("Player Speed")]
    float currentSpeed;

    private float inpAxisH;
    private float inpAxisV;

    private float hInputFromKeyboard;
    private float vInputFromKeyboard;
    private float arrowsInputFromKeyboard;

    public float rotationSpeed;
    private PlayerWeaponRecoilController playerRecoil;

    private CrosshairController crosshair;
   // private Rigidbody rb;

    float mX;
    float mZ;

    bool isMovingCheck;

    float dashCooldown;
    float dashCooldownLeft;
    public float dodgeStrength;
    public float dashStrength;
    public float dashLerp;

    int numberOfDashes;
    int maxNumberOfDashes;

    bool isDashing;

    float dashXDestination;
    float dashZDestination;

    float dashLerpTimeLeft = 0;

    int defaultLayer;

    public float maxArenaX;
    public float minArenaX;
    public float maxArenaZ;
    public float minArenaZ;

    public bool ClampMovement;
    public DashUpgrades dashUpgrades;
    void Start()    
    {
        InitializeInputAsset();
        isMovingCheck = false;
        playerRecoil = GetComponent<PlayerWeaponRecoilController>();
        crosshair = GetComponent<CrosshairController>();
        dashLerpTimeLeft = 0;
        numberOfDashes = maxNumberOfDashes;
        currentSpeed = walkSpeed;
        defaultLayer = this.gameObject.layer;
        dashUpgrades.RegisterPlayer(this);
       // Application.targetFrameRate = 30;
      //  rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SetStatesForCrosshair();

        CalculateInputs();
        CalculateDashCooldown();

        if (!isDashing)
        {
            mX = inpAxisH * currentSpeed * Time.deltaTime;
            mZ = inpAxisV * currentSpeed * Time.deltaTime;
        }
        else
        {
            dashLerpTimeLeft += dashLerp * Time.deltaTime;
            mX = Mathf.Lerp(mX, dashXDestination, dashLerp / Time.deltaTime);
            mZ = Mathf.Lerp(mZ, dashZDestination, dashLerp / Time.deltaTime);
            if(dashLerpTimeLeft >= 1 * Time.deltaTime)
            {
                isDashing = false;
                dashLerpTimeLeft = 0;
                this.gameObject.layer = defaultLayer;
            }
        }

        if (ClampMovement)
        {
            mX = Mathf.Clamp(mX + transform.position.x, minArenaX, maxArenaX) - transform.position.x;
            mZ = Mathf.Clamp(mZ + transform.position.z, minArenaZ, maxArenaZ) - transform.position.z;
        }

        if(arrowsInputFromKeyboard != 0)
        {
            CalculatePlayerRotation();
        }

        transform.Translate(mX, 0, mZ, Space.World);
    }
    void Sprint()
    {
        currentSpeed = runSpeed;
        playerRecoil.Sprint();
        crosshair.Sprint();
    }
    void Walk()
    {
        currentSpeed = walkSpeed;
        playerRecoil.Walk();
        crosshair.Walk();
    }

    void Dash(bool isDodging)
    {
        if (numberOfDashes > 0 && ProgressionManager.instance.GetUpgradeLevel(0) > 0)
        {
            isDashing = true;
            numberOfDashes--;

            float strenght = !isDodging ? dashStrength : dodgeStrength;
            dashXDestination = hInputFromKeyboard * strenght;
            dashZDestination = vInputFromKeyboard * strenght;

            dashCooldownLeft = dashCooldown;
            this.gameObject.layer = 11;
        }
    }


    void CalculateDashCooldown()
    {
        if(!isDashing && numberOfDashes < maxNumberOfDashes)
        {
            dashCooldownLeft -= Time.deltaTime;
            if(dashCooldownLeft <= 0)
            {
                numberOfDashes++;
                dashCooldownLeft = dashCooldown;
            }
        }
    }

    void CalculateInputs()
    {

        inpAxisH += hInputFromKeyboard / (accelerationReducer / Time.deltaTime);
        inpAxisH = Mathf.Clamp(inpAxisH, -1, 1);
        if (hInputFromKeyboard == 0)
        {
            inpAxisH = Mathf.Lerp(inpAxisH, 0, 25f * Time.deltaTime);
        }
        inpAxisV += vInputFromKeyboard / (accelerationReducer / Time.deltaTime);
        inpAxisV = Mathf.Clamp(inpAxisV, -1, 1);
        if (vInputFromKeyboard == 0)
        {
            inpAxisV = Mathf.Lerp(inpAxisV, 0, 25f * Time.deltaTime);
        }
    }

    void CalculatePlayerRotation()
    {
        transform.Rotate(new(0,arrowsInputFromKeyboard * Time.deltaTime * rotationSpeed,0));
    }

    void SetStatesForCrosshair()
    {
        if (!IsMoving() && !isDashing && isMovingCheck)
        {
            playerRecoil.StopMoving();
            crosshair.StopMoving();
            isMovingCheck = false;
        }
        else if ((IsMoving() || isDashing) && !isMovingCheck)
        {
            playerRecoil.StartMoving();
            crosshair.StartMoving();
            isMovingCheck = true;
        }
    }
    bool IsMoving() => hInputFromKeyboard != 0 || vInputFromKeyboard != 0; 

    void InitializeInputAsset()
    {
        InitializeMovementInput();

        InputSystem.actions.FindActionMap("Movement").FindAction("Sprint").performed += contex => Sprint();
        InputSystem.actions.FindActionMap("Movement").FindAction("Sprint").canceled += contex => Walk();

        InputSystem.actions.FindActionMap("Movement").FindAction("Dash").performed += context => Dash(true); //false
      //  movementInputs.FindActionMap("Player").FindAction("Dodge").performed += context => Dash(true);
    }

    void InitializeMovementInput()
    {
        InputSystem.actions.FindActionMap("Movement").FindAction("Movement").performed += contex => hInputFromKeyboard = contex.ReadValue<Vector2>().x;
        InputSystem.actions.FindActionMap("Movement").FindAction("Movement").canceled += contex => hInputFromKeyboard = contex.ReadValue<Vector2>().x;
        InputSystem.actions.FindActionMap("Movement").FindAction("Movement").performed += contex => vInputFromKeyboard = contex.ReadValue<Vector2>().y;
        InputSystem.actions.FindActionMap("Movement").FindAction("Movement").canceled += contex => vInputFromKeyboard = contex.ReadValue<Vector2>().y;

        InputSystem.actions.FindActionMap("Movement").FindAction("Arrows").performed += contex => arrowsInputFromKeyboard = contex.ReadValue<float>();
        InputSystem.actions.FindActionMap("Movement").FindAction("Arrows").canceled += contex => arrowsInputFromKeyboard = contex.ReadValue<float>();
    }

    public void SetDashData(DashData dashData)
    {
        dashCooldown = dashData.dashCooldown;
        maxNumberOfDashes = dashData.maxNumberOfDashes;
    }

    public int GetNumberOfDashes() => numberOfDashes; 
}
