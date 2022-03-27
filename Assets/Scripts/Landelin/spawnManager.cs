using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class spawnManager : MonoBehaviour
{
    [SerializeField] private GameObject zombie; 
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float delay;
    [SerializeField] private GameObject day_obj;
    private LightingManager day_script;
    [SerializeField] private float startOfDay;
    [SerializeField] private float endOfDay;

    [SerializeField] private GameObject Audio_obj;
    private AudioManager c_audio; 
    private Stopwatch timer;

    [SerializeField] private GameObject[] plants;

    private bool newDay; 


    private void Start()
    {
        c_audio = Audio_obj.GetComponent<AudioManager>();
        day_script =  day_obj.GetComponent<LightingManager>();
        timer = new Stopwatch();
        StartCoroutine(c_updatePlants());

        newDay = false;
    }

    private void Update()
    {
        musics();
        dispawnEnnemiesOnDaylight();

        if (day_script.getTimeOfDay() < startOfDay || day_script.getTimeOfDay() > endOfDay)
        {
            newDay = true;
            if(plants.Length > 0)
            { 
                if (!timer.IsRunning)
                {
                    foreach (GameObject plant in plants)
                    {
                        if (plant && plant.GetComponent<plantStatus>().getisTargeted() == false)
                        {
                            plant.GetComponent<plantStatus>().updateisTargeted(true);
                            int rand = Random.Range(0, spawnPoints.Length - 1);
                            GameObject newZ = Instantiate(zombie, spawnPoints[rand].transform.position, Quaternion.identity);
                            newZ.GetComponent<ennemyStatus>().updateTarget(plant);
                            timer.Start();
                            break;
                        }
                    }
                }
                if (timer.Elapsed.TotalMilliseconds > delay)
                {
                    timer.Stop();
                    timer.Reset();
                }
            }
            
        }
    }

    IEnumerator c_updatePlants()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            updatePlants();
        }
    }

    private void updatePlants()
    {
        plants = GameObject.FindGameObjectsWithTag("Plant");
        return; 
    }

    private void musics()
    {
        if (day_script.getTimeOfDay() < startOfDay || day_script.getTimeOfDay() > endOfDay)
        {
            //night
            if (c_audio.checkIsPlaying("Night") == true)
                return;
            else
            {
                c_audio.StopPlay("Day");
                c_audio.Play("Night");
            }
        }
        else
        {
            //day
            if (c_audio.checkIsPlaying("Day") == true)
                return;
            else
            {
                c_audio.StopPlay("Night");
                c_audio.Play("Day");
            }
        }
    }

    private void dispawnEnnemiesOnDaylight()
    {
        if (day_script.getTimeOfDay() > startOfDay && day_script.getTimeOfDay() < endOfDay)
        {
            if (newDay == true)
            {
                newDay = false;
                GameObject[] zombies = GameObject.FindGameObjectsWithTag("Ennemy");
                foreach (GameObject zombie in zombies)
                {
                    zombie.GetComponent<ennemyStatus>().death();
                }

            }
            else
                return;
        }
        else
            return;
    }
}
