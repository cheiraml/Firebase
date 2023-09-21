using Firebase.Auth;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLogin : MonoBehaviour
{
    [SerializeField]
    private Button _loginButton;

    [SerializeField]
    private TMP_InputField _emailInputField;
    [SerializeField]
    private TMP_InputField _passwordInputField;

    void Reset()
    {
        _loginButton = GetComponent<Button>();
        _emailInputField = GameObject.Find("InputEmail").GetComponent<TMP_InputField>();
        _passwordInputField = GameObject.Find("InputPassword").GetComponent<TMP_InputField>();
    }

    void Start()
    {
        _loginButton.onClick.AddListener(HandleLoginButtonClicked);
        _passwordInputField.contentType = TMP_InputField.ContentType.Password;
    }

    private void HandleLoginButtonClicked()
    {
        // Comprobar si el correo y la contraseña están vacíos
        if (string.IsNullOrEmpty(_emailInputField.text) || string.IsNullOrEmpty(_passwordInputField.text))
        {
            Debug.LogError("Por favor, ingrese su correo y contraseña.");
            return;
        }

        var auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(_emailInputField.text, _passwordInputField.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }
}
