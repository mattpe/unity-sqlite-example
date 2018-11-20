using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

/// <summary>
/// Example class for SQLite usage in Unity
/// Based on https://ornithoptergames.com/how-to-set-up-sqlite-for-unity/
/// </summary>
public class SQLiteExample : MonoBehaviour
{

    private string dbPath;

    private void Start()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/exampleDB.db";
        Debug.Log("Database path: " + dbPath);
        CreateSchema();
    }

    public void CreateSchema()
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'high_score' ( " +
                                    "  'id' INTEGER PRIMARY KEY, " +
                                    "  'name' TEXT NOT NULL, " +
                                    "  'score' INTEGER NOT NULL" +
                                    ");";

                var result = cmd.ExecuteNonQuery();
                Debug.Log("create schema: " + result);
            }
        }
    }

    public void InsertScore(string highScoreName, int score)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO high_score (name, score) " +
                                    "VALUES (@Name, @Score);";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Name",
                    Value = highScoreName
                });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Score",
                    Value = score
                });

                var result = cmd.ExecuteNonQuery();
                Debug.Log("insert score: " + result);
            }
        }
    }

    public string GetHighScores(int limit)
    {
        using (var conn = new SqliteConnection(dbPath))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM high_score ORDER BY score DESC LIMIT @Count;";

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Count",
                    Value = limit
                });

                var highScoresText = "";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var highScoreName = reader.GetString(1);
                    var score = reader.GetInt32(2);
                    var text = string.Format("{0}: {1} [#{2}] \n", highScoreName, score, id);
                    //Debug.Log(text);
                    highScoresText += text;
                }
                return highScoresText;
            }
        }
    }
}

