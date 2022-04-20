using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    Text resText;
    // Start is called before the first frame update
    void Start()
    {
        resText = this.GetComponent<Text>();
        if (PlayerPrefs.GetInt("Result")%2 == 1)
        {
            resText.text = "Player 1 Wins";
        }
        else
        {
            resText.text = "Player 2 Wins";
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
