using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
public class CameraParallaxController : MonoBehaviour {
    public AnimationCurve ParallaxLayerDepthCurve;

    private List<CameraParallaxObject> activeParallaxObjects = new List<CameraParallaxObject>();
    private Transform cameraTrans;
	// Use this for initialization
	void Start () {
        cameraTrans = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        
        if(activeParallaxObjects.Count >= 1)
        for (int i = 0; i < activeParallaxObjects.Count; i++)
        {
            activeParallaxObjects[i].ParallaxScroll();
        }
	}

    public void AddParallaxObjectToList(CameraParallaxObject obj)
    {
        if (GetObjectFromList(obj) == null)
        {
            activeParallaxObjects.Add(obj);
        }
    }

    public CameraParallaxObject GetObjectFromList(CameraParallaxObject obj)
    {
        for (int i = activeParallaxObjects.Count-1; i > 0 ; i--)
        {
            if (obj == activeParallaxObjects[i])
            {
                return activeParallaxObjects[i];
            }
        }

        return null;
    }

    public void RemoveParallaxObjectFromList(CameraParallaxObject obj)
    {
        activeParallaxObjects.Remove(GetObjectFromList(obj));
    }

    public float GetParallaxStrength(float layerDepth)
    {
        return ParallaxLayerDepthCurve.Evaluate(layerDepth);
    }
}
