using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public float Speed;

    private float _DesiredZDepth = -5f;
    private float _CurrentZDepth = -5f;
    private bool _StartedChangingDepth = false;

    private const string INTERATABLEOBJECTTAG = "InteractableObject";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        bool changingDepth = false;

        if (_StartedChangingDepth == false)
        {
            bool allowHorizontalMove = true;
            
            if (vertical != 0)
            {
                _StartedChangingDepth = true;
                allowHorizontalMove = false;
                
                if (vertical > 0f)
                {
                    _DesiredZDepth = -5f;
                }
                else if (vertical < 0f)
                {
                    _DesiredZDepth = -15f;
                }
                
                Vector3 depthChangeDirection = new Vector3(transform.position.x, transform.position.y, _DesiredZDepth);
                depthChangeDirection -= transform.position;
                int layerToIgnore = 1 << 8;
                layerToIgnore = ~layerToIgnore;
                
                bool rayCastHit = Physics.Raycast(transform.position, depthChangeDirection.normalized, depthChangeDirection.magnitude);

                if (_CurrentZDepth == _DesiredZDepth ||
                    rayCastHit == true)
                {
                    _DesiredZDepth = _CurrentZDepth;              
                    _StartedChangingDepth = false;
                    allowHorizontalMove = true;
                }
            }
            
            if (allowHorizontalMove == true)
            {
                Controller.SimpleMove(new Vector3(horizontal * Speed, 0f, 0f));
            }
        }
        else
        {
            _CurrentZDepth = Mathf.Lerp(transform.position.z, _DesiredZDepth, 0.1f);
            transform.position = new Vector3(transform.position.x, transform.position.y, _CurrentZDepth);

            if (Mathf.Abs(_CurrentZDepth - _DesiredZDepth) <= 0.1f)
            {
                _CurrentZDepth = _DesiredZDepth;
                _StartedChangingDepth = false;
            }
        }

        ClickInteraction();
    }

    /// <summary>
    /// Raycast mouse position on left click and interact with object if present
    /// </summary>
    private void ClickInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D mouseHit = Physics2D.Raycast(mouseRay, Vector2.zero);

            if (mouseHit)
            {
                if (mouseHit.transform.tag == INTERATABLEOBJECTTAG)
                {
                    mouseHit.transform.GetComponent<InteractableObject>().Interact();
                }
            }
        }
    }
}
