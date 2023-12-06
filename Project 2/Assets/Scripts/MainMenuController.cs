using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;
    
    public void TransitionToMaze()
    {
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            
            img.color = new Color(0, 0, 0, a);
            
            yield return 0;
        }
        SceneManager.LoadScene("MazeMain");
    }
}
