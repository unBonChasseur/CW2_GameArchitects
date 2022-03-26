using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableLog : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float distance;
    [SerializeField] private LayerMask target;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject UI_obj; 
    void Start()
    {
        UI_obj.SetActive(false);
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

            //Interact with the item 

            if (Input.GetKeyUp(KeyCode.E))
            {
                player.GetComponent<playerStatus>().updateWood(1);
                //player.GetComponent<PlayerController>.StartCoroutine(InteractAnimation(0));
            }

        }
        else
        {
            UI_obj.SetActive(false);
        }
    }
}
