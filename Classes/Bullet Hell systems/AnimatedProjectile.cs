using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedProjectile : MonoBehaviour
{
    //AnimationCurves to steer projectile behaviours
    public AnimationCurve rotationCurve;       
    public AnimationCurve forwardVelocityCurve;
    public AnimationCurve sidewardsVelocityCurve;

    //use a global timer for evaluating the curves.
    public bool globalTimer = false;

    //booleans to toggle behaviour modifiers
    public bool rotation;
    public bool variableSpeed;
    public bool sidewards;

    // a generic class holding the time.
    private InternalTimer internalTimer;

    // variables to keep much needed variables between frames.
    private float addedAngle;
    private float startingRotation;
    private float linearSpeed;

    private float   rotationEval,
                    forwardVelEval,
                    sidewardsVelEval = 0f;
   
    //inverts rotation
    private bool alternateBehaviour;

    // Use this for initialization

    void Start()
    {
        internalTimer = GetComponent<InternalTimer>();
        linearSpeed = EvaluateCurve(forwardVelocityCurve);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 combinedVector = new Vector3(0, 0, 0);

        if (alternateBehaviour)
        {
            if (rotation)
            {
                rotationEval = -EvaluateCurve(rotationCurve);
            }
        }
        else
        {
            if (rotation)
            {
                rotationEval = EvaluateCurve(rotationCurve);
            }
        }

        if (sidewards)
        {
            sidewardsVelEval = EvaluateCurve(sidewardsVelocityCurve);
            combinedVector += Vector2.up * Time.deltaTime * sidewardsVelEval;
        }
        if (variableSpeed)
        {
            forwardVelEval = EvaluateCurve(forwardVelocityCurve);
            combinedVector += Vector2.left * Time.deltaTime * forwardVelEval;
        }
        else
        {
            combinedVector += Vector2.left * Time.deltaTime * linearSpeed;
        }
        
        UpdateRotation();
        Translate(combinedVector);
    }
    private void Translate(Vector3 vector)
    {
        transform.Translate(vector);
    }

    private void UpdateRotation()
    {
        transform.localRotation = Quaternion.Euler(0, 0, startingRotation + rotationEval);
    }

    private float EvaluateCurve(AnimationCurve curve)
    {
        if (globalTimer)
        {
            return curve.Evaluate(Time.time);
        }
        else
        {
            return curve.Evaluate(internalTimer.currentInternalTime);
        }
    }

    public void Reset(Transform emitter)
    {
        transform.rotation = emitter.rotation;
        transform.position = emitter.position;
        startingRotation = transform.rotation.eulerAngles.z;
    }

    public void Alternate(bool alternate)
    {
        alternateBehaviour = alternate;
    }
}
