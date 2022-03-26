using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableFence : MonoBehaviour
{

    [SerializeField] private float distance;
    [SerializeField] private LayerMask target;
    [SerializeField] private GameObject UI_obj;

    private GameObject player;
    void Start()
    {
        UI_obj.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        checkZone();
    }

    private void checkZone()
    {
        Collider[] zone = Physics.OverlapSphere(transform.position, distance, target);

        if (zone.Length != 0)
        {
            //displayUI 
            UI_obj.SetActive(true);
            UI_obj.transform.rotation = Quaternion.Euler(90.0f, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z);

        }
        else
        {
            UI_obj.SetActive(false);
        }
    }
}
