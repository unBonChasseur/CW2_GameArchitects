using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemyStatus : MonoBehaviour
{
    [SerializeField] private int max_health_points;
    [SerializeField] private int speed;

    private int current_health_points;

    [SerializeField] private GameObject target;

    private void Awake()
    {
        current_health_points = max_health_points; 
    }

    public void updateHealthPoints(int damage)
    {
        current_health_points += damage; 
        if(current_health_points <= 0)
        {
            target.GetComponent<plantStatus>().updateisTargeted(false);
            Destroy(gameObject);
        }
    }

    public int getHealthPoints()
    {
        return current_health_points;
    }

    public int getSpeed()
    {
        return speed;
    }

    public GameObject getTarget()
    {
        return target; 
    }

    public void updateTarget(GameObject newTarget)
    {
        target = newTarget; 
    }
    
}
