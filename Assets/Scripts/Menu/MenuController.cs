using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScene"); // coloque o nome da sua cena de tutorial
    }

    // Chamar esta fun��o ao clicar no bot�o "Start"
    public void LoadGame()
    {
        SceneManager.LoadScene("Mapa 1"); // nome da cena do jogo
    }

    // Chamar esta fun��o ao clicar no bot�o "Exit"
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Jogo fechado"); // funciona no editor apenas para teste
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
