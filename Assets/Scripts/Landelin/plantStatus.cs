using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantStatus : MonoBehaviour
{
    [SerializeField] private float nutritiveValue; 
    [SerializeField] private float growingTime;
    [SerializeField] private GameObject plant;
    private float currentTime;


    private bool m_isWatered = false;
    private float m_wateredTime = 0;
    private bool isTargeted = false;

    private TileStatus tile; 

    void Start()
    {
        StartCoroutine(countDown(growingTime * 60));
    }

    private IEnumerator countDown(float time)
    {
        currentTime = time;
        WaitForSeconds wait = new WaitForSeconds(1.0f);

        while (currentTime > 0)
        {
            yield return wait;

            if (m_isWatered)
            {
                currentTime -= 2;
                plant.transform.localScale += new Vector3(1, 1, 1) * 2 / (growingTime * 60);
            }

            else
            {
                currentTime--;
                plant.transform.localScale += new Vector3(1, 1, 1) * 1 / (growingTime * 60);
            }
        }

        currentTime = 0;
    }

    private IEnumerator DryPlant()
    {
        WaitForSeconds wait = new WaitForSeconds(1.0f);
        while(m_wateredTime > 0)
        {
            yield return wait;
            m_wateredTime--;
        }

        m_isWatered = false;
    }

    public void WaterPlant(float wateredTime)
    {
        m_wateredTime = wateredTime;

        if (!m_isWatered)
        {
            m_isWatered = true;
            StartCoroutine(DryPlant());
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

    public bool getisTargeted()
    {
        return isTargeted; 
    }

    public void updateisTargeted(bool state)
    {
        isTargeted = state; 
    }

    public void setTile(TileStatus newTile)
    {
        tile = newTile;
    }

    public void destroyPlant()
    {
        tile.destroyInstancePlant();
    }
}
