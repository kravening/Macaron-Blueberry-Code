using UnityEngine;

public class CcelerationTimer{
    public float multiplier = 2;
    float timer = 0;

    public void UpdateTimer()
    {
        if (timer < 1 || timer > 0)
        {
            timer += Time.deltaTime * multiplier;
        }
        else if (timer > 1)
        {
            timer = 1;
        }
        else if(timer < 0)
        {
            timer = 0;
        }
    }

    public void ResetTimer()
    {
        timer = 0;
    }
    public float GetTimer()
    {
        return timer;
    }

    public void SetTimer(float value)
    {
        timer = value;
    }
}
