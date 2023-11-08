using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            // Swap to the From Scene
            SwapToScene("From Scene");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            // Swap to the To Scene
            SwapToScene("To Scene");
        }
    }

    void SwapToScene(string scene)
    {
        Debug.Log("Switching to " + scene);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        // Debug.Log("Counter = " + CardInventory.instance.testCounter++);
    }
}
