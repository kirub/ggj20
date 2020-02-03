using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    public List<Item> Items { get; private set; } = new List<Item>();
    private int CurrentItemIndex = -1;
    public bool IsWearing { get { return ItemWeared != null; } }
    public Item ItemWeared { get; private set; } = null;
    public Item CurrentItem
    {
        get
        {
            if (CurrentItemIndex < 0)
                return null;
            if (Items.Count < CurrentItemIndex)
                return null;
            
            return Items[CurrentItemIndex];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextItem()
    {
        CurrentItemIndex = (CurrentItemIndex + 1) % Items.Count;
        Debug.Log(CurrentItem.Name);
    }

    public void ToggleCurrentItem( Animator CharacterAnimator = null)
    {
        Item ItemToToggle = null;
        if (IsWearing)
        {
            ItemToToggle = ItemWeared;
            ItemWeared = null;
        }
        else
        {
            ItemWeared = CurrentItem;
            ItemToToggle = ItemWeared;
        }

        if (CharacterAnimator)
        {
            if (ItemToToggle != null)
            {
                String TriggerName = String.Format("Toggle{0}", ItemToToggle.Name);
                CharacterAnimator.SetTrigger(TriggerName);
            }
        }
    }

    public void AddItem(Item.EType InType)
    {
        Item NewItem = new Item(InType);
        AddItem(NewItem);
        Debug.Log(NewItem.Name);
    }

    public void AddItem(Item InItem)
    {
        if(!Items.Contains(InItem))
        {
            Items.Add(InItem);
            if( CurrentItem == null)
            {
                NextItem();
            }
        }
    }

    public void RemoveItem(Item.EType InType)
    {
        int Index = Items.FindIndex(Element => Element.Type == InType);
        if (Index != -1)
        {
            Items.RemoveAt(Index);
        }
    }
}
