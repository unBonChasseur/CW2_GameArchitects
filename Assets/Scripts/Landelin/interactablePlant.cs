using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactablePlant : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
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

            //Interact with the item 

            if (Input.GetKeyUp(KeyCode.E) && this.GetComponent<plantStatus>().getGrowingTime() <= 0)
            {
                //gather the plant
                player.GetComponent<playerStatus>().updateHunger(this.GetComponent<plantStatus>().getNutritiveValue());
                Destroy(gameObject);
            }

        }
        else
        {
            UI_obj.SetActive(false);
        }
    }
}
