using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorTip : MonoBehaviour
{
    // Start is called before the first frame update
    public Text textTip;
    public Button closeTip;
    void Awake()
    {
        closeTip.onClick.AddListener(delegate { Hide(); });
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Show(string message)
    {
        
        gameObject.SetActive(true);
        textTip.text = message;
    }
}
