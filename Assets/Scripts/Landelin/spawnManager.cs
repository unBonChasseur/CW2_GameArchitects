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
        if (day_script.getTimeOfDay() < 6 || day_script.getTimeOfDay() > 20)
        {
            if (!timer.IsRunning)
            {
                int rand = Random.Range(0, spawnPoints.Length - 1 ) ;
                Instantiate(zombie, spawnPoints[rand]);
                timer.Start();
            }
            if(timer.Elapsed.TotalMilliseconds > delay)
            {
                timer.Stop();
                timer.Reset();
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
