using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(sceneIndex);
    }
    
    public void HideObject(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
