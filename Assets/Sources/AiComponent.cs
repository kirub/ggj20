using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AiComponent : MovementComponent
{
    private static Vector3 InvalidTarget = new Vector3(-997, -998, -999);
    public List<Vector3> WanderingTargets = new List<Vector3>();
    public float WaitingOnSpotIntervalInSeconds = 5.0f;

    public class OnPlayerSpottedEvent : UnityEvent { }
    public OnPlayerSpottedEvent PlayerSpottedtEvent { get; } = new OnPlayerSpottedEvent();
    public class OnPlayerUnspottedEvent : UnityEvent { }
    public OnPlayerUnspottedEvent PlayerUnspottedtEvent { get; } = new OnPlayerUnspottedEvent();

    public class OnPlayerCaughtEvent : UnityEvent { }
    public OnPlayerCaughtEvent PlayerCaughtEvent { get; } = new OnPlayerCaughtEvent();

    public bool CanSeePlayer { get; private set; }
    public bool HasPlayerBeenSpotted { get; private set; }
    public bool HasPlayerBeenCaught { get; private set; }
    private List<Vector3>.Enumerator Target;
    private Vector3 CurrentTarget = InvalidTarget;
    private Vector3 NextTarget = InvalidTarget;
    private Vector3 InitialPosition;
    private float CooldownOnSpot = 0.0f;
    private UIPlayerComponent ContextualUI = null;

    public float DistanceThreshold = 0.5f;
    private float DistanceOffsetFromPlayer = 0.5f;
    public bool IsDead { get; private set; } = false;

    private void Awake()
    {
        InitPlayerCharacter();
    }

    protected void InitPlayerCharacter()
    {
        InitMovementComponent();
        ContextualUI = GetComponentInChildren<UIPlayerComponent>();
    }

    Vector3 ValidatePositions(Vector3 Position)
    {
        return new Vector3(Position.x, InitialPosition.y, Position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            CanSeePlayer = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanSeePlayer = false;
        }
    }

    public void Kill()
    {
        IsDead = true;
    }

    // Start is called before the first frame update
    void Start()
    {       
        InitialPosition = transform.position;
        Target = WanderingTargets.GetEnumerator();
        if (Target.MoveNext())
        {
            CurrentTarget = Target.Current;
        }
        if (Target.MoveNext())
        {
            NextTarget = Target.Current;
        }
    }

    void OnPlayerCaught()
    {
        HasPlayerBeenCaught = true;
        Debug.Log("Player Caught!");
        PlayerCaughtEvent.Invoke();
    }

    void OnPlayerUnspotted()
    {
        HasPlayerBeenSpotted = false;
        PlayerUnspottedtEvent.Invoke();
        Debug.Log("Player lost!");
    }

    void OnPlayerSpotted()
    {
        ResetCooldownOnSpot();

        if (!HasPlayerBeenSpotted)
        {
            Debug.Log("Player Spotted!");
            PlayerSpottedtEvent.Invoke();
            NextTarget = CurrentTarget;
            HasPlayerBeenSpotted = true;
        }

        CurrentTarget = PlayerCharacterController.Player.transform.position;
        Vector3 AIToPlayer = (CurrentTarget - NextTarget);
        AIToPlayer.Normalize();
        CurrentTarget = CurrentTarget - AIToPlayer * Vector3.Dot(new Vector3(DistanceOffsetFromPlayer, 0.0f, 0.0f), -AIToPlayer);
    }

    void ResetCooldownOnSpot()
    {
        CooldownOnSpot = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
            return;

        if (!HasPlayerBeenCaught)
        {
            if (CanSeePlayer)
            {
                bool PlayerSpotted = (PlayerCharacterController.Player.IsSpottable);
                if (PlayerSpotted)
                {
                    OnPlayerSpotted();
                }
                else if (HasPlayerBeenSpotted)
                {
                    OnPlayerUnspotted();
                }
            }
            else if (HasPlayerBeenSpotted)
            {
                OnPlayerUnspotted();
            }
        }

        if (ContextualUI)
        {
            ContextualUI.IsEnabled = HasPlayerBeenSpotted || HasPlayerBeenCaught;
        }

        if (CooldownOnSpot > 0.0f && !HasPlayerBeenSpotted)
        {
            CooldownOnSpot -= Time.deltaTime;
            if (CooldownOnSpot <= 0.0f)
            {
                ResetCooldownOnSpot();
            }
            Move(0.0f);
        }
        else if(CurrentTarget != InvalidTarget)
        {
            Vector3 TargetPos = ValidatePositions(CurrentTarget);
            if (Vector3.Distance(TargetPos, transform.position) < DistanceThreshold )
            {
                float Direction = Vector3.Dot(TargetPos, InitialPosition - TargetPos) > 0.0f ? 1.0f : -1.0f;
                CapAngles(Direction);
                if (Vector3.Distance(transform.position, PlayerCharacterController.Player.transform.position) < DistanceOffsetFromPlayer * 2)
                {
                    if (!HasPlayerBeenCaught)
                    {
                        OnPlayerCaught();
                    }
                }
                else
                {
                    CurrentTarget = NextTarget;
                    if (Target.MoveNext())
                    {
                        NextTarget = Target.Current;
                    }
                    else
                    {
                        Target = WanderingTargets.GetEnumerator();
                        if (Target.MoveNext())
                        {
                            NextTarget = Target.Current;
                        }
                        else
                        {
                            NextTarget = InvalidTarget;
                        }
                    }
                }

                CooldownOnSpot = WaitingOnSpotIntervalInSeconds;
            }

            if (!HasPlayerBeenCaught)
            {
                Move(TargetPos - transform.position);
            }
            else
            {
                Move(0.0f); // go to idle
            }
        }
    }
}
