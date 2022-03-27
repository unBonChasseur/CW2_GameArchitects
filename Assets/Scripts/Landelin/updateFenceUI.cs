using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateFenceUI : MonoBehaviour
{
    [SerializeField] private Text currentLevel;
    [SerializeField] private Text nbPlanks; 
    [SerializeField] private Text t_currentHP;
    [SerializeField] private Slider s_currentHP;

    private fenceStatus status; 

    private void Start()
    {
        status = this.GetComponent<fenceStatus>();
        s_currentHP.maxValue = status.getLevelMaxHP();
        s_currentHP.value = status.getCurrentHP();
    }

    private void Update()
    {
        currentLevel.text = "Level " + status.getCurrentLevel().ToString() + " / " + status.getMaxLevel();
        t_currentHP.text = status.getCurrentHP().ToString() + " / " + status.getLevelMaxHP();
        s_currentHP.value = status.getCurrentHP();
        if (status.getNbPlanksToUpgrade() != -1)
            nbPlanks.text =  "<F> to upgrade ! (cost " + status.getNbPlanksToUpgrade().ToString() + ")";
        else
            nbPlanks.text = "MAX Level";
    }
}
