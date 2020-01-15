using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassChose : MonoBehaviour
{
    public Button[] btns;

    public Button Quit;
    void Awake()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(delegate { ChoseClass(btn.name); });
        }

        Quit.onClick.AddListener(delegate { QuitGame(); });
    }

    private void QuitGame()
    {
        Tool.Instance.Quit();
    }

    private void ChoseClass(string BtnName)
    {
        Debug.Log(BtnName);
        PlayerPrefs.SetString("ClassType", BtnName.Trim());
        Tool.Instance.LoadScene(Util.ShowCourseScene);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
