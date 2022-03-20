using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantStatus : MonoBehaviour
{
    [SerializeField] private float nutritiveValue; 
    [SerializeField] private float growingTime;
    private float currentTime; 
    void Start()
    {
        StartCoroutine(countDown(growingTime * 60)); 
    }

    private IEnumerator countDown(float time)
    {
        currentTime = time;
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentTime--;
            this.transform.localScale += new Vector3(1, 1, 1) * 1 /(growingTime * 60);
        }
    }

    public float getGrowingTime()
    {
        return growingTime;
    }

    public float getNutritiveValue()
    {
        return nutritiveValue;
    }

    public float getCurrentTime()
    {
        return currentTime;
    }
}
