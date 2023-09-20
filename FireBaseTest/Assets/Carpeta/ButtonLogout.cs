using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonLogout : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string _scnenetoLoad = "Home";
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData)
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene(_scnenetoLoad);

    }
}