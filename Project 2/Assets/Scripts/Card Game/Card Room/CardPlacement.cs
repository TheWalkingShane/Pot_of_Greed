using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacement : MonoBehaviour
{
    private GameObject currHand;
    public GameObject card;
    private GameObject selected = null;
    public SlotTracking ST;
    private int numOfCards;
    public Transform[] slotPositions = new Transform[3];
    private Vector3 storedPos;
    private bool debounce = false;

    private void Start()
    {
        currHand = this.gameObject;
        numOfCards = 4; //Set somehow once inventory is set up
        for (int i = 0; i < numOfCards; i++)
        {
            Instantiate(card, currHand.transform.GetChild(i).transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.transform.tag);
                if (hit.transform.CompareTag("Card"))
                {
                    //if Card is hit, check if they have a card selected already
                    if (selected == null)
                    {
                        //save original position
                        storedPos = hit.transform.position;
                        //shift card up to indicate that it's selected
                        hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.2f, hit.transform.position.z);
                        //save card as selected
                        selected = hit.transform.gameObject;
                        debounce = true;
                    }

                    if (selected != null && selected.transform.position != hit.transform.position)
                    {
                        //set previously selected card to it's original position
                        selected.transform.position = storedPos;
                        //save new position
                        storedPos = hit.transform.position;
                        //shift new card up
                        hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.2f, hit.transform.position.z);
                        //make new card the selected card
                        selected = hit.transform.gameObject;
                        debounce = true;
                    }
                    else if (selected != null && selected.transform.position == hit.transform.position && !debounce)
                    {
                        //set previously selected card to it's original position
                        selected.transform.position = storedPos;
                        // clear selected
                        selected = null;
                    }
                    
                }
                if (hit.transform.CompareTag("CardSlot") && selected != null)
                {
                    //if CardSlot is hit, check the name for its placement
                    if (hit.transform.name == "CardSlot 1" && !ST.isTaken(0))
                    {
                        //check to see if slot is taken
                        ST.fillSlot(0);
                        //put selected card at position and rotation of card slot
                        selected.transform.position = slotPositions[0].position;
                        selected.transform.rotation = Quaternion.Euler(0,0,0);
                        //set selected card to null
                        selected = null;
                    }
                    if (hit.transform.name == "CardSlot 2" && !ST.isTaken(1))
                    {
                        //check to see if slot is taken
                        ST.fillSlot(1);
                        //put selected card at position and rotation of card slot
                        selected.transform.position = slotPositions[1].position;
                        selected.transform.rotation = Quaternion.Euler(0,0,0);
                        //set selected card to null
                        selected = null;
                    }
                    if (hit.transform.name == "CardSlot 3" && !ST.isTaken(2))
                    {
                        //check to see if slot is taken
                        ST.fillSlot(2);
                        //put selected card at position and rotation of card slot
                        selected.transform.position = slotPositions[2].position;
                        selected.transform.rotation = Quaternion.Euler(0,0,0);
                        //set selected card to null
                        selected = null;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            debounce = false;
        }
    }
}
