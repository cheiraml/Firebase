using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UsernameLabel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    private void Reset()
    {
        _label = GetComponent<TMP_Text>();
    }
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthChange;

    }

    private void HandleAuthChange(object sender, EventArgs e)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if(currentUser != null)
        {
            _label.text = currentUser.Email;
        }
    }

   
}
