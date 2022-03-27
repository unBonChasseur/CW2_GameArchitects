using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updatePlantUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text t_timer;
    [SerializeField] private Slider s_timer;
    [SerializeField] private Text t_Interaction;

    private plantStatus status;

    void Start()
    {
        status = this.GetComponent<plantStatus>();
        s_timer.maxValue = status.getGrowingTime() * 60;
        s_timer.value = status.getCurrentTime();
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = 0, minutes = 0;
        minutes = Mathf.FloorToInt(status.getCurrentTime()) / 60;
        if(minutes > 0)
        {
            seconds = Mathf.FloorToInt(status.getCurrentTime()) - (minutes * 60);
            t_timer.text = minutes.ToString() + ":" + seconds.ToString();
            
        }
        else
        {
            t_timer.text = Mathf.FloorToInt(status.getCurrentTime()).ToString();
        }

        if (status.getCurrentTime() == 0)
            t_Interaction.text = "<E> to Harvest";
        else
            t_Interaction.text = "<E> to Water";

        s_timer.value = status.getCurrentTime();
    }
}
