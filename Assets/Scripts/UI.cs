using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI Instance;

    [Header("Panels")]
    public GameObject Menu;
    public GameObject Game;
    public GameObject Editor;

    [Header("Components")]
    public TMP_Text score;
    public TMP_Text level;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        Menu.SetActive(true);
        Game.SetActive(false);
        Editor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCLickPlay()
    {
        Menu.SetActive(false);
        Game.SetActive(true);
        SceneManager.LoadScene("Game");
    }

    public void OnClickEdit()
    {
        Menu.SetActive(false);
        Editor.SetActive(true);
        SceneManager.LoadScene("Editeur");
    }
    
    public void OnClickQuit()
    {
        
    }
    
    public void ShowHit(int hits)
    {
        score.text = "Hits : " + hits;
    }
    
    public void ShowLevelName(string name)
    {
        level.text = "Level : " + name;
    }
}
