using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public List<TextMeshProUGUI> usernameTexts; // Lista de TextMeshProUGUI para nombres de usuario
    public List<TextMeshProUGUI> scoreTexts;    // Lista de TextMeshProUGUI para puntajes

    public void UpdateUI(Dictionary<string, int> leaderboardData)
    {
        int i = 0; // Índice para recorrer las listas de TextMeshProUGUI

        foreach (var kvp in leaderboardData)
        {
            string username = kvp.Key;
            int userScore = kvp.Value;

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

