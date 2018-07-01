using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parallax based on the camera position relative to the object, this fakes a perspective.

public class CameraParallaxObject : MonoBehaviour {
    [SerializeField]private Camera mainCamera;

    public int objectDistance = 0;

    Vector3 cameraStartingPosition;
    Vector3 startingPosition;
    float parallaxStrength;
    [Range(0, 100)]

    Vector3 lastPosition;

    [SerializeField]private CameraParallaxController parallaxController;
	// Use this for initialization

	void Start () {
        if(objectDistance > 0)
        {
            parallaxController.AddParallaxObjectToList(this);
            parallaxStrength = -parallaxController.GetParallaxStrength(objectDistance / 100f);
            if (!GetComponent<SpriteRenderer>())
            {

            }
            else
            {
                GetComponent<SpriteRenderer>().sortingOrder = -objectDistance * 2;
            }

            startingPosition = transform.position;

            if(!mainCamera)
            mainCamera = Camera.main;
        }
        else
        {
            this.enabled = false;
        }
    }

    public void ParallaxScroll()
    {
            Vector3 difference = (startingPosition - new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0)); //subtraction cancels out z axis, reduces extra vector calculations;
            transform.localPosition = startingPosition + (difference * objectDistance);
    }
}
