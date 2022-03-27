using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fenceStatus : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int[] max_HP;
    [SerializeField] private int[] nbPlanksToUpgrade;
    private int currentHP;
    private int currentLevel;

    private void Start()
    {
        currentHP = max_HP[0];
    }

    public void updateCurrentHP(int damage)
    {
        currentHP += damage;
        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void updateCurrentLevel()
    {
        if (currentLevel == (max_HP.Length - 1))
            return;
        currentLevel++;
        currentHP = max_HP[currentLevel];
    }
    public int getCurrentHP()
    {
        return currentHP; 
    }

    public int getCurrentLevel()
    {
        return currentLevel + 1; 
    }

    public int getMaxLevel()
    {
        return max_HP.Length; 
    }
    public int getLevelMaxHP()
    {
        return max_HP[currentLevel];
    }

    public int getNbPlanksToUpgrade()
    {
        if (currentLevel >= nbPlanksToUpgrade.Length)
            return -1;
        return nbPlanksToUpgrade[currentLevel];
    }

    //function repair
}
