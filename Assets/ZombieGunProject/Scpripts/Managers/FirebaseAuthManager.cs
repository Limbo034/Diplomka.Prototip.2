using Firebase;
using Firebase.Auth;
using System;
using System.Collections;
using System.Security.Policy;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    private string defaultUserImage = "https://icon-icons.com/icon/person-avatar-account-user/191606";
    private void Start()
    {
        StartCoroutine(CheckAndFixDependenciesAsync());
    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);


        dependencyStatus = dependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();
            yield return new WaitForEndOfFrame();
            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                ManagerMenu.Instance.OpenLoginPanel();
                ClearLoginInputFieldText();
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    #region AutoLogin
    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUser = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUser.IsCompleted);

            AutoLogin();
        }
        else
        {
            ManagerMenu.Instance.OpenLoginPanel();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                Debug.Log("AutoLogin User name: " + user.DisplayName);
                ManagerMenu.Instance.OpenProfilePanel(); // ��������� ProfilePanel ��� �������� ���������� � ���������������� email

                if (!string.IsNullOrEmpty(user.PhotoUrl?.ToString())) // ��������� �� null ����� ����������
                {
                    ManagerMenu.Instance.LoadProfileImage(user.PhotoUrl.ToString());
                }
            }
            else
            {
                SendEmailForVerification();
            }
        }
        else
        {
            ManagerMenu.Instance.OpenLoginPanel(); // ��������� LoginPanel, ���� ��� ������������� ������������
        }
    }
    #endregion

    #region Logout
    public void Logout()
    {
        if(auth != null && user != null)
        {
            auth.SignOut();
        }
    }

    private void ClearLoginInputFieldText() 
    {
        emailLoginField.text = ""; 
        passwordLoginField.text = "";
    }
    #endregion

    #region Login
    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result.User;
            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

            if (user.IsEmailVerified)
            {
                Debug.Log("In Go Game");
                ManagerMenu.Instance.OpenProfilePanel();

                if (string.IsNullOrEmpty(user.PhotoUrl.ToString()))
                {
                    ManagerMenu.Instance.LoadProfileImage(user.PhotoUrl.ToString());
                }
            }
            else
            {
                SendEmailForVerification();
            }
        }
    }
    #endregion

    #region Regiter
    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);
                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Because ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }
                Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = name, PhotoUrl = new Uri(defaultUserImage)};

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = " Profile update Failed ! Because ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += " Email is invalid ";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += " Wrong Password ";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += " Email is missing ";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += " Password is missing ";
                            break;
                        default:
                            failedMessage = " Profile update Failed ";
                            break;
                    }
                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log(" Registration Successful Welcome " + user.DisplayName);
                    
                    if (user.IsEmailVerified)
                    {
                        ManagerMenu.Instance.OpenLoginPanel();
                    }
                    else
                    {
                        SendEmailForVerification();
                    }
                }
            }
        }
    }
    #endregion

    #region Verfication
    public void SendEmailForVerification()
    {
        StartCoroutine(SendEmailForVerificationAsync());
    }

    private IEnumerator SendEmailForVerificationAsync()
    {
        if (user != null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();

            yield return new WaitUntil(() => sendEmailTask.IsCompleted);


            if (sendEmailTask.Exception != null)
            {
                FirebaseException firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                AuthError error = (AuthError)firebaseException.ErrorCode;

                string errorMessage = "Unknown Error : Please try again later";

                switch (error)
                {
                    case AuthError.Cancelled:
                        errorMessage = "Email Verification Was Cancelled";
                        break;
                    case AuthError.TooManyRequests:
                        errorMessage = "Too Many Request";
                        break;
                    case AuthError.InvalidRecipientEmail:
                        errorMessage = "The Email You Entered Is Invalid";
                        break;
                }

                ManagerMenu.Instance.ShowVerificationResponse(false, user.Email, errorMessage);
            }
            else 
            {
                Debug.Log("Email has successfully sent");
                ManagerMenu.Instance.ShowVerificationResponse(true, user.Email, null);
            }
        }
    }
    #endregion

    private void UpdateProfilePicture()
    {
        StartCoroutine(UpdateProfilePictureIE());
    }

    private IEnumerator UpdateProfilePictureIE()
    {
        if (user != null)
        {
            string url = ManagerMenu.Instance.GetProfileUpdateURL();
            UserProfile profile = new UserProfile() { PhotoUrl = new Uri(url) };

            var profileUpdateTask = user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(() => profileUpdateTask.IsCompleted);

            if (profileUpdateTask.Exception != null)
            {
                Debug.LogError(profileUpdateTask.Exception);
            }
            else
            {
                ManagerMenu.Instance.LoadProfileImage(user.PhotoUrl.ToString());
            }
        }
    }
}

