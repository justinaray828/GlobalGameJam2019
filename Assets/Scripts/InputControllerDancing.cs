using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllerDancing : MonoBehaviour
{
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    private GameChangeInformation gameChangeInformation;

    private bool horizontalAxisToggle = false;
    private bool verticalAxisToggle = false;

    // 0: up
    // 1: down
    // 2: left
    // 3: right
    int[] inputCheck = { 1, 2, 3, 0, 2, 3 };
    int[] inputArray;
    int inputCount;

    private void Start()
    {
        disableAllDances();
        inputArray = new int[6];
        gameChangeInformation = GameObject.FindGameObjectWithTag("GameChangeInformation").GetComponent<GameChangeInformation>();
    }

    void Update()
    {
        InputAxisHorizontal();
        InputAxisVertical();
        CheckInputs();
    }

    void Awake()
    {
        disableAllDances();
        inputCount = 0;
        Debug.Log("awake");
    }

    void onEnable()
    {
        disableAllDances();
        inputCount = 0;
        Debug.Log("onenable");
    }

    private void CheckInputs()
    {
        if (inputCount >= 6)
        {
            inputCount = 0;
            bool isInputCorrect = true;

            foreach(int input in inputArray)
            {
                if(input != inputCheck[inputCount])
                {
                    isInputCorrect = false;
                }

                inputCount++;
            }
            disableAllDances();
            inputCount = 0;
            inputArray = new int[6];
            gameChangeInformation.ChangeToMainGame(isInputCorrect, "dance");
        }
    }

    private void InputAxisHorizontal()
    {
        if (Input.GetAxis("Horizontal") < 0 && !horizontalAxisToggle)
        {
            horizontalAxisToggle = true;
            disableAllDances();
            left.SetActive(true);
            inputArray[inputCount] = 2;
            inputCount++;
        }

        if (Input.GetAxis("Horizontal") > 0 && !horizontalAxisToggle)
        {
            horizontalAxisToggle = true;
            disableAllDances();
            right.SetActive(true);
            inputArray[inputCount] = 3;
            inputCount++;
        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            horizontalAxisToggle = false;
        }
    }

    private void InputAxisVertical()
    {
        if (Input.GetAxis("Vertical") < 0 && !verticalAxisToggle)
        {
            verticalAxisToggle = true;
            disableAllDances();
            down.SetActive(true);
            inputArray[inputCount] = 1;
            inputCount++;
        }

        if (Input.GetAxis("Vertical") > 0 && !verticalAxisToggle)
        {
            verticalAxisToggle = true;
            disableAllDances();
            up.SetActive(true);
            inputArray[inputCount] = 0;
            inputCount++;
        }

        if (Input.GetAxis("Vertical") == 0)
        {
            verticalAxisToggle = false;
        }
    }

    private void disableAllDances()
    {
        up.SetActive(false);
        down.SetActive(false);
        right.SetActive(false);
        left.SetActive(false);
    }
}
