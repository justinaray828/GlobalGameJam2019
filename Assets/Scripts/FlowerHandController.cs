using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerHandController : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    private GameObject grabbedFlower;
    [SerializeField] private Sprite grabHand;
    GameChangeInformation gameChangeInformation;
    [SerializeField] GameObject handRose;
    [SerializeField] GameObject flowerPick;

    private const string GAMECHANGEINFORMATIONTAG = "GameChangeInformation";

    // Start is called before the first frame update
    void Start()
    {
        gameChangeInformation = GameObject.FindWithTag(GAMECHANGEINFORMATIONTAG).GetComponent<GameChangeInformation>();
    }

    // Update is called once per frame
    void Update()
    {
        hand.transform.position = GetWorldPositionOnPlane(Input.mousePosition, 0);
        

        if(grabbedFlower)
        {
            grabbedFlower.transform.position = hand.transform.position;
        }
        else
        {
            ClickInteraction();
        }

    }

    private void ClickInteraction()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D mouseHit;
            mouseHit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

            if (mouseHit.collider != null)
            {
                if (mouseHit.transform.tag == "WhiteRose" || mouseHit.transform.tag == "PurpleRose" || mouseHit.transform.tag == "RedRose")
                {
                    grabbedFlower = mouseHit.transform.gameObject;
                    hand.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    hand.GetComponent<SpriteRenderer>().sprite = grabHand;
                }
            }
        }
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "PurpleRose")
        {
            handRose.SetActive(true);
            flowerPick.SetActive(false);
        }
        else
        {
            gameChangeInformation.ChangeToMainGame(false, "flower");
        }
    }

}
