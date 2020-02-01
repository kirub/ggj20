using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MovementComponent
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float IsMoving = 0.0f;
        if( Input.GetKey( KeyCode.RightArrow) )
        {
            IsMoving = 1.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            IsMoving = -1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleMask();
        }

        Move(IsMoving);
    }

    void ToggleMask()
    {
        CharacterAnimator.SetBool("WearMask", !CharacterAnimator.GetBool("WearMask"));
    }
}
