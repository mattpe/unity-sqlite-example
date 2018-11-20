using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private Button getScoresButton;
    private Button addScoreButton;
    private InputField nameInput;
    private InputField scoreInput;

    private Text scoresText;
    private SQLiteExample scoreDB;

    void Start () {
        // Initialize UI 
        scoreDB = GetComponent<SQLiteExample>();
        getScoresButton = GameObject.Find("ButtonGetScores").GetComponent<Button>();
        addScoreButton = GameObject.Find("ButtonAddScore").GetComponent<Button>();
        scoresText = GameObject.Find("TextScores").GetComponent<Text>();
        nameInput = GameObject.Find("InputName").GetComponent<InputField>();
        scoreInput = GameObject.Find("InputScore").GetComponent<InputField>();
        getScoresButton.onClick.AddListener(() => ShowScores());
        addScoreButton.onClick.AddListener(() => AddScore());

    }

    private void ShowScores() {
        // Show top ten
        scoresText.text = scoreDB.GetHighScores(10);
    }

    private void AddScore() {

        string name = nameInput.text;
        int score = 0;
        int.TryParse(scoreInput.text, out score);

        // Save input data only if valid
        if (name.Length > 2 && score > 0) {
            Debug.Log("Saving score: " + name + ": " + score);
            scoreDB.InsertScore(name, score);
        }
        ShowScores();
    }

}
