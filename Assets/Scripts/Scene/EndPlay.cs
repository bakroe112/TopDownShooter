using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPlay : MonoBehaviour
{
    // Restart lại màn chơi hiện tại
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }


    // Quay lại Main Menu
    public void BackToMenu()
    {
        Debug.Log("Back to Menu");
        SceneManager.LoadScene("MainMenu");
 
    }

    // Thoát game
    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
