using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerMenu : MonoBehaviour
{
    #region Variables
    [Header("Scene Transition")]
    public Animator trans;

    [Header("Game Data")]
    public Data data;

    [Header("UI Buttons")]
    public Button info, rule;

    [Header("Singleton")]
    public static ManagerMenu Instance;

    [Header("UI")]
    public GameObject loginPanel;
    public GameObject registerPanel;
    public GameObject emailVerificationPanel;
    public GameObject profilePanel;

    public TMP_Text emailVerificationText;

    [Header("Profile Picture Update Data")]
    public GameObject profileUpdatePanel;
    public Image profileImage;
    public TMP_InputField urlInputField;
    #endregion

    #region Unity Lifecycle

    public void Awake()
    {
        CreateInstance();
    }

    private void Update()
    {
        HandleInput();
    }

    #endregion

    #region Singleton Implementation
    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    #region UI Management
    private void ClearUI()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
        profilePanel.SetActive(false); 
    }

    public void OpenLoginPanel()
    {
        ClearUI();
        loginPanel.SetActive(true);
    }

    public void OpenRegisterPanel()
    {
        ClearUI();
        registerPanel.SetActive(true);
    }

    public void OpenProfilePanel()
    {
        ClearUI();
        profilePanel.SetActive(true);
    }

    public void OpebClosrProfileUpdatePanel()
    {
        profileUpdatePanel.SetActive(!profileUpdatePanel.activeSelf); 
    }

    public void ShowVerificationResponse(bool isEmailSent, string emailid, string errorMessage)
    {
        ClearUI();
        emailVerificationPanel.SetActive(true);

        if (isEmailSent)
        {
            emailVerificationText.text = $"Please verify your email address \n Verification email has been sent to {emailid}";
        }
        else
        {
            emailVerificationText.text = $"Couldn't sent email: {errorMessage}";
        }
    }

    public void LoadProfileImage(string url)
    {
        StartCoroutine(LoadProfileImageIE(url));
    }

    public IEnumerator LoadProfileImageIE(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
            profileImage.sprite = sprite;
            profileUpdatePanel.SetActive(false);
        }
    }

    public string GetProfileUpdateURL()
    {
        return urlInputField.text;
    }
    #endregion

    #region Time Management
    public void TimeGo() => Time.timeScale = 1;
    #endregion

    #region Scene Management
    public void BackToMenu()
    {
        data.DialogManager = false;
        data.countFuel = 0;
        data.countCoins = 0;
        StartCoroutine(LoadLevelNext(0));
    }

    public void GoToGame() => StartCoroutine(LoadLevelNext(1));
    public void ExitToGame() => Application.Quit();

    public void LoadLevel(int index)
    {
        StartCoroutine(LoadLevelNext(index));
    }

    IEnumerator LoadLevelNext(int levelIndex)
    {
        trans.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }
    #endregion

    #region Input Handling
    private void HandleInput()
    {
        HandleCheatKeys();
        HandleEscapeKey();
        HandleLKey();
    }

    private void HandleCheatKeys()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            data.countFuel = 0;
            data.countCoins = 0;
            data.DialogManager = false;
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HandleEscapeKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (info != null)
            {
                info.onClick.Invoke();
            }
        }
    }

    private void HandleLKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (rule != null)
            {
                Time.timeScale = 1;
                rule.onClick.Invoke();
            }
        }
    }
    #endregion
}