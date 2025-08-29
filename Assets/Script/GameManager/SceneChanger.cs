using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void Level_1x1()
    {
        SceneManager.LoadScene("Level 1x1");
    }
    public void Level_2x2()
    {
        SceneManager.LoadScene("Level 2x2");
    }
    public void Level_3x3()
    {
        SceneManager.LoadScene("Level 3x3");
    }
    public void Restart_1x1()
    {
        Level_1x1();
    }
    public void Restart_2x2()
    {
        Level_2x2();
    }
    public void Restart_3x3()
    {
        Level_3x3();
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
