using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent enemyAI;
    Transform player;

    public float movementFieldOfView;
    public float rotationSpeed;

    [Header("Stamina")]

    public float maxEnemyStamina;

    public float maxStaminaRandomization;

    float stamina;

    public float outOfStaminaSpeed;

    public float outOfStaminaRotationSpeed;

    public float staminaRegenerationRate;

    public float marginOfError;

    public float rotationMarginOfError;

    bool state;

    float speed;

    Vector3 tRotation;

    bool isMoving;
    float actualSpeed;
    float actualRotationSpeed;

    Vector3 targetPosition;

    bool isDisabled;

    bool touchedPlayer;
    Decoy nearestDecoy;
    bool targetsDecoy;
    public bool isFastZombie;
    public bool isDashyZombie;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        maxEnemyStamina -= Random.Range(-maxStaminaRandomization, maxStaminaRandomization);
        stamina = maxEnemyStamina;
        state = true;
        speed = enemyAI.speed;
        tRotation = transform.localEulerAngles;
        isMoving = false;
        actualSpeed = speed;
        actualRotationSpeed = rotationSpeed;
        targetsDecoy = false;
        this.gameObject.name = "Enemy " + GetEntityId();
    }

    
    void Update()
    {
        if (!PlayerManager.instance.isDead || !isDisabled)
        {
            CalculateTarget();

            float fov = 1 - movementFieldOfView / 180;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toPlayer = targetPosition - transform.position;

            if (Vector3.Dot(forward, toPlayer) < fov)
            {
                enemyAI.speed = 0.1f;
                isMoving = false;
            }
            else if (!isMoving)
            {
                SetSpeed(actualSpeed);
                isMoving = true;
            }
            // float angularDistance = ((Vector3.Dot(forward,toPlayer)*-1) + 1.01f)/2;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(forward, toPlayer, actualRotationSpeed * Time.deltaTime, 0.1f));
        }

        CalculateStamina();
    }

    void CalculateTarget()
    {
        if(isDashyZombie)
        {
            enemyAI.destination = player.position;
            targetPosition = player.transform.position;
            return;
        }
        float distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        float nearestDecoyDistance = float.PositiveInfinity;
        Vector3 nearestDecoyPosition = Vector3.zero;
        foreach(Decoy decoy in FindObjectsByType<Decoy>())
        {
            if (decoy.IsAvailable(GetEntityId().GetHashCode()))
            {
                float distanceFromDecoy = Vector3.Distance(transform.position, decoy.transform.position);
                if (distanceFromDecoy < nearestDecoyDistance) 
                { 
                    nearestDecoyDistance = distanceFromDecoy; 
                    nearestDecoyPosition = decoy.transform.position;
                    nearestDecoy = decoy.GetComponent<Decoy>(); 
                }
            }
        }
        if(distanceFromPlayer-5 < nearestDecoyDistance) 
        { 
            enemyAI.destination = player.position;
            targetPosition = player.transform.position;
            if(targetsDecoy && nearestDecoy != null && nearestDecoy.indexOfID(GetEntityId().GetHashCode()) != -1)
            {
                nearestDecoy.UnregisterEnemy(GetEntityId().GetHashCode());
            }
            targetsDecoy = false;
        } 
        else
        { 
            enemyAI.destination = nearestDecoyPosition;
            targetPosition = nearestDecoyPosition;
            if(!targetsDecoy && nearestDecoy != null)
            {
                nearestDecoy.RegisterEnemy(GetEntityId().GetHashCode());
            }
            targetsDecoy = true;
        }
    }

    void CalculateStamina()
    {
        Vector3 rotationVelocity = transform.localEulerAngles - tRotation;
        tRotation = transform.localEulerAngles;

        if (CalculateXZVelocity(enemyAI.velocity) > marginOfError)
        {
            stamina -= Time.deltaTime;
        }
        if (Mathf.Abs(rotationVelocity.y) > rotationMarginOfError)
        {
            stamina -= Time.deltaTime * Mathf.Abs(rotationVelocity.y);
        }
        else if (stamina < maxEnemyStamina)
        {
            stamina += Time.deltaTime * staminaRegenerationRate;
        }

        if (stamina <= 0 && state)
        {
            SetSpeed(outOfStaminaSpeed);
            actualRotationSpeed = outOfStaminaRotationSpeed;
            state = false;
        }
        if (stamina >= maxEnemyStamina && !state)
        {
            SetSpeed(speed);
            actualRotationSpeed = rotationSpeed;
            state = true;
        }
    }

    public void DisableMovement()
    {
        isDisabled = true;
    }

    public void EnableMovement()
    {
        isDisabled = false;
    }

    float CalculateXZVelocity(Vector3 velocity)
    {
        return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(velocity.x), 2) + Mathf.Pow(Mathf.Abs(velocity.z), 2));
    }

    void SetSpeed(float speed)
    {
        actualSpeed = speed;
        enemyAI.speed = speed;
    }
    public void DrainStamina(float staminaToDrain)
    {
        stamina -= staminaToDrain;
    }

    public void Freeze()
    {
        if(!isFastZombie)
        {
            stamina = 0;
            SetSpeed(outOfStaminaSpeed);
            actualRotationSpeed = outOfStaminaRotationSpeed;
            state = false;
        }
    }

    public float GetStoppingDistance()
    {
        return enemyAI.stoppingDistance;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public bool IsRechargingStamina()
    {
        return !state;
    }

    public bool IsDisabled()
    {
        return isDisabled;
    }

    public bool IsOnFOV(int fieldOfView)
    {
        float fov = 1 - fieldOfView / 180;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 toPlayer = targetPosition - transform.position;

        return Vector3.Dot(forward, toPlayer) > fov;
    }
    public float GetRotationSpeed()
    {
        return actualRotationSpeed;
    }
    public void SetRotationSpeed(float speed)
    {
        actualRotationSpeed = speed;
    }

    public float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
    public bool IsFurther(float distance)
    {
        return Vector3.Distance(transform.position, player.position) > distance;
    }
    public void OnDying()
    {
        if (targetsDecoy && nearestDecoy != null)
        {
            nearestDecoy.UnregisterEnemy(GetEntityId().GetHashCode());

        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.layer == 11 && !touchedPlayer) { DrainStamina(50); touchedPlayer = true; }
    }

    private void OnCollisionExit(Collision other) 
    {
        if(other.gameObject.layer == 11 && touchedPlayer) { touchedPlayer = false; }
    }
}
