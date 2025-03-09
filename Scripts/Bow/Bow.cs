using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public enum BowMode
    {
        Arrow,
        Grappling
    }


    private Animator bowAnimator;

    // Start is called before the first frame update
    void Start()
    {
        bowAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBowAnimation()
    {
        bowAnimator.SetTrigger("Fire");
    }
}
