using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerComponent : MonoBehaviour
{
    private Image UIElement;

    public bool IsEnabled { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        UIElement = GetComponentInChildren<Image>();
        UIElement.enabled = IsEnabled; 
    }

    // Update is called once per frame
    void Update()
    {
        if(UIElement.enabled != IsEnabled)
        {
            UIElement.enabled = IsEnabled;
        }
    }
}
