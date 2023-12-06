using UnityEngine;

public class CardPlacement : MonoBehaviour
{
    private GameObject currHand;
    public GameObject card;
    private GameObject selected = null;
    public SlotTracking ST;
    private int numOfSpecials;
    private int numOfBaseKit;
    public Transform[] slotPositions = new Transform[3];
    private Vector3 storedPos;
    private bool debounce = false;
    private GameObject currCard;
    public CardLookUp CLU;
    public Card[] baseKit;
    private bool blockCardInput = false;
    public Gameplay G;
    
    

    private void Start()
    {
        numOfBaseKit = 4; //Not set in stone (Will be changed later)
        
        //Set baseKit list manually
        baseKit = new Card[numOfBaseKit];
        baseKit[0] = CLU.cardList[0];
        baseKit[1] = CLU.cardList[1];
        baseKit[2] = CLU.cardList[2];
        baseKit[3] = CLU.cardList[3];
        
        //currHand is just a transform for storage
        currHand = this.gameObject;
        numOfSpecials = 0; //Set somehow once inventory is set up
        for (int i = 0; i < numOfBaseKit; i++)
        {
            currCard = Instantiate(card, currHand.transform.GetChild(i).transform); //Instantiate visual object for card
            currCard.GetComponent<CardInfo>().setCard(baseKit[i].health, baseKit[i].damage); //Set current instantiated card to the baseKit card at index i
            //Check if card has a texture stored
            if (baseKit[i].cardImage != null)
            {
                currCard.GetComponent<MeshRenderer>().material.mainTexture = baseKit[i].cardImage; //Use card texture
            }
        }

        for (int i = 0; i < numOfSpecials; i++)
        {
            currCard = Instantiate(card, currHand.transform.GetChild(i + numOfBaseKit).transform); //Instantiate visual object for card
            //Todo read card from inventory and add it to hand
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!blockCardInput)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
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

                        if (selected != null && selected.transform.position != hit.transform.position) //Check to see if selected card is in the same spot as the placed one
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
                    if (hit.transform.CompareTag("CardSlot") && selected != null) //Handles where card is placed
                    {
                        //if CardSlot is hit, check the name for its placement
                        if (hit.transform.name == "CardSlot 1" && !ST.isTaken(0)) //check to see if slot is taken
                        {
                            ST.fillSlot(0, selected);
                            //put selected card at position and rotation of card slot
                            selected.transform.position = slotPositions[0].position;
                            selected.transform.rotation = Quaternion.Euler(0,0,0);
                            //set selected card to null
                            selected = null;
                            G.cardPlaced();
                        }
                        if (hit.transform.name == "CardSlot 2" && !ST.isTaken(1))//check to see if slot is taken
                        {
                            ST.fillSlot(1, selected);
                            //put selected card at position and rotation of card slot
                            selected.transform.position = slotPositions[1].position;
                            selected.transform.rotation = Quaternion.Euler(0,0,0);
                            //set selected card to null
                            selected = null;
                            G.cardPlaced();
                        }
                        if (hit.transform.name == "CardSlot 3" && !ST.isTaken(2))//check to see if slot is taken
                        {
                            ST.fillSlot(2, selected);
                            //put selected card at position and rotation of card slot
                            selected.transform.position = slotPositions[2].position;
                            selected.transform.rotation = Quaternion.Euler(0,0,0);
                            //set selected card to null
                            selected = null;
                            G.cardPlaced();
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //debounce so card can't be clicked 1000000 times from mouse being held down (Mainly for deselecting an already selected card)
            debounce = false;
        }
        
        //Example / Test for getting card information through the slots
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     GameObject card1;
        //     CardInfo card1I;
        //     GameObject card2;
        //     CardInfo card2I;
        //     card1 = ST.getCard(0);
        //     card2 = ST.getCard(1);
        //     card1I = card1.GetComponent<CardInfo>();
        //     card2I = card2.GetComponent<CardInfo>();
        //     Debug.Log(card1I.health + " | " + card1I.damage);
        //     Debug.Log(card2I.health + " | " + card2I.damage);
        // }
    }

    public void cardInput(bool b)
    {
        blockCardInput = !b;
    }
}
