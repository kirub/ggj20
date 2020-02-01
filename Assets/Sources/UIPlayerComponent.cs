using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerComponent : MonoBehaviour
{
    public GameObject UIElement;

    bool IsEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        UIElement.GetComponent<Image>().enabled = IsEnabled; 
    }

    // Update is called once per frame
    void Update()
    {
        bool WasEnabled = IsEnabled;
        IsEnabled = PlayerCharacterController.Player.IsTriggering("ContextualUI");

        if(WasEnabled != IsEnabled)
        {
            UIElement.GetComponent<Image>().enabled = IsEnabled;
        }
    }
}
