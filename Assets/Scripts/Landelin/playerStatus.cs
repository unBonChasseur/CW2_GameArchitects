using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatus : MonoBehaviour
{
    private float hunger; 
    private int wood;

    private void Awake()
    {
        wood = 0;
    }

    public void updateHunger(float damage)
    {
        hunger += damage;
    }
    public float getHunger()
    {
        return hunger;
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
}
