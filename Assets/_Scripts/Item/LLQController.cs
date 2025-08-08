using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LLQController : MonoBehaviour
{
    public Animator anim;

    public void FadeDisappear()
    {
        anim.Play("Stop", 0, 0f);
    }    

    public void DisAppearObject()
    {
        this.gameObject.SetActive(false);
    }    
}
