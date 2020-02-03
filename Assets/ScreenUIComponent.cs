using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUIComponent : MonoBehaviour
{
    public static ScreenUIComponent HUD = null; 
    public SoulGaugeBehaviour SoulGauge { get; private set; } = null;

    private void Awake()
    {
        HUD = this;
        SoulGauge = GetComponentInChildren<SoulGaugeBehaviour>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
