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


    private Stopwatch timer;

    private GameObject[] plants; 


    private void Awake()
    {
        day_script =  day_obj.GetComponent<LightingManager>();
        timer = new Stopwatch();
        StartCoroutine(c_updatePlants());
    }

    private void Update()
    {
        if (day_script.getTimeOfDay() < startOfDay || day_script.getTimeOfDay() > endOfDay)
        {
            if(plants.Length > 0)
            { 
                if (!timer.IsRunning)
                {
                    foreach (GameObject plant in plants)
                    {
                        if (plant.GetComponent<plantStatus>().getisTargeted() == false)
                        {
                            plant.GetComponent<plantStatus>().updateisTargeted(true);
                            int rand = Random.Range(0, spawnPoints.Length - 1);
                            GameObject newZ = Instantiate(zombie, spawnPoints[rand]);
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
}
