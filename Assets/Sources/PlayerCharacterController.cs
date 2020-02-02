using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MovementComponent
{
    HashSet<string> TriggeredTags = new HashSet<string>();
    public static PlayerCharacterController Player = null;
    public bool IsSpottable { get; set; } = true;
    public bool IsWearingMask { get; set; } = false;
    public float killDistance = 2.0f;
    public Sprite reviveSprite;
    public Sprite hidingSprite;
    public Sprite killingSprite;

    private UIPlayerComponent ContextualUI = null;
    private Image image = null;
    private GameObject closestNPC;
    private GameObject[] NPC;

    void Awake()
    {
        InitPlayerCharacter();
    }

    protected void InitPlayerCharacter()
    {
        InitMovementComponent();
        Player = this;
        ContextualUI = GetComponentInChildren<UIPlayerComponent>();
        image = ContextualUI.GetComponentInChildren<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        NPC = GameObject.FindGameObjectsWithTag("AI");
        closestNPC = NPC[0];
    }

    public bool IsTriggering(string TagName)
    {
        return TriggeredTags.Contains(TagName);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject npc in NPC)
        {
            if (Vector3.Distance(npc.transform.position, transform.position) < Vector3.Distance(closestNPC.transform.position, transform.position))
            {
                closestNPC = npc;
            }
        }

        bool IsAtKillingDistance = Vector3.Distance(closestNPC.transform.position, transform.position) < killDistance && !IsTriggering("AI");

        float IsMoving = 0.0f;
        if( Input.GetKey( KeyCode.RightArrow) )
        {
            IsMoving = 1.0f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            IsMoving = -1.0f;
        }

        // ContextualUI
        if (IsTriggering("Corpse"))
        {
            ContextualUI.IsEnabled = true;
            image.sprite = reviveSprite;
        }
        else if(IsAtKillingDistance)
        {
            ContextualUI.IsEnabled = true;
            image.sprite = killingSprite;
        }
        else
        {
            // Default Behavior
            ContextualUI.IsEnabled = IsTriggering("ContextualUI");
            image.sprite = hidingSprite;
        }

        // Actions
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (IsTriggering("Corpse"))
            {
                GetComponent<ReviveComponent>().Revive();
            }
            else if (IsAtKillingDistance)
            {
                // Kill Someone
            }
            else
            {
                ToggleMask();
            }
        }

        Move(IsMoving);
    }

    void ToggleMask()
    {
        IsWearingMask = !IsWearingMask;
        CharacterAnimator.SetTrigger("ToggleMask");
        IsSpottable = IsWearingMask;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            TriggeredTags.Add(other.attachedRigidbody.gameObject.tag);
        }

        Debug.Log("TRIGGERED!!");
        //UIElement.GetComponent<Image>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            TriggeredTags.Remove(other.attachedRigidbody.gameObject.tag);
        }
        Debug.Log("TRIGXIT!!");
        // UIElement.GetComponent<Image>().enabled = false;
    }
}
