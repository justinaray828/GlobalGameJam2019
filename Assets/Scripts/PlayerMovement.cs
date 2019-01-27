using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;

    public float Speed;
    public AudioManager am;

    private float _DesiredZDepth = -5f;
    private float _CurrentZDepth = -5f;
    private bool _StartedChangingDepth = false;
    private bool iswalking = false;

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

        if(horizontal > 0f || horizontal < 0f)
        {
            if (!iswalking)
            {
                am.PlayWalk();
                iswalking = true;
            }
        }
        else
        {
            if (iswalking)
            {
                am.StopWalk();
                iswalking = false;
            }
        }

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
            float zDirection = _DesiredZDepth - _CurrentZDepth;
            zDirection /= Mathf.Abs(zDirection);
            
            Controller.SimpleMove(new Vector3(0f, 0f, zDirection * Speed));
            _CurrentZDepth = transform.position.z;

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
        if (Input.GetMouseButtonUp(0))
        {
            Ray mousRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            Physics.Raycast(mousRay, out mouseHit);

            if (mouseHit.collider != null)
            {
                if (mouseHit.transform.tag == INTERATABLEOBJECTTAG)
                {
                    mouseHit.transform.GetComponent<InteractableObject>().Interact();
                }
            }
        }
    }
}
