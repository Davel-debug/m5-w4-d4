using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void Level()
    {
        SceneManager.LoadScene("Level");
    }
    public void Restart()
    {
        Level();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void HowToPlay()
        {
            SceneManager.LoadScene("HowToPlay");
        }
    public void ExitGame()
    {
        Application.Quit();
    }
}
