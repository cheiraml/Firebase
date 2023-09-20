using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class GestorFirebase : MonoBehaviour
{
    private DatabaseReference reference;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    public void GuardarPuntaje(string jugador, int puntaje)
    {
        if (reference != null)
        {
            reference.Child("puntajes").Child(jugador).SetValueAsync(puntaje);
        }
    }
}
