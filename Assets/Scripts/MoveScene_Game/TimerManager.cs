using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoSingleton<TimerManager>
{
    int timer = 0;
    public Text timeText;
    private void Awake()
    {
        timer = 0;
      //  timeText = transform.Find("Time").GetComponent<Text>();
        StartTimer();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //
    public void StartTimer()
    {
        timer = 0;
        StartCoroutine("OpenTimer");
    }


    public void StopTimer()
    {

        StopCoroutine("OpenTimer");
        
    }

    public void PauseTime()
    {
        StopCoroutine("OpenTimer");
    }

    public void ContinueTime()
    {
        StartCoroutine("OpenTimer");
    }


    //开启时间
    IEnumerator OpenTimer()
    {
        
        timeText.text = string.Format("{0:D2}:{1:D2}", timer / 60, timer % 60);
        while (timer >= 0)
        {
            yield return new WaitForSeconds(1);
            timer++;
            timeText.text = string.Format("{0:D2}:{1:D2}", timer / 60, timer % 60);
        }
    }

}
