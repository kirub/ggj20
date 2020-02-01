using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiComponent : MovementComponent
{
    public List<Vector3> WanderingTargets = new List<Vector3>();
    public float DistanceThreshold = 1.0f;
    public float WaitingOnSpotIntervalInSeconds = 5.0f; 

    private List<Vector3>.Enumerator Target;
    private Vector3 InitialPosition;
    private float CooldownOnSpot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {       
        InitialPosition = transform.position;
        Target = WanderingTargets.GetEnumerator();
        Target.MoveNext();
    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownOnSpot > 0.0f)
        {
            CooldownOnSpot -= Time.deltaTime;
            if (CooldownOnSpot <= 0.0f)
            {
                CooldownOnSpot = 0.0f;
            }
            Move(0.0f);
        }
        else
        {
            if (Vector3.Distance(Target.Current, transform.position) < DistanceThreshold)
            {
                if (!Target.MoveNext())
                {
                    Target = WanderingTargets.GetEnumerator();
                    Target.MoveNext();
                }

                CooldownOnSpot = WaitingOnSpotIntervalInSeconds;
            }
            Move(Target.Current - transform.position);
        }
    }
}
