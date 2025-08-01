using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Firebase_Auth_Manager : MonoBehaviour
{
    public PlayerInformation _curentInfor;
    FirebaseDatabaseManager _databaseManager;
    FirebaseUser user;
    private DatabaseReference reference;

    //[Header("Hiệu ứng")]
    //public AudioClip clickSound;
    //public AudioSource audioSource;

    [Header("Register")]
    public InputField ipRegisterEmail;
    public InputField ipRegisterPassword;
    public InputField ipRePassword;

    public Button buttonRegister;

    [Header("Sign In")]
    public InputField ipLoginEmail;
    public InputField ipLoginPassword;

    public Button buttonLogin;

    private FirebaseAuth auth; // quản lí đk, đn.


    [Header("Switch Form")]
    public Button buttonMoveToSignIn;
    public Button buttonMoveToRegister;

    public GameObject loginForm;
    public GameObject registerForm;

    public Text warningText;
    public GameObject _cvWarning;

    private void Start()
    {
       // _cvWarning.SetActive(false);
        auth = FirebaseAuth.DefaultInstance;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //buttonRegister.onClick.AddListener(RegisterAccountWithFirebase);
        //buttonLogin.onClick.AddListener(SignInAccountWithFirebase);

        //buttonMoveToRegister.onClick.AddListener(SwitchForm);
        //buttonMoveToSignIn.onClick.AddListener(SwitchForm);
        buttonRegister.onClick.AddListener(() => OnButtonClick(RegisterAccountWithFirebase));
        buttonLogin.onClick.AddListener(() => OnButtonClick(SignInAccountWithFirebase));
        buttonMoveToRegister.onClick.AddListener(() => OnButtonClick(SwitchForm));
        buttonMoveToSignIn.onClick.AddListener(() => OnButtonClick(SwitchForm));
    }

    public void RegisterAccountWithFirebase()
    {
        string email = ipRegisterEmail.text.Trim();
        string password = ipRegisterPassword.text.Trim();
        string confirmPassword = ipRePassword.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            warningText.text = "Email hoặc mật khẩu không được để trống!";
            _cvWarning.SetActive(true);
            return;
        }

        if (password.Length < 6)
        {
            warningText.text = "Mật khẩu phải có ít nhất 6 ký tự!";
            _cvWarning.SetActive(true);
            return;
        }

        if (password != confirmPassword)
        {
            warningText.text = "Mật khẩu không khớp!";
            _cvWarning.SetActive(true);
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log(" Đăng kí bị hủy");
                _cvWarning.SetActive(true);
                return;

            }
            if (task.IsFaulted)
            {
                warningText.text = "Đăng kí thất bại";
                _cvWarning.SetActive(true);
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log(" Đăng kí thành công!");
                warningText.text = "Đăng ký thành công!";

                SwitchForm();
                _cvWarning.SetActive(true);
                FirebaseUser newUser = task.Result.User;

                SaveUserData(newUser);
                //string defaultCharacterName = "Hero";
                //int defaultGold = 0; 
                //int defaultHealth = 200; 
                //int defaultEnergy = 100;
                //_databaseManager.WriteDatabase(user.UserId, defaultHealth, defaultEnergy);
            }
        });
    }

    public void SignInAccountWithFirebase()
    {
        string email = ipLoginEmail.text.Trim();
        string password = ipLoginPassword.text.Trim();

        //Debug.Log("Email: " + email);
        //Debug.Log("Email: " + password);
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            warningText.text = "Email hoặc mật khẩu không được để trống!";
            _cvWarning.SetActive(true);
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                warningText.text = "Đăng nhập bị hủy";
                return;

            }
            else if (task.IsFaulted)
            {
                warningText.text = "Đăng nhập thất bại. Vui lòng kiểm tra email/mật khẩu!";
                _cvWarning.SetActive(true);
                return;
            }
            else if (task.IsCompleted)
            {
                Debug.Log(" Đăng nhập thành công!");
                warningText.text = "Đăng nhập thành công!";
                _cvWarning.SetActive(true);
                FirebaseUser user = task.Result.User;
                EnsureUserDataExists(user);

                StartCoroutine(DelayedSceneLoad());
                // SceneManager.LoadScene("Vu_Menu");
            }
        });
    }

    public void SwitchForm()
    {
        _cvWarning.SetActive(false);
        loginForm.SetActive(!loginForm.activeSelf);
        registerForm.SetActive(!registerForm.activeSelf);
    }

    private void SaveUserData(FirebaseUser user)
    {

        if (string.IsNullOrEmpty(user.UserId))
        {
            return;
        }

        if (reference == null)
        {
            return;
        }

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
             { "Name", user.Email },
             { "Health", 200 },
             { "Energy", 100 }
        };

        reference.Child("Users").Child(user.UserId).SetValueAsync(userData).ContinueWithOnMainThread(task =>
        {
        });
    }

    private void EnsureUserDataExists(FirebaseUser user)
    {
        if (user == null) return;

        reference.Child("Users").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.Result.Exists)
            {
                Dictionary<string, object> userData = new Dictionary<string, object>
            {
                 { "Name", user.Email },
                 { "Health", 200 },
                 { "Energy", 100 }
            };

                reference.Child("Users").Child(user.UserId).SetValueAsync(userData).ContinueWithOnMainThread(saveTask =>
                {
                    if (saveTask.IsCompletedSuccessfully)
                    {
                    }
                });
            }
        });
    }

    IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(1f);
       // Loading_Script.NEXT_SCENE = "StoryScene";
        //SceneManager.LoadScene("Loading_Story");
    }
    void OnButtonClick(System.Action action)
    {
        // Phát âm thanh
       // audioSource.PlayOneShot(clickSound);

        // Gọi hành động tương ứng
        action.Invoke();
    }
}
