using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcelerationCurves : MonoBehaviour {
    public AnimationCurve accelerationCurve;
    public AnimationCurve deccelerationCurve;

    public float accelTimerMultiplier = 2f;
    public float deccelTimerMultiplier = 4f;

    private float evaluatedValue;
    private float evaluationPoint;

    private bool isAccelerating;
    private bool switchHolder;

    CcelerationTimer accelTimer = new CcelerationTimer();
    CcelerationTimer deccelTimer = new CcelerationTimer();

    // Use this for initialization
    void Start () {
        accelTimer.multiplier = accelTimerMultiplier;
        deccelTimer.multiplier = -deccelTimerMultiplier;
	}
	
	// Update is called once per frame
	void Update () {
        ProcessCceleration();
	}

    public void ProcessCceleration()
    {
        if(isAccelerating == true && switchHolder == false)
        {
            switchHolder = isAccelerating;
            accelTimer.ResetTimer();
        }
        else if(isAccelerating == false && switchHolder == true)
        {
            switchHolder = isAccelerating;
            if(accelTimer.GetTimer() > 1)
            {
                deccelTimer.SetTimer(1);
            }
            else
            {
                deccelTimer.SetTimer(accelTimer.GetTimer());
            }
        }

        if (isAccelerating)
        {
            accelTimer.UpdateTimer();
        }
        else
        {
            deccelTimer.UpdateTimer();
        }
    }

    public void SetAccelerationBool(bool boolean)
    {
        isAccelerating = boolean;
    }

    public float GetCCelerationSpeed()
    {
        if (isAccelerating)
        {
            return accelerationCurve.Evaluate(accelTimer.GetTimer());
        }
        else
        {
            return deccelerationCurve.Evaluate(deccelTimer.GetTimer());
        }
    }

    public void Reset()
    {
        accelTimer.SetTimer(0);
    }
}
