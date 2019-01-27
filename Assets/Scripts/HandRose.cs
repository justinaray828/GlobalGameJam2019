using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRose : MonoBehaviour
{
    public Transform EndMark;
    Vector3 movement;
    [SerializeField] private float movementAmount = 1f;
    [SerializeField] GameChangeInformation gameChangeInformation;

    private bool _HasFinished = false;

    void Start()
    {
        movement = new Vector3(movementAmount, 0, 0);
        gameChangeInformation = GameObject.FindGameObjectWithTag("GameChangeInformation").GetComponent<GameChangeInformation>();
    }

    private void OnEnable()
    {
        _HasFinished = false;
    }

    void Update()
    {
        transform.position -= movement * Time.deltaTime;

        if (_HasFinished == false &&
            transform.position.x <= EndMark.position.x)
        {
            gameChangeInformation.ChangeToMainGame(true, "flower");
            _HasFinished = true;
        }
    }
}
