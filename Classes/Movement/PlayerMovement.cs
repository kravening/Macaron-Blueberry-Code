using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMoveable
{
    public float movementSpeed;
    public float jumpStrength;

    private float currentMovementSpeed;

    public List<string> actionQueue = new List<string>(); //holds a list of player actions in a queue to handle multiple inputs and act on the latest
    public List<MovementTriggerCheck> colliderChecks = new List<MovementTriggerCheck>();

    private int     actionCount;

    private Vector2 horizontalVector,
                    processedHorizontalVector,
                    verticalVector,
                    processedVerticalVector;

    private bool    movingLeft,
                    movingRight,
                    movingUp,
                    movingDown,
                    idle,
                    use = false;

    private string  previousHorizontalInput,
                    previousVerticalInput;

    public CcelerationCurves horizontalCcelerationCurves, verticalCcelerationCurves;

    private Interactable interactable;

    //Methods

    //MonoBehaviour methods 
    void Start()
    {
        actionQueue.Add("placeholder");
    }
    
    void Update()
    {
        ProcessMovementSpeed();
    }

    //Input behaviour stuff
    void IMoveable.MoveLeft(bool keystate)
    {
        if (movingLeft == false && keystate == true) //getkeyUp
        {
            AddActionToActionQueue("left");
            horizontalCcelerationCurves.Reset();
        }
        else if (movingLeft == true && keystate == false) //getkeyDown
        {
            RemoveActionFromActionQueue("left");
        }

        if (GetHorizonatalPriority() == "left")
        {
            if (previousHorizontalInput == "right")
            {
                horizontalCcelerationCurves.Reset();
            }
            previousHorizontalInput = "left";
            MoveLeft();
        }
        movingLeft = keystate;
    }
    void IMoveable.MoveRight(bool keystate)
    {
        if (movingRight == false && keystate == true) //getkeyUp
        {

            AddActionToActionQueue("right");
            horizontalCcelerationCurves.Reset();
        }
        else if (movingRight == true && keystate == false) //getkeyDown
        {
            RemoveActionFromActionQueue("right");
        }

        if (GetHorizonatalPriority() == "right")
        {
            if(previousHorizontalInput == "left") {
                horizontalCcelerationCurves.Reset();
            }
            previousHorizontalInput = "right";
            MoveRight();
        }
        movingRight = keystate;
    }
    void IMoveable.MoveUp(bool keystate)
    {
        if (movingUp == false && keystate == true) //getkeyUp
        {
            AddActionToActionQueue("up");
            verticalCcelerationCurves.Reset();
        }
        else if (movingUp == true && keystate == false) //getkeyDown
        {
            RemoveActionFromActionQueue("up");
        }

        if (GetVerticalPriority() == "up")
        {
            if (previousVerticalInput== "down")
            {
                verticalCcelerationCurves.Reset();
            }
            previousVerticalInput = "up";
            MoveUp();
        }
        movingUp = keystate;
    }
    void IMoveable.MoveDown(bool keystate)
    {
        if (movingDown == false && keystate == true) //getkeyUp
        {
            AddActionToActionQueue("down");
            verticalCcelerationCurves.Reset();
        }
        else if (movingDown == true && keystate == false) //getkeyDown
        {
            RemoveActionFromActionQueue("down");
        }

        if (GetVerticalPriority() == "down")
        {
            if (previousVerticalInput == "up")
            {
                verticalCcelerationCurves.Reset();
            }
            previousVerticalInput = "down";
            MoveDown();
        }
        movingDown = keystate;
    }
    void IMoveable.Use(bool keystate)
    {
        if (use == false && keystate == true)
        {
            AddActionToActionQueue("Use");
        }
        else if (use == true && keystate == false)
        {
            RemoveActionFromActionQueue("Use");
        }

        if (actionQueue[actionCount] == "Use")
        {
            Use();
        }
        use = keystate;
    }

    void MoveRight()
    {
            horizontalVector = Vector2.right;
    }
    void MoveLeft()
    {
            horizontalVector = Vector2.left;
    }
    void MoveUp()
    {
            verticalVector = Vector2.up;

    }
    void MoveDown()
    {
            verticalVector = Vector2.down;
    }

    void Use()
    {
        if(interactable)
        interactable.Interaction();
    }

    //Movement stuff
    void ProcessMovementSpeed()
    {

        if(movingLeft || movingRight) // player is moving either left or right
        {
            horizontalCcelerationCurves.SetAccelerationBool(true);
        }
        else
        {
            horizontalCcelerationCurves.SetAccelerationBool(false);
            //deccelerate horizontaly
        }

        if(movingUp || movingDown)
        {
            verticalCcelerationCurves.SetAccelerationBool(true);
        }
        else
        {
            verticalCcelerationCurves.SetAccelerationBool(false);
            //deccelerate vertically
        }

        transform.Translate(horizontalVector * movementSpeed * Time.deltaTime * horizontalCcelerationCurves.GetCCelerationSpeed());
        transform.Translate(verticalVector * movementSpeed * Time.deltaTime * verticalCcelerationCurves.GetCCelerationSpeed());

        if (movingUp == false && movingDown == false && movingLeft == false && movingRight == false)
        {
            idle = true;
        }
        else
        {
            idle = false;
        }
    }

    //ActionQueue stuff

    void AddActionToActionQueue(string actionName)
    {
        actionQueue.Add(actionName);
        actionCount = actionQueue.Count - 1;
    }

    void RemoveActionFromActionQueue(string actionName)
    {
        actionCount = actionQueue.Count;
        for (int i = actionCount - 1; i > 0; i--)
        {
            if (actionQueue[i] == actionName)
            {
                actionQueue.RemoveAt(i);
            }
        }
        actionCount = actionQueue.Count - 1;
    }

    string GetHorizonatalPriority()
    {
        for (int i = actionCount; i > 0; i--)
        {
            if(actionQueue[i] == "left" || actionQueue[i] == "right")
            {
                return actionQueue[i];
            }
        }
        return null;
    }
    string GetVerticalPriority()
    {
        for (int i = actionCount; i > 0; i--)
        {
            if (actionQueue[i] == "up" || actionQueue[i] == "down")
            {
                return actionQueue[i];
            }
        }
        return null;
    }



    public void SetInteractAble(Interactable ia)
    {
        interactable = ia;
    }
}
