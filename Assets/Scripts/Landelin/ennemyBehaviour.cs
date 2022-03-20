using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ennemyBehaviour : MonoBehaviour
{
    private ennemyStatus status;
    private GameObject[] plants;
    private int rand; 

    private void Start()
    {
        status = this.GetComponent<ennemyStatus>();
    }

    private void Update()
    {
        
    }
}
