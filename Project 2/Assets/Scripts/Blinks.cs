using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinks : MonoBehaviour
{
    private float time = 0;
    private int rand;
    private bool l1 = false;
    private bool l2 = false;
    private bool l3 = false;

    public GameObject lurker1;
    public GameObject lurker2;
    public GameObject lurker3;
    
    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;
        if (time >= 5)
        {
            rand = Random.Range(1, 4);
            if (rand == 1)
            {
                if (l1)
                {
                    l1 = false;
                }
                else
                {
                    l1 = true;
                }
            }

            if (rand == 2)
            {
                if (l2)
                {
                    l2 = false;
                }
                else
                {
                    l2 = true;
                }
            }
            
            if (rand == 3)
            {
                if (l3)
                {
                    l3 = false;
                }
                else
                {
                    l3 = true;
                }
            }

            time = 0;
        }
        lurker1.SetActive(l1);
        lurker2.SetActive(l2);
        lurker3.SetActive(l3);
    }
}
