using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTracking : MonoBehaviour
{
    private bool[] slots = new bool[6]; //1-3 are players slot, 4-6 are enemy slots
    private GameObject[] cards = new GameObject[6]; //1-3 are players cards, 4-6 are enemy cards
    

    void Start()
    {
        slots[0] = false;
        slots[1] = false;
        slots[2] = false;
        slots[3] = false;
        slots[4] = false;
        slots[5] = false;
    }

    public bool isTaken(int slot)
    {
        return slots[slot];
    }

    public GameObject getCard(int slot)
    {
        return cards[slot];
    }

    public void fillSlot(int slot, GameObject card)
    {
        slots[slot] = true;
        cards[slot] = card;
    }

    public void emptySlot(int slot)
    {
        slots[slot] = false;
        Destroy(cards[slot]);
        cards[slot] = null;
    }

    public bool isEslotsEmpty()
    {
        if(isTaken(3) && isTaken(4) && isTaken(5))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
