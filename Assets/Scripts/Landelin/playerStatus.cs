using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatus : MonoBehaviour
{
    [SerializeField] private float maxHunger;
    [SerializeField] private float hunger;
    [SerializeField] private int wood;
    [SerializeField] private int m_initWater;
    [SerializeField] private int m_water;

    private void Awake()
    {
        m_initWater = 3;
        wood = 0;
        m_water = 0;
        hunger = maxHunger;
        StartCoroutine(HungerGame());
    }

    public void updateHunger(float damage)
    {
        hunger += damage;
    }
    public float getHunger()
    {
        return hunger;
    }

    public float getMaxHunger()
    {
        return maxHunger;
    }
    public void updateWood(int amount)
    {
        if (amount < 0 && wood - amount < 0)
            return;
        wood += amount;
    }

    public int getWood()
    {
        return wood; 
    }

    public void decreaseWater(int amount)
    {
        m_water -= amount;
    }

    public void resetWater()
    {
        m_water = m_initWater;
    }

    public int getWater()
    {
        return m_water;
    }

    public int getMaxWater()
    {
        return m_initWater;
    }

    private IEnumerator HungerGame()
    {
        WaitForSeconds wait = new WaitForSeconds(1);

        while(hunger > 0)
        {
            yield return wait;
            hunger--;
        }
        Debug.Log("EndGame");
    }

}
