using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gamesMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject scinMenu;

    [SerializeField] private GameObject[] panels;

    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject soundButtonClose;

    private int currentPanelIndex = 0;
    [SerializeField] private bool sonnd = true;

    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButtonClose;

    [SerializeField] private GameObject infoButton;

    [SerializeField] private GameObject[] skinPanels;
    private int currentSkinPanelsIndex = 0;


    public void LoadTo(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #region LogicPanelMainMenu
    #region GamesPanel
    public void OpenGamesPanel()
    {
        ToggleMenus(gamesMenu, mainMenu);
    }

    public void CloseGamesPanel()
    {
        ToggleMenus(mainMenu, gamesMenu);
    }

    public void ToggleNextPanel()
    {
        panels[currentPanelIndex].SetActive(false);

        currentPanelIndex = (currentPanelIndex + 1) % panels.Length;

        panels[currentPanelIndex].SetActive(true);
    }

    public void TogglePreviousPanel()
    {
        panels[currentPanelIndex].SetActive(false);

        currentPanelIndex = (currentPanelIndex - 1 + panels.Length) % panels.Length;

        panels[currentPanelIndex].SetActive(true);
    }
    #endregion

    #region OptionsPanel
    public void OpenOptionsPanel()
    {
        ToggleMenus(optionsMenu, mainMenu);
    }

    public void CloseOptionsPanel()
    {
        ToggleMenus(mainMenu, optionsMenu);
    }
    #endregion

    #region ScinPanel
    public void OpenScinMenuPanel()
    {
        ToggleMenus(scinMenu, mainMenu);
    }

    public void CloseScinMenuPanel()
    {
        ToggleMenus(mainMenu, scinMenu);
    }
    #endregion

    public void Sound()
    {
        if (sonnd == true)
        {
            ToggleMenus(soundButton, soundButtonClose);
            sonnd = false;
        }
        else if (sonnd == false)
        {
            ToggleMenus(soundButtonClose, soundButton);
            sonnd = true;
        }

    }

    private void ToggleMenus(GameObject showMenu, GameObject hideMenu)
    {
        showMenu.SetActive(true);
        hideMenu.SetActive(false);
    }
    #endregion


    public void PanelReset ()
    {
        ToggleMenus(yesButton, noButtonClose);
    }
    public void PaneExitReset()
    {
        ToggleMenus(noButtonClose, yesButton);
    }

    public void PaneInfo()
    {
        infoButton.SetActive(true);
    }
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void SkinNextPanel()
    {
        skinPanels[currentSkinPanelsIndex].SetActive(false);

        currentSkinPanelsIndex = (currentSkinPanelsIndex + 1) % skinPanels.Length;

        skinPanels[currentSkinPanelsIndex].SetActive(true);
    }

    public void SkinPreviousPanel()
    {
        skinPanels[currentSkinPanelsIndex].SetActive(false);

        currentSkinPanelsIndex = (currentSkinPanelsIndex - 1 + skinPanels.Length) % skinPanels.Length;

        skinPanels[currentSkinPanelsIndex].SetActive(true);
    }
}
