using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public float Speed { get; set; } = 1f;
    public float RotationSpeed { get; set; } = 0.2f;

    private float Orientation { get; set; }

    private Animator PlayerAnimator;
    private Rigidbody2D RigidBody;

    void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        RigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Speed = 0.0f;
        if( Input.GetKey( KeyCode.RightArrow) )
        {
            Orientation = -180.0f;
            Speed = 5.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Orientation = 180.0f;
            Speed = -5.0f;
        }

        PlayerAnimator.SetBool("Running", Speed > 0.0f || Speed < 0.0f);
        UpdatePosition();
    }

    void UpdateOrientation()
    {
        transform.Rotate(new Vector3(0.0f, Orientation * RotationSpeed, 0.0f));
        if (Orientation > 0)
        {
            Vector3 ResetValue = transform.eulerAngles;
            ResetValue.y = Mathf.Max(ResetValue.y, 0.0f);
            transform.eulerAngles = ResetValue;
        }
        else
        {
            Vector3 ResetValue = transform.eulerAngles;
            ResetValue.y = Mathf.Min(ResetValue.y, 180.0f);
            transform.eulerAngles = ResetValue;
        }
    }

    void UpdatePosition()
    {
        transform.position = transform.position + Vector3.right * Speed * Time.deltaTime;
    }
}
