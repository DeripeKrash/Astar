using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class CircleMouvement : MonoBehaviour
{
    public GameObject origin = null;

    CharacterController controller = null;

    [SerializeField] GameObject idleObject = null;
    [SerializeField] GameObject stunObject = null;

    [SerializeField] Canvas speedCanvas = null;
    [SerializeField] Canvas slownCanvas = null;


    [Header("Control parameters")]
    [SerializeField] float speed = 35;
    [SerializeField] float maxSpeed = 35;
    [SerializeField] [Range(0, 1)] float maxSpeedSlowingFactor = 0.99f;
    [SerializeField] float jump = 35;
    [SerializeField] float gravity = 85;
    [SerializeField] [Range(0, 1)] float verticalInertiaDown = 0.5f;
    [SerializeField] [Range(0, 1)] float verticalInertiaUp = 0.5f;

    [SerializeField] int nbJump = 2;
    [SerializeField] [Range(0, 1)] float slideOnTurn = 0.5f;
    [SerializeField] [Range(0, 1)] float aerialControl = 0.5f;
    [SerializeField] [Range(0, 1)] float slowingFactorOnGround = 0.5f;
    [SerializeField] [Range(0, 1)] float slowingFactorInAir = 0.5f;

    [SerializeField] Vector2 wallJump = new Vector2(50,50);

    [System.NonSerialized] public bool isGrounded = false;

    [SerializeField] float coyoteTime = 0.25f;
    float saveCoyoteTime = 0f;
    [SerializeField] float cooldownJump = 0.1f;
    int usedJump = 0;
    float savedTime = 0.0f;

    [SerializeField] float centerAttractionDivider = 10;

    Vector3 oldPosition = new Vector3();
    [SerializeField] Vector2 velocity = new Vector3(0, 0);

    Vector2 factorSpeed = new Vector2(1,1);
    float startSlowingTime = 0;
    float slowingDuration = 0;
    [System.NonSerialized] public bool isSlowed = false;
    float slowMotionTimer = 0;

    [Space]
    [Header("Dash parameters")]
    [SerializeField] [Range(2, 36)] int numberDirectionDash = 2;

    [SerializeField] public int nbDash = 1;
    [System.NonSerialized] public int usedDash = 0;
    [SerializeField] float dashCooldown = 0.5f;
    [SerializeField] Vector2 dashPower = new Vector2();
    [SerializeField] float dashDuration = 0.1f;
    [System.NonSerialized] public bool isDash = false;

    float startTimeDash = 0;
    Vector2 dash = new Vector2(0, 0);

    [Space]
    [Header("Teleportation parameters")]
    [SerializeField] float Teleportationcooldown = 0.5f;
    float startTimeTeleportation = 0f;

    [Space]
    [Header("Stun parameters")]
    [SerializeField] float stunDuration = 10;
    [SerializeField] Vector2 factorStun = new Vector2();
    float stunStart = 0;
    [System.NonSerialized] public bool isStuned = false;
    [SerializeField] float inviciblityDuration = 0;
    float inviciblityStart = 0;
    bool isInvincible = false;

    [Space]
    [Header("Bumper parameters")]
    [SerializeField] float startBumperTime = 0;
    [SerializeField] float bumperDuration = 1;
    [SerializeField] [Range(0, 1)] float minimalBumperFactor = 0.5f;

    float inputDirection = 0;
    float velocityDirection = 0;


    public delegate void PlayerEvent();
    
    public event PlayerEvent JumpEvent;
    public event PlayerEvent DashEvent;
    public event PlayerEvent TeleportEvent;
    public event PlayerEvent BumperEvent;
    
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        controller = GetComponent<CharacterController>();

        if (slownCanvas != null)
        {
            slownCanvas.enabled = false;
        }

        if (speedCanvas != null)
        {
            speedCanvas.enabled = false;
        }

        
        JumpEvent += OnJump;

        SpeedFactor(Vector2.zero, 1);

        SwitchParticuleForStun(true);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayerInputManager.PS4Input = !PlayerInputManager.PS4Input;
        }

        controller.Move(Vector3.zero);

        if (origin == null)
            return;

        if (Time.timeScale < 1 && Time.timeScale != 0)
        {
            if (slownCanvas != null)
            {
                slownCanvas.enabled = true;
            }
        }
        else
        {
                      
            if (slownCanvas != null)
            {
                slownCanvas.enabled = false;
            }
        }

        if (isSlowed)
        {
            if (Time.timeScale != 0)
                slowMotionTimer += Time.unscaledDeltaTime;

            if (slowMotionTimer >= slowingDuration)
            {
                if (speedCanvas != null)
                {

                    speedCanvas.enabled = false;
                }

                isSlowed = false;
                factorSpeed.x = 1;
                factorSpeed.y = 1;
            }
        }

        PlayerInvicible();
        PlayerStuned();

        if (Dash())
            return;

        VerticalControl();
        HorizontalControl();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Move>() != null || other.gameObject.GetComponent<Ammunition>() != null)
        {
            if (!isInvincible)
            {
                if (other.CompareTag("Enemy"))
                {
                    if (!other.GetComponent<Enemy>().isDead)
                    {
                        other.GetComponent<Enemy>().OnFusRoDah(other, controller);
                        inviciblityStart = Time.realtimeSinceStartup;
                        isInvincible = true;
                        StunPlayer();
                    }
                }
                else
                {
                    Physics.IgnoreCollision(controller, other.gameObject.GetComponent<Collider>());
                    inviciblityStart = Time.realtimeSinceStartup;
                    isInvincible = true;
                    StunPlayer();
                }
            }
            else
            {
                if (other.CompareTag("Enemy"))
                    other.GetComponent<Enemy>().OnFusRoDah(other, controller);
                else
                    Destroy(other.gameObject);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 posNormal = (new Vector3(transform.position.x - origin.transform.position.x, 0, transform.position.z - origin.transform.position.z)).normalized;

        if ((hit.normal.normalized - posNormal).magnitude >= 0.05 && Mathf.Abs(hit.normal.y) <= 0.1)
        {

            if(hit.gameObject.CompareTag("WallJump") && PlayerInputManager.JumpInput() && !controller.isGrounded)
            {
                velocity.x = 0;
                velocity.x = -1 * wallJump.x * velocityDirection;
                
                velocity.y = 0;
                velocity.y = wallJump.y;
            }
            else if (controller.velocity.x == 0 && controller.velocity.z == 0)
            {
               velocity.x = 0;
               startBumperTime -= bumperDuration;
            }
        }
        else if (velocity.y > 0 && hit.collider.transform.position.y > transform.position.y)
        {
            if (Mathf.Abs(hit.normal.y) >= 0.1)
            {
                velocity.y = 0;
            }
        }
    }

    private void VerticalControl()
    {
        controller.Move(new Vector3(0, velocity.y * factorSpeed.y, 0) * Time.unscaledDeltaTime);

        if (controller.isGrounded)
        {
            usedDash = 0;
            saveCoyoteTime = 0f;
            usedJump = 0;
            velocity.y = -0.1f;
            if (PlayerInputManager.JumpInput() && !isStuned)
            {
                JumpEvent?.Invoke();
                velocity.y = jump;
                usedJump++;
                savedTime = Time.realtimeSinceStartup;
            }
        }
        else
        {
            if (usedJump == 0 && saveCoyoteTime == 0f)
            {
                saveCoyoteTime = Time.realtimeSinceStartup;
            }
            else if (usedJump == 0 && Time.realtimeSinceStartup - saveCoyoteTime > coyoteTime)
            {
                usedJump = 1;
            }
            velocity.y -= gravity * factorSpeed.y * Time.unscaledDeltaTime;

            if (PlayerInputManager.JumpInputDown() && usedJump < nbJump && !isStuned && Time.realtimeSinceStartup - savedTime > cooldownJump)
            {
                if (velocity.y < 0)
                    velocity.y *= verticalInertiaDown;
                else if (velocity.y > 0)
                    velocity.y *= verticalInertiaUp;
                JumpEvent?.Invoke();
                velocity.y += jump;
                usedJump++;
                savedTime = Time.realtimeSinceStartup;
            }
        }
        isGrounded = controller.isGrounded;
    }

    private void HorizontalControl()
    {
        // calculate velocity

        float aerialInertiaNoInput = slowingFactorInAir;
        float groundInertiaNoInput = slowingFactorOnGround;
        float aerialInertiaInput = aerialControl;
        float groundInertiaInput = slideOnTurn;

        if (Time.realtimeSinceStartup - startBumperTime <= bumperDuration)
        {
            float InertiaFactor = 1 - ((Time.realtimeSinceStartup - startBumperTime) / bumperDuration);
            aerialInertiaNoInput += (1 - slowingFactorInAir) * InertiaFactor;
            groundInertiaNoInput += (1 - slowingFactorOnGround) * InertiaFactor;
            aerialInertiaInput += (1 - aerialControl) * InertiaFactor;
            groundInertiaInput += (1 - slideOnTurn) * InertiaFactor;
            if (InertiaFactor < minimalBumperFactor)
            {
                startBumperTime -= bumperDuration;
            }
        }
        inputDirection = Input.GetAxis("Horizontal");

        if (inputDirection != 0)
        {
            inputDirection /= Mathf.Abs(inputDirection);

            if (controller.isGrounded)
            {
                if (velocity.x * inputDirection > 0)
                    velocity.x *= (groundInertiaInput + Time.unscaledDeltaTime) * (1 - Time.unscaledDeltaTime);
                if (Mathf.Abs(velocity.x) <= maxSpeed)
                    velocity.x -= factorSpeed.x * inputDirection * speed * Time.unscaledDeltaTime;
            }
            else
            {
                if (velocity.x * inputDirection > 0)
                {
                    velocity.x *= (aerialInertiaInput + Time.unscaledDeltaTime) * (1 - Time.unscaledDeltaTime);
                }
                    if (Mathf.Abs(velocity.x) <= maxSpeed)
                    velocity.x -= factorSpeed.x * inputDirection * speed * aerialInertiaInput * Time.unscaledDeltaTime;
            }

            if (Mathf.Abs(velocity.x) > maxSpeed)
            {
                velocity.x *= 1 - (maxSpeedSlowingFactor * Time.unscaledDeltaTime);
            }
        }
        else if (velocity.x != 0)
        {
            controller.Move(new Vector2(0, -0.01f));

            if (controller.isGrounded)
            {
                velocity.x *= (groundInertiaNoInput + Time.unscaledDeltaTime) * (1 - Time.unscaledDeltaTime);                
            }
            else
            {
                velocity.x *= (aerialInertiaNoInput + Time.unscaledDeltaTime) * (1 - Time.unscaledDeltaTime);
            }
        }

        // Apply Velocity

        oldPosition = transform.position;

        velocityDirection = velocity.x;
        if (velocityDirection != 0)
            velocityDirection /= Mathf.Abs(velocityDirection);

        transform.RotateAround(new Vector3(origin.transform.position.x, transform.position.y, origin.transform.position.z), Vector3.up, velocityDirection * Time.unscaledDeltaTime);

        if (transform.position != oldPosition)
        {
            if (inputDirection == velocityDirection)
                transform.forward = - (transform.position - oldPosition).normalized;
            else
                transform.forward = (transform.position - oldPosition).normalized;
        }
        controller.Move((transform.position - oldPosition).normalized * ((velocity.x * factorSpeed.x) * velocityDirection) * Time.unscaledDeltaTime);
        controller.Move((new Vector3(origin.transform.position.x - transform.position.x, 0, origin.transform.position.z - transform.position.z).normalized * (Mathf.Abs(velocity.x) / centerAttractionDivider) * Time.unscaledDeltaTime));
    }

    private bool Dash()
    {
        // start Dash
        if (Input.GetButtonDown("Dash") && Time.realtimeSinceStartup - startTimeDash > dashCooldown && usedDash < nbDash && !isStuned)
        {
            Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (inputVector.sqrMagnitude == 0)
            {
                return false;
            }

            DashEvent?.Invoke();

            isDash = true;

            OnDash();

            usedDash++;

            dash.x = 0;
            dash.y = 0;

            if (numberDirectionDash == 2)
            {
                if (inputVector.x != 0)
                    dash.x = -(inputVector.x / Mathf.Abs(inputVector.x)) * dashPower.x;
            }
            else if (numberDirectionDash == 4)
            {
                if (Mathf.Abs(inputVector.x) >= Mathf.Abs(inputVector.y))
                {
                    if (inputVector.x != 0)
                        dash.x = -(inputVector.x / Mathf.Abs(inputVector.x)) * dashPower.x;
                }
                else
                {
                    if (inputVector.y != 0)
                        dash.y = (inputVector.y / Mathf.Abs(inputVector.y)) * dashPower.y;
                }
            }
            else if (numberDirectionDash == 8)
            {
                if (inputVector.x != 0)
                    dash.x = -(inputVector.x / Mathf.Abs(inputVector.x)) * dashPower.x;
                if (inputVector.y != 0)
                    dash.y = (inputVector.y / Mathf.Abs(inputVector.y)) * dashPower.y;
            }
            else
            {
                dash.x = -(inputVector.x);
                dash.y = (inputVector.y);

                dash.Normalize();
                dash.x *= dashPower.x;
                dash.y *= dashPower.y;

            }
            velocity = dash;
            velocityDirection = velocity.x;
            if (velocityDirection != 0)
                velocityDirection /= Mathf.Abs(velocityDirection);
            startTimeDash = Time.realtimeSinceStartup;
        }

        // Apply Start

        if (Time.realtimeSinceStartup - startTimeDash < dashDuration)
        {
            controller.Move(new Vector3(0, dash.y * factorSpeed.y, 0) * Time.unscaledDeltaTime);

            if (dash.x == 0)
                return true;


            float direction = dash.x / Mathf.Abs(dash.x);

            oldPosition = transform.position;

            transform.RotateAround(new Vector3(origin.transform.position.x, transform.position.y, origin.transform.position.z), Vector3.up, direction * Mathf.Abs(dash.x) * Time.unscaledDeltaTime);

            controller.Move((transform.position - oldPosition).normalized * ((dash.x * (Mathf.PI / 2)) * direction * factorSpeed.x) * Time.unscaledDeltaTime
                + (new Vector3(origin.transform.position.x, transform.position.y, origin.transform.position.z) - transform.position).normalized * (dash.x * direction) * Time.unscaledDeltaTime);

            return true;
        }

        isDash = false;

        return false;
    }

    void StunPlayer()
    {
        isStuned = true;
        stunStart = Time.realtimeSinceStartup;
        SwitchParticuleForStun(false);
    }

    void PlayerStuned()
    {
        if (isStuned && Time.realtimeSinceStartup - stunStart < stunDuration)
        {
            SpeedFactor(factorStun, stunDuration);
        }
        else if (isStuned)
        {

            SwitchParticuleForStun(true);

            SpeedFactor(Vector2.zero, 0);
            isStuned = false;
        }
    }

    void PlayerInvicible()
    {
        if (isInvincible && Time.realtimeSinceStartup - inviciblityStart > inviciblityDuration)
        {
            isInvincible = false;
        }
    }

    public bool SpeedFactor(Vector2 _factorSpeed, float factorDuration, bool checkGrounded = false)
    {
        if (checkGrounded && isGrounded)
        {
            return true;
        }

        if (!isStuned && _factorSpeed.sqrMagnitude < factorSpeed.sqrMagnitude)
        {
            if (_factorSpeed == Vector2.zero)
            {
                return isGrounded;
            }
        }

        if (speedCanvas != null && _factorSpeed.x > 1)
        {
            speedCanvas.enabled = true;
        }

        isSlowed = true;
        factorSpeed = _factorSpeed;
        startSlowingTime = Time.realtimeSinceStartup;
        slowingDuration = factorDuration;
        slowMotionTimer = 0;

        return isGrounded;
    }

    public void Teleport(Vector3 newPosition, bool stuckOnY = false)
    {
        if (Time.realtimeSinceStartup - startTimeTeleportation >= Teleportationcooldown)
        { 
            TeleportEvent?.Invoke();
            startTimeTeleportation = Time.realtimeSinceStartup;
            controller.enabled = false;
            if (stuckOnY)
                transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            else
                transform.position = newPosition;
            controller.enabled = true;

            OnTeleport();
        }
    }

    public void Bumper(Vector2 direction, float duration, float minimalFactor, bool keepSpeed = true)
    {
        BumperEvent?.Invoke();
        usedJump = 1;
        AddDash(1,1);
        if (keepSpeed)
        {
            velocity.x += direction.x;
            velocity.y = direction.y;
        }
        else
            velocity = direction;

        bumperDuration = duration;
        minimalBumperFactor = minimalFactor;

        startBumperTime = Time.realtimeSinceStartup;

        velocityDirection = velocity.x;
        if (velocityDirection != 0)
            velocityDirection /= Mathf.Abs(velocityDirection);

        startTimeDash -= dashDuration;
        OnBumper();
    }

    public bool AddDash(int addDash, int minValue)
    {
        if (usedDash == 0 || usedDash <= minValue)
        {
            return false;
        }
        usedDash -= addDash;
        return true;
    }

    public bool AddJump(int addJump, int minValue)
    {
        if (usedJump == 0 || usedJump <= minValue)
        {
            return false;
        }
        usedJump -= addJump;
        return true;
    }

    void SwitchParticuleForStun(bool idle)
    {
        if (stunObject != null || idleObject != null)
        {
            idleObject.SetActive(idle);
            stunObject.SetActive(!idle);
        } 
    }

    void OnJump()
    {
        SoundManager.instance.PlaySingle(SoundAssets.instance.playerJump, 0.25f);
    }

    void OnDash()
    {
        if (usedDash == 0)
        {
            SoundManager.instance.PlaySingle(SoundAssets.instance.firstPlayerDash, 0.25f);
        }
        else if (usedDash == 1)
        {
            SoundManager.instance.PlaySingle(SoundAssets.instance.secondPlayerDash, 0.25f);
        }
    }

    void OnTeleport()
    {
        SoundManager.instance.PlaySingle(SoundAssets.instance.teleportation);
    }

    void OnBumper()
    {

    }



    public Vector3 VectorFromOrigin(Vector3 obj)
    {
        return new Vector3(obj.x - origin.transform.position.x, 0, obj.z - origin.transform.position.z);
    }

}
