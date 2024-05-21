using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;
    public bool isCoopMode = false;
    [SerializeField]
    private GameObject _pauseMenuPanel;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(2);  
        }
        if (Input.GetKeyDown(KeyCode.R) && isCoopMode == true)
        {
            SceneManager.LoadScene(1);  
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0); 
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenuPanel.SetActive(true);
        }
        
    }
    public void GameOver()
    {
        _isGameOver = true;
    }

    // Start is called before the first frame update
}
