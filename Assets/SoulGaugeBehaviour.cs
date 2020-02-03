using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGaugeBehaviour : MonoBehaviour
{
    private const int MaxSoulValue = 3;
    private int CurrentSoulValue = MaxSoulValue;
    private Animator[] UISoulAnims = null;

    private void Awake()
    {
        UISoulAnims = GetComponentsInChildren<Animator>();
        SetSoulsCount(3);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelaySetActiveFalse(int Idx)
    {
        yield return new WaitForSeconds(0.2f);
        UISoulAnims[Idx].SetBool("IsActive", false);
    }

    public void SetSoulsCount(int SoulValue)
    {
        SoulValue = Mathf.Clamp(SoulValue, 1, 3 );
        int PreviousSoulValue = CurrentSoulValue;
        CurrentSoulValue = SoulValue;

        if( CurrentSoulValue != PreviousSoulValue )
        {
            for( int Idx = Mathf.Max(PreviousSoulValue, CurrentSoulValue) - 1; Idx > Mathf.Min(PreviousSoulValue, CurrentSoulValue) - 1; --Idx )
            {
                UISoulAnims[Idx].SetTrigger("Fade");
                StartCoroutine(DelaySetActiveFalse(Idx));
            }
        }

        UISoulAnims[CurrentSoulValue - 1].SetBool("IsActive", true);
    }
}
