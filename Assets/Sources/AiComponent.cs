using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiComponent : MovementComponent
{
    private static Vector3 InvalidTarget = new Vector3(-997, -998, -999);
    public List<Vector3> WanderingTargets = new List<Vector3>();
    public float DistanceThreshold = 1.0f;
    public float WaitingOnSpotIntervalInSeconds = 5.0f; 

    private List<Vector3>.Enumerator Target;
    private Vector3 CurrentTarget = InvalidTarget;
    private Vector3 NextTarget = InvalidTarget;
    private Vector3 InitialPosition;
    private float CooldownOnSpot = 0.0f;
    private bool PlayerSpotted = false;
    private UIPlayerComponent ContextualUI = null;

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
            PlayerSpotted = (PlayerCharacterController.Player.IsSpottable);
            if(PlayerSpotted)
            {
                WhenPlayerSpotted();
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerSpotted = false;
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

    void WhenPlayerSpotted()
    {
        NextTarget = CurrentTarget;
        CurrentTarget = PlayerCharacterController.Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(ContextualUI)
            ContextualUI.IsEnabled = PlayerSpotted;

        if (CooldownOnSpot > 0.0f && !PlayerSpotted)
        {
            CooldownOnSpot -= Time.deltaTime;
            if (CooldownOnSpot <= 0.0f)
            {
                CooldownOnSpot = 0.0f;
            }
            Move(0.0f);
        }
        else if(CurrentTarget != InvalidTarget)
        {
            Vector3 TargetPos = ValidatePositions(CurrentTarget);
            if (Vector3.Distance(TargetPos, transform.position) < DistanceThreshold)
            {
                CurrentTarget = NextTarget;
                if (Target.MoveNext())
                {
                    NextTarget = Target.Current;
                }
                else
                {
                    Target = WanderingTargets.GetEnumerator();
                    if( Target.MoveNext() )
                    {
                        NextTarget = Target.Current;
                    }
                    else
                    {
                        NextTarget = InvalidTarget;
                    }
                }

                CooldownOnSpot = WaitingOnSpotIntervalInSeconds;
            }
            Move(TargetPos - transform.position);
        }
    }
}
