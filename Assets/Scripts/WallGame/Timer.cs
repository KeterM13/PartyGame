using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public float minutes;
    public float seconds;
    public float time;

    public TextMeshProUGUI timerText;
    

    private void OnDestroy()
    {
        instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        //time = minutes * 60 + seconds;
    }

    
    void Update()
    {
        time += Time.deltaTime;
        timerText.text = time.ToString();
        timerText.text = $"{Mathf.FloorToInt(time / 60f):00}:{time % 60:00}";
       

    }
}
