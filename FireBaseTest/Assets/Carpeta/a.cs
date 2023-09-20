using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class a : MonoBehaviour
{
    [SerializeField] private Button _registrationButton;
    private Coroutine _registrationCoroutine;
    private DatabaseReference mDatabaseRef;

    void Reset()
    {
        _registrationButton = GetComponent<Button>();
    }

    void Start()
    {
        _registrationButton.onClick.AddListener(HandleRegisterButtonClicked);
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void HandleRegisterButtonClicked()
    {
        string email = GameObject.Find("InputEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("InputPassword").GetComponent<TMP_InputField>().text;

        _registrationCoroutine = StartCoroutine(RegisterUser(email, password));
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.IsCanceled)
        {
            Debug.LogError($"CreateUserWithEmailAndPasswordAsync is canceled");
        }
        else if (registerTask.IsFaulted)
        {
            Debug.LogError($"CreateUserWithEmailAndPasswordAsync encountered error" + registerTask.Exception);
        }
        else
        {
            Firebase.Auth.AuthResult result = registerTask.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

            string name = GameObject.Find("InputUser").GetComponent<TMP_InputField>().text;

            mDatabaseRef.Child("users").Child(result.User.UserId).Child("username").SetValueAsync(name);
        }
    }
}

