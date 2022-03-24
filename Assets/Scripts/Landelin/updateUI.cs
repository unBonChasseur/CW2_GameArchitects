using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class updateUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    private playerStatus status;

    [SerializeField] private Text wood; 
    void Start()
    {
        status = player.GetComponent<playerStatus>(); 
    }

    // Update is called once per frame
    void Update()
    {
        wood.text = status.getWood().ToString();
    }
}
