using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MovementComponent
{
    HashSet<string> TriggeredTags = new HashSet<string>();
    public static PlayerCharacterController Player = null;

    void Awake()
    {
        InitPlayerCharacter();
    }

    protected void InitPlayerCharacter()
    {
        InitMovementComponent();
        Player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public bool IsTriggering(string TagName)
    {
        return TriggeredTags.Contains(TagName);
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D RB = GetComponent<Rigidbody2D>();
        if (RB)
        {
            //Debug.Log(RB.transform.position);
        }
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
        CharacterAnimator.SetTrigger("ToggleMask");
        CharacterAnimator.SetBool("WearMask", !CharacterAnimator.GetBool("WearMask"));
    }

    void OnTriggerEnter(Collider other)
    {
        TriggeredTags.Add(other.attachedRigidbody.gameObject.tag);

        Debug.Log("TRIGGERED!!");
        //UIElement.GetComponent<Image>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        TriggeredTags.Remove(other.attachedRigidbody.gameObject.tag);
        Debug.Log("TRIGXIT!!");
        // UIElement.GetComponent<Image>().enabled = false;
    }
}
