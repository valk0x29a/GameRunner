using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyMovement))]
public class DashyZombieAdditionalAbilities : MonoBehaviour
{
    EnemyHealth health;
    EnemyMovement movement;

    public float dashCooldown;
    float dashCooldownLeft;
    public float dashStrenght;
    public float dashLerp;
    public float dashEnergyDrain;

    int numberOfDashes;
    public int maxNumberOfDashes;

    bool isDashing;

    float dashLerpTimeLeft = 0;

    float mX;
    float mZ;

    float dashXDestination;
    float dashZDestination;

    int defaultLayer;

    public float dashTimer;
    float dashTimerLeft;
    public float dashTimerRange;

    bool canDash;

    public int dashFOV;
    public float minDashDistance;

    void Start()
    {
        health = GetComponent<EnemyHealth>();
        health.lastHitSpecial = DashX;

        movement = GetComponent<EnemyMovement>();

        dashLerpTimeLeft = 0;
        numberOfDashes = maxNumberOfDashes;
        defaultLayer = this.gameObject.layer;
        dashTimerLeft = Random.Range(dashTimer - dashTimerRange, dashTimer + dashTimerRange);
        canDash = false;
    }


    void Update()
    {
        CalculateTimer();
        CheckForDashes();
        CalculateDashCooldown();
        if(isDashing)
        {
            dashLerpTimeLeft += Time.deltaTime * dashLerp;
            mX = Mathf.Lerp(mX, dashXDestination, dashLerp * Time.deltaTime);
            mZ = Mathf.Lerp(mZ, dashZDestination, dashLerp * Time.deltaTime);
            transform.Translate(mX, 0,mZ, Space.Self);
            if (dashLerpTimeLeft >= 1)
            {
                movement.SetRotationSpeed(movement.GetRotationSpeed() / 10);
                isDashing = false;
                dashLerpTimeLeft = 0;
                movement.EnableMovement();
                this.gameObject.layer = defaultLayer;
            }
        }
       
    }

    void CalculateTimer()
    {
        if(!canDash)
        {
            dashTimerLeft -= Time.deltaTime;
            if(dashTimerLeft <= 0)
            {
                canDash = true;
                dashTimerLeft = Random.Range(dashTimer - dashTimerRange, dashTimer + dashTimerRange);
            }
        }
    }

    void CheckForDashes()
    {
        if(canDash)
        {
            if(movement.IsOnFOV(dashFOV))//&& movement.isFurther(minDashDistance))
            {
                DashForward();
            }
                  
        }
    }

    void CalculateDashCooldown()
    {
        if (!isDashing && numberOfDashes < maxNumberOfDashes)
        {
            dashCooldownLeft -= Time.deltaTime;
            if (dashCooldownLeft <= 0)
            {
                numberOfDashes++;
                dashCooldownLeft = dashCooldown;
            }
        }
    }

    void DashX()
    {
        if (numberOfDashes > 0 && !movement.IsRechargingStamina())
        {
            movement.SetRotationSpeed(movement.GetRotationSpeed() * 10);
            int dashDirection = 0;
            while (dashDirection == 0)
            {
                dashDirection = (int)Random.Range(-1, 1);
            }
            isDashing = true;
            movement.DrainStamina(dashEnergyDrain);
            numberOfDashes--;
            dashXDestination = dashDirection * dashStrenght * Time.deltaTime;
            dashZDestination = 0;
            dashCooldownLeft = dashCooldown;
            movement.DisableMovement();
            this.gameObject.layer = 11;
        }
    }

    void DashForward()
    {
        if (numberOfDashes > 0 && !movement.IsRechargingStamina())
        {
            movement.SetRotationSpeed(movement.GetRotationSpeed() * 10);
            canDash = false;
            int dashDirection = 1;
            isDashing = true;
            movement.DrainStamina(dashEnergyDrain);
            numberOfDashes--;
            float dashStrenghtTemp = dashStrenght * Mathf.Clamp(movement.GetDistanceFromPlayer() / 7.5f, 0.1f, 1);
            dashZDestination = dashDirection * dashStrenghtTemp * Time.deltaTime;
            dashXDestination = 0;
            dashCooldownLeft = dashCooldown;
            movement.DisableMovement();
            this.gameObject.layer = 11;
        }
    }
}
