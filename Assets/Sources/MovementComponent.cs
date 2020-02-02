using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    protected Animator CharacterAnimator;
    
    public float MovementSpeed = 5.0f;
    public float RotationSpeed = 0.2f;

    void Awake()
    {
        InitMovementComponent();
    }

    protected void InitMovementComponent()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Move(float Direction)
    {
        Move(Vector3.right * Direction);
    }

    public void Move(Vector3 Target)
    {
        Target.Normalize();
        float Direction = Vector3.Dot(Target, Vector3.right) > 0.0f ? 1.0f : -1.0f;

        if (Target.sqrMagnitude == 0.0f)
        {
            CharacterAnimator.SetBool("Running", false);
            return;
        }

        if (CharacterAnimator)
        {
            CharacterAnimator.SetBool("Running", true);
        }

        transform.position = transform.position + Target * MovementSpeed * Time.deltaTime;
        
        if (Direction > 0.0f && transform.eulerAngles.y > 0.1f )
        {
            transform.Rotate(Vector3.up, -180.0f * RotationSpeed);
        }
        else if (Direction < 0.0f && transform.eulerAngles.y < 179.9f )
        {
            transform.Rotate(Vector3.up, 180.0f * RotationSpeed);
        }

        CapAngles(Direction);
    }

    public void FaceRight()
    {
        Vector3 Reset = transform.eulerAngles;
        Reset.y = 0.0f;
        transform.eulerAngles = Reset;
    }

    public void FaceLeft()
    {
        Vector3 Reset = transform.eulerAngles;
        Reset.y = 180.0f;
        transform.eulerAngles = Reset;
    }

    public void CapAngles(float Direction)
    {
        if (Direction > 0.0f)
        {
            if (transform.eulerAngles.y > 180.0f)
            {
                FaceRight();
            }
        }
        else
        {
            if (transform.eulerAngles.y > 180.0f)
            {
                FaceLeft();
            }
        }
    }
}
