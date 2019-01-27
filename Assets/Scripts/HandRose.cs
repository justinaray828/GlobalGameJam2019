using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRose : MonoBehaviour
{
    Vector3 movement;
    float target = 312f;
    [SerializeField] private float movementAmount = 1f;
    [SerializeField] GameChangeInformation gameChangeInformation;

    void Start()
    {
        movement = new Vector3(movementAmount, 0, 0);
        gameChangeInformation = GameObject.FindGameObjectWithTag("GameChangeInformation").GetComponent<GameChangeInformation>();
    }

    void Update()
    {
        transform.position -= movement * Time.deltaTime;
        if(transform.position.x <= target)
        {
            WinState();
        }
    }

    void WinState()
    {
        gameChangeInformation.ChangeToMainGame(true, "flower");
    }
}
