using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float duration;
    public float elapsed;

    public Image timerFill;
    public Image tintFill;

    public float minPulseSpeed;
    public float maxPulseSpeed;

    public Color startTint;
    public Color endTint;

    public Animator animator;
    public AnimationCurve pulseRamp;



    public void Initialise(float _duration)
    {
        elapsed = 0;
        duration = _duration;
    }

    public void Refresh(float elapsed)
    {
        timerFill.fillAmount = elapsed / duration;

        tintFill.fillAmount = timerFill.fillAmount;
        tintFill.color = Color.Lerp(startTint, endTint, timerFill.fillAmount);

        animator.speed = minPulseSpeed + (pulseRamp.Evaluate(timerFill.fillAmount) * maxPulseSpeed);


    }
}
