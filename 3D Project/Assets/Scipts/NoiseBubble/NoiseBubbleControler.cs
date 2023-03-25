using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBubbleControler : MonoBehaviour
{
    public Animator playerAnimator;
    private Vector3 scale;
    private void Start()
    {
        scale = new Vector3(15, 15, 15);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerAnimator.GetFloat("Speed") >= 0.1 && playerAnimator.GetBool("isRunning") == false)
        {
            transform.localScale = scale * 1;
        }
        else if (playerAnimator.GetBool("isRunning"))
        {
            transform.localScale = scale * 2;
        }
        else
        {
            transform.localScale = scale * 0;
        }
    }
}
