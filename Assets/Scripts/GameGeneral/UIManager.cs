using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject gameMenu;
    [SerializeField] public GameObject deadMenu;
    [SerializeField] public GameObject stopMenu;


    [SerializeField] public bool OneGame = false;
    private void Start()
    {
    }

    private void Update()
    {
    }
    public void GamePause()
    {
        Time.timeScale = 0;

        gameMenu.SetActive(false); 
        stopMenu.SetActive(true);
    }
    public void GameStart()
    {
        Time.timeScale = 1;

        gameMenu.SetActive(true);
        stopMenu.SetActive(false);
    }
    public void Restart(int restart)
    {
        SceneManager.LoadScene(restart);
    }
    public void GamePauseLoadMainMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    public void GameLoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
