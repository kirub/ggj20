using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerComponent : MonoBehaviour
{
    public GameObject UIElement;

    public bool IsEnabled { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        UIElement.GetComponent<Image>().enabled = IsEnabled; 
    }

    // Update is called once per frame
    void Update()
    {
        if(UIElement.GetComponent<Image>().enabled != IsEnabled)
        {
            UIElement.GetComponent<Image>().enabled = IsEnabled;
        }
    }
}
