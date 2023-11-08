using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    private static SwitcherMethod _method = SwitcherMethod.Additive;
    private GameObject cameraParent;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraParent = FindObjectOfType<AudioListener>().gameObject.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (_method == SwitcherMethod.Swap)
            {
                // Swap to the From Scene
                SwapToScene("From Scene");
            }
            else
            {
                TryRemoveScene("From Scene");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (_method == SwitcherMethod.Swap)
                // Swap to the To Scene
                SwapToScene("To Scene");
            else
            {
                AddScene("From Scene");
            }
        }
}
    
    void SwapToScene(string scene)
    {
        Debug.Log("Switching to " + scene);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
        // Debug.Log("Counter = " + CardInventory.instance.testCounter++);
    }

    // Additively load scene
    void AddScene(string scene)
    {
        if (SceneManager.GetActiveScene().name.Equals(scene) || SceneManager.sceneCount > 1)
            return;
        
        cameraParent.SetActive(false);
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    // Unload additively loaded scene
    void TryRemoveScene(string scene)
    {
        if (SceneManager.sceneCount == 1)
            return;
        
        SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
        cameraParent.SetActive(true);
    }

    private enum SwitcherMethod
    {
        Swap,
        Additive
    }
}
