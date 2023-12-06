using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void Quitmenu()
    {
        Application.Quit();
        Debug.Log("Game is exiting"); // This line is for testing in the editor
    }
}
