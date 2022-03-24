using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemyBehaviour : MonoBehaviour
{
    private ennemyStatus status; 

    private void Start()
    {
        status = this.GetComponent<ennemyStatus>();
    }

    private void Update()
    {
        //Compute path to target 
        //if ennemy arrived -> destroy target 
    }

}
