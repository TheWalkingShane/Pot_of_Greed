using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTracking : MonoBehaviour
{
    private bool[] slots = new bool[3];
    
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

    public void fillSlot(int slot)
    {
        slots[slot] = true;
    }

    public void emptySlot(int slot)
    {
        slots[slot] = false;
    }
}
