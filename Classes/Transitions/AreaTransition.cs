using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition : MonoBehaviour // transitions between areas to save up performance, especially regarding the colliders.
{
    private BoxCollider2D switchTrigger; //the trigger

    public GameObject leftArea;
    public GameObject rightArea;

    private void Start()
    {
        switchTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision) //once the player leaves the trigger
    {
        if (leftArea && rightArea)
        {
            if (collision.transform.position.x <= transform.position.x) //left of trigger box, switch on the area on the left, and turn off the area on the right.
            {
                leftArea.SetActive(true);
                rightArea.SetActive(false);
            }
            else
            {                                                      // right of triggerBox, switch on the area on the right, and turn off the area on the left.
                leftArea.SetActive(false);
                rightArea.SetActive(true);
            }
        }
    }
}
