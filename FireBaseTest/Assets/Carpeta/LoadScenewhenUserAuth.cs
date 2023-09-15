using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenewhenUserAuth : MonoBehaviour
{
    [SerializeField]
    private string _scnenetoLoad = "GameScene";

    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }
    private void HandleAuthStateChange(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene(_scnenetoLoad);
        }
    }
    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChange;
    }
}
