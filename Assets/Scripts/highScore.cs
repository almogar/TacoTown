using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class highScore : MonoBehaviour
{
    private List<int> ScoreArray = new List<int>();
    public Text score1;
    public Text score2;
    public Text score3;
    public bool finishGame;
    

    // Start is called before the first frame update
    void Start()
    {
        loadScore();
        ScoreArray.Sort();
        score1.text = "" + ScoreArray[2]; ;
        score2.text = "" + ScoreArray[1];
        score3.text = "" + ScoreArray[0];
    }

    void Update()
    {
     
        
    }

    private void loadScore()
    {
        string name = "Score";
        for (int i = 0; i < 3; i++)
            ScoreArray.Add(PlayerPrefs.GetInt(name + i, 0));
    }

}
