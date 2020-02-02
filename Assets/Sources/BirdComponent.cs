using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdComponent : MovementComponent
{
    public float DistanceToFlyAway = 2.0f;

    private Animator Anim = null;
    private Vector3 TargetPos;
    private bool IsFlyingAway = false; 

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsFlyingAway && Vector3.Distance(PlayerCharacterController.Player.transform.position, transform.position) < DistanceToFlyAway)
        {
            float Direction = Vector3.Dot(transform.position, PlayerCharacterController.Player.transform.position - transform.position) > 0.0f ? 1.0f : -1.0f;
            if(Anim)
            {
                Anim.SetTrigger("Fly");
            }

            TargetPos = Vector3.up * 10.0f + Vector3.right * Direction * 4.0f;
            transform.localScale = new Vector3(transform.localScale.x * -Direction, transform.localScale.y, transform.localScale.z);
            IsFlyingAway = true;
        }

        if (IsFlyingAway)
        {
            Move(TargetPos);
        }
    }


}
