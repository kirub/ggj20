using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    protected Animator CharacterAnimator;

    public float MovementSpeed { get; set; } = 5.0f;
    public float RotationSpeed { get; set; } = 0.2f;

    void Awake()
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
        if (CharacterAnimator)
        {
            CharacterAnimator.SetBool("Running", Direction > 0.0f || Direction < 0.0f);
        }

        if (Direction == 0.0f)
            return;

        transform.position = transform.position + Vector3.right * Direction * Time.deltaTime;
        
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

    void CapAngles(float Direction)
    {
        if (Direction > 0.0f)
        {
            if (transform.eulerAngles.y > 180.0f)
            {
                Vector3 Reset = transform.eulerAngles;
                Reset.y = 0.0f;
                transform.eulerAngles = Reset;
            }
        }
        else
        {
            if (transform.eulerAngles.y > 180.0f)
            {
                Vector3 Reset = transform.eulerAngles;
                Reset.y = 180.0f;
                transform.eulerAngles = Reset;
            }
        }
    }
}
