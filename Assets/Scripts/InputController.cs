using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InputController : MonoBehaviour
{
    private enum Inputs { Horizontal , Vertical }; //List of inputs used for the game

    private DelayBool inputDelay;

    private bool axisToggleBool = false; //Used to make an axis a toggle

    private Rigidbody2D rb2D;

    [SerializeField] float inputDelaySeconds = 0.5f;
    [SerializeField] float gameObjectSpeed = 0.5f;

    private void Start()
    {
        inputDelay = new DelayBool(inputDelaySeconds);
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        float horizontalMovement = Input.GetAxis(Inputs.Horizontal.ToString());
        float verticalMovement = Input.GetAxis(Inputs.Vertical.ToString());

        if ( horizontalMovement != 0 || verticalMovement != 0 )
        {
            MoveGameObject(horizontalMovement, verticalMovement);
        }
    }

    private void MoveGameObject(float horizontalMovement, float verticalMovement)
    {
        Vector2 movementVector2D = new Vector2(horizontalMovement, verticalMovement);
        rb2D.AddForce(movementVector2D * gameObjectSpeed);
    }

    private bool InputAxisIsToggled(string input)
    {
        if (Input.GetAxis(input) > 0 && !axisToggleBool)
        {
            axisToggleBool = true;
            return true;
        }

        if (Input.GetAxis(input) == 0)
        {
            axisToggleBool = false;
        }

        return false;
    }

    //DelayBool Timer
    //--------------------------------------------------------
    private IEnumerator Delay(DelayBool delayBool)
    {
        delayBool.delayBoolState = false;
        yield return new WaitForSeconds(delayBool.delayTime);
        delayBool.delayBoolState = true;
    }

    /// <summary>
    /// Starts Delay and returns true if the delay starts and false if the delay is in progress
    /// </summary>
    /// <param name="delayBool"></param>
    /// <returns></returns>
    private bool CallDelay(DelayBool delayBool)
    {
        if (delayBool.delayBoolState == true)
        {
            StartCoroutine(Delay(delayBool));
            return true;
        }
        else
        {
            return false;
        }
    }
    //--------------------------------------------------------
    //
}
