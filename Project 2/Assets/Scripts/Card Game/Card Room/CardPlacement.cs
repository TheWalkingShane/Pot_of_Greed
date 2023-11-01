using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacement : MonoBehaviour
{
    private GameObject currHand;
    public GameObject card;
    private GameObject selected = null;
    private int[] cardSlots = new int[3];

    private void Start()
    {
        currHand = this.gameObject;
        for (int i = 0; i < currHand.transform.childCount; i++)
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
                    if (selected == null)
                    {
                        hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.2f, hit.transform.position.z);
                        selected = hit.transform.gameObject;
                    }

                    if (selected != null && selected.transform.position != hit.transform.position)
                    {
                        selected.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - 0.2f, selected.transform.position.z);
                        hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y + 0.2f, hit.transform.position.z);
                        selected = hit.transform.gameObject;
                    }
                    
                }
                if (hit.transform.CompareTag("CardSlot") && selected != null)
                {
                    if (hit.transform.GetChild(0).name == "Num1" && cardSlots[0] == 0)
                    {
                        cardSlots[0] = 1;
                        selected.transform.position = hit.transform.GetChild(6).position;
                        selected.transform.rotation = Quaternion.Euler(0,0,0);
                        selected = null;
                    }
                    if (hit.transform.GetChild(0).name == "Num2" && cardSlots[1] == 0)
                    {
                        cardSlots[1] = 1;
                        selected.transform.position = hit.transform.GetChild(6).position;
                        selected.transform.rotation = Quaternion.Euler(0,0,0);
                        selected = null;
                    }
                    if (hit.transform.GetChild(0).name == "Num3" && cardSlots[2] == 0)
                    {
                        cardSlots[2] = 1;
                        selected.transform.position = hit.transform.GetChild(6).position;
                        selected.transform.rotation = Quaternion.Euler(0,0,0);
                        selected = null;
                    }
                }
            }
        }
    }
}
