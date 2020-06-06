using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate_Manager : MonoBehaviour
{

    Animator animator;
    public bool isOpen;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            animator.SetBool("isOpen", true);
        }
        else
        {
            animator.SetBool("isOpen", false);
        }
    }
}
