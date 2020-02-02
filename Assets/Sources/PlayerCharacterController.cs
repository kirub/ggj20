using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MovementComponent
{
    HashSet<string> TriggeredTags = new HashSet<string>();
    public static PlayerCharacterController Player = null;
    public bool IsSpottable { get; set; } = true;
    public bool IsWearingMask { get; set; } = false;

    private UIPlayerComponent ContextualUI = null;
    private SpriteRenderer SpriteRender = null;
    private GameObject ContextualUIObject = null;
    private int InitialSortingOrder = -1;
    private float InitialZPosition = 0.0f;

    void Awake()
    {
        InitPlayerCharacter();
    }

    protected void InitPlayerCharacter()
    {
        InitMovementComponent();
        Player = this;
        ContextualUI = GetComponentInChildren<UIPlayerComponent>();
        SpriteRender = GetComponentInChildren<SpriteRenderer>();
        InitialSortingOrder = SpriteRender.sortingOrder;
        InitialZPosition = transform.position.z;
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
            if (IsTriggering("ContextualUI"))
            {
                ToggleHide();
            }
            else if (IsTriggering("Corpse"))
            {
                ToggleHide();
            }
            else
            {
                ToggleMask();
            }
        }

        if (IsTriggering("Revive"))
        {   
            // display UI for reviving
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<ReviveComponent>().Revive();
            }
        }

        ContextualUI.IsEnabled = IsTriggering("ContextualUI");

        Move(IsMoving);
    }

    void ResetHide()
    {
        IsSpottable = true;
        SpriteRender.sortingOrder = InitialSortingOrder;
        transform.position = new Vector3(transform.position.x, transform.position.y, InitialZPosition);
    }

    void ToggleHide()
    {
        if (SpriteRender.sortingOrder != InitialSortingOrder)
        {
            ResetHide();
        }
        else if(ContextualUIObject)
        {
            IsSpottable = false;
            SpriteRenderer CollidingObjSprite = ContextualUIObject.GetComponent<SpriteRenderer>();
            if (CollidingObjSprite)
            {
                SpriteRender.sortingOrder = CollidingObjSprite.sortingOrder - 1;
                transform.position = new Vector3(transform.position.x, transform.position.y, ContextualUIObject.transform.position.z - 0.1f);
            }
        }
        else
        {
            Debug.LogWarning("No Contextual Object!");
        }
    }


    void ToggleMask()
    {
        IsWearingMask = !IsWearingMask;
        CharacterAnimator.SetTrigger("ToggleMask");
        IsSpottable = !IsWearingMask;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            TriggeredTags.Add(other.attachedRigidbody.gameObject.tag);
            if(other.CompareTag("ContextualUI"))
            {
                ContextualUIObject = other.gameObject;
            }
        }

        Debug.Log("TRIGGERED!!");
        //UIElement.GetComponent<Image>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            TriggeredTags.Remove(other.attachedRigidbody.gameObject.tag);
            if(other.CompareTag("ContextualUI"))
            {
                ContextualUIObject = null;
                ResetHide();
            }
        }
        Debug.Log("TRIGXIT!!");
        // UIElement.GetComponent<Image>().enabled = false;
    }
}
