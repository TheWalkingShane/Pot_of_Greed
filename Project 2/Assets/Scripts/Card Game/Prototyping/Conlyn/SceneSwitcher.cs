using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private static SwitcherMethod _method = SwitcherMethod.Additive;
    private GameObject cameraParent;
    public string toScene = "MazeMain";
    public string fromScene = "CardsMain";

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
                SwapToScene(fromScene);
            }
            else
            {
                TryRemoveScene(fromScene);
            }
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (_method == SwitcherMethod.Swap)
                // Swap to the To Scene
                SwapToScene(toScene);
            else
            {
                AddScene(fromScene);
            }
        }
    }

    void SwapToScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    // Additively load scene
    void AddScene(string scene)
    {
        if (SceneManager.GetActiveScene().name.Equals(scene) || SceneManager.sceneCount > 1)
            return;

        cameraParent.SetActive(false);
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Unload additively loaded scene
    void TryRemoveScene(string scene)
    {
        if (SceneManager.sceneCount == 1)
            return;

        SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
        cameraParent.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private enum SwitcherMethod
    {
        Swap,
        Additive
    }
}