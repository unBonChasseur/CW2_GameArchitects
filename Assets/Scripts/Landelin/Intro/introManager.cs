using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introManager : MonoBehaviour
{
    [SerializeField] private float time;
    private float currentTime; 
    private void Awake()
    {
        StartCoroutine(countDown(time));
    }
    private void Update()
    {
        if(currentTime <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }


    IEnumerator countDown(float timing)
    {
        currentTime = timing;
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;
        }
    }
}
