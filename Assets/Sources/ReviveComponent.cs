using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveComponent : MonoBehaviour
{
    public float timeToRevive = 5.0f;
    private float timer = -1.0f;
    private bool isRevived = false;
    private bool isReviving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isReviving && !isRevived)
        {
            Debug.Log("Now reviving. Please wait...");
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                timer = -1.0f;
                isRevived = true;
                isReviving = false;
                Debug.Log("Revive Successful"); // TODO: substitute with thing that happens
            }
        }
    }
    public void Revive()
    {
        // create a timer for the revive
        timer = timeToRevive;
        isReviving = true;
    }
}
