using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupValue : MonoBehaviour
{
    // Start is called before the first frame update
    private Toggle[] toggles;
    private string Value= "Boy";
    void Start()
    {
        toggles = transform.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)

        {

            Toggle toggle = toggles[i];

            toggle.onValueChanged.AddListener((bool value) => OnToggleClick(toggle, value));

        }

    }

    //功能：点击选中某一个toggle时播放相对应的视频

    void OnToggleClick(Toggle toggle, bool isSwitch)
    {
        
        if (isSwitch)// 选择状态

        {

            Value = toggle.name;



        }

        else
        {

           

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public string GetToggleValue()
    {
        return Value;
    }
}
