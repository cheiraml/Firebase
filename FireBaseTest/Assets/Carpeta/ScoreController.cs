using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScoreController : MonoBehaviour
{
    DatabaseReference mDatabase;
    string UserId;

    int score = 0;
    public TextMeshProUGUI puntajeText;
    public List<TextMeshProUGUI> usernameTexts; // Lista de TextMeshProUGUI para nombres de usuario
    public List<TextMeshProUGUI> scoreTexts;    // Lista de TextMeshProUGUI para puntajes

    // Evento que se dispara cuando el puntaje se actualiza
    public event Action<int> OnScoreUpdated;
    private userdatsuper[] user_to_deploy = new userdatsuper[5];
    private Dictionary<string, int> all_users = new Dictionary<string, int>();

    void Start()
    {
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
        UserId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        GetUserScore();

        // Llama a la función para cargar los puntajes iniciales
        GetUsersHighestScores();

        // Configura una escucha continua para actualizar el leaderboard en tiempo real
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .OrderByChild("score").LimitToLast(5)
            .ValueChanged += HandleLeaderboardValueChanged;
    }

    public void WriteNewScore(int newScore)
    {
        mDatabase.Child("users").Child(UserId).Child("score").SetValueAsync(newScore);
    }

    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + UserId + "/score")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string _score = "" + snapshot.Value;
                    score = int.Parse(_score);
                    //Debug.Log("Score: " + score);
                    setLabel();
                }
            });
    }

    // Actualiza el puntaje solo si es mayor que el puntaje actual
    public void ActualizarScore(int amount)
    {
        if (amount > score)
        {
            score = amount;
            puntajeText.text = "Score: " + score;
            WriteNewScore(score);

            // Disparar el evento para notificar que el puntaje se ha actualizado
            OnScoreUpdated?.Invoke(score);

            // Actualiza el leaderboard después de actualizar el puntaje
            
        }

        ActualizarLeaderboard();
    }

    public void setLabel()
    {
        //Debug.Log("Set label");
        puntajeText.text = "Score: " + score;
    }

    // Función para obtener el puntaje actual
    public int GetPuntaje()
    {
        return score;
    }

    // Función para manejar cambios en el leaderboard en tiempo real
    void HandleLeaderboardValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
        {
            Dictionary<string, int> leaderboardData = new Dictionary<string, int>();
            int i = 0; // Índice para recorrer las listas de TextMeshProUGUI

            foreach (var userDoc in (Dictionary<string, object>)args.Snapshot.Value)
            {
                var userObject = (Dictionary<string, object>)userDoc.Value;
                if (userObject.ContainsKey("score"))
                {
                    string username = userObject["username"].ToString();
                    int userScore = int.Parse(userObject["score"].ToString());
                    leaderboardData.Add(username, userScore);

                    // Actualiza los TextMeshProUGUI correspondientes en la UI
                    if (i < usernameTexts.Count && i < scoreTexts.Count)
                    {
                        usernameTexts[i].text = username;
                        scoreTexts[i].text = userScore.ToString();
                        i++;
                    }
                }
            }

        }
    }

    // Función para actualizar el leaderboard cuando sea necesario
    public void ActualizarLeaderboard()
    {
        // Actualiza el puntaje en Firebase
        //WriteNewScore(score);

        // También puedes forzar una actualización inmediata del leaderboard
        GetUsersHighestScores();
    }

    // Función para obtener los puntajes más altos
    public void GetUsersHighestScores()
    {

        FirebaseDatabase.DefaultInstance
            .GetReference("users").OrderByChild("score").LimitToLast(5)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                    {
                        var userObject = (Dictionary<string, object>)userDoc.Value;
                        string username = (string)userObject["username"];
                        int score = Convert.ToInt32(userObject["score"]);
                        if (all_users.ContainsKey(username))
                        {
                            if (all_users[username] < score) 
                            {
                                all_users[username] = score;
                            }
                        }
                        else 
                        {
                            all_users.Add(username, score);
                        }
                       
                    }

                    var sortedUsers = all_users.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    var list_Users_Name = sortedUsers.Keys.ToList();
                    var list_Users_Score = sortedUsers.Values.ToList();

                for (int i = 0; i < sortedUsers.Count || i < 5; i++)
                {
                    userdatsuper user = new userdatsuper();
                        user.username = list_Users_Name[i];
                        user.score = list_Users_Score[i];
                        user_to_deploy[i] = user;
                        usernameTexts[i].text = user_to_deploy[i].username;
                        scoreTexts[i].text = user_to_deploy[i].score.ToString();

                }



            }

        });
        
        

        
    }
}

[System.Serializable]
public class userdatsuper 
{
    public string username;
    public int score;
}
