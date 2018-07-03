using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraPanToObject : MonoBehaviour
{
    public Transform target;
    public float xOffset = 10;
    public float zOffset = 25;
    public float engageFollowDistance = 2;
    // Use this for initialization
    void Start()
    {

        if (!transform) // if there is no target set player as default target
            target = FindObjectOfType<PlayerLives>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 difference = transform.position - new Vector3(target.position.x + xOffset, target.position.y, -10); //calculates the differences of the x and y of the vectors
            float distance = Mathf.Sqrt(difference.x * difference.x + difference.y * difference.y); // asqr*bsqr=csqr distance calculation

            distance -= engageFollowDistance; // adds a radius around the current position in which the camera stops following the player

            if (distance > 2) // if outside of the deadzone lerp camera
                transform.position = Vector3.Slerp(transform.position, new Vector3(target.position.x + xOffset, target.position.y, zOffset), Time.deltaTime * ((distance * distance) / 2)); // cameralerp, multiplies smoothing value by the distance for a exponential speed increase the further away the target is
            else if (distance > 0)
                transform.position = Vector3.Slerp(transform.position, new Vector3(target.position.x + xOffset, target.position.y, zOffset), Time.deltaTime * distance); // resorts to slower lerping and easing into of the new position within a certain range, for a better feeling lerp.
        }
    }
}
