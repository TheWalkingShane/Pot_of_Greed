using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTracking : MonoBehaviour
{
    private bool[] slots = new bool[3];
    private GameObject[] cards = new GameObject[3];
    
    void Start()
    {
        slots[0] = false;
        slots[1] = false;
        slots[2] = false;
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
        cards[slot] = null;
    }
}
