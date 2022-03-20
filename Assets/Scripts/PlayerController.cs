using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Hashes useful for animations
    [Header("Animations")]
    private Animator m_animator;
    private int m_walkingHash;

    [Header("Movement")]
    [SerializeField]
    private float m_speed;
    [SerializeField]
    private CharacterController m_controller;
    [SerializeField]
    private Transform m_playerTransform;
    private float rotation = 0f;

    [Header("Ground")]
    private RaycastHit hit;

    [Header("interactables")]
    [SerializeField]
    private GameObject[] Fence;
    [SerializeField]
    private GameObject[] Gate;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_walkingHash = Animator.StringToHash("IsWalking");
    }

    // Update is called once per frame
    void Update()
    {
        // Control forward/backward
        float z = 0f;
        z = Input.GetAxis("Vertical");

        // Control Left/Right
        float x = 0f;
        x = Input.GetAxis("Horizontal");

        bool IsWalking = m_animator.GetBool("IsWalking");

        // horizontal
        if(x == 1)
        {
            // up
            if(z == 1)
                rotation = 45f;

            // down
            else if (z == -1)
                rotation = 135f;
            
            // only
            else if(z == 0)
                rotation = 90f;
        }
        else if (x == -1)
        {
            // up
            if (z == 1)
                rotation = 315f;

            // down
            else if (z == -1)
                rotation = 225f;

            // only
            else if (z == 0)
                rotation = 270f;
        }
        else
        {
            if (z == -1)
                rotation = 180f;

            else if (z == 1)
                rotation = 0f;

            else if (rotation % 90 != 0)
                rotation -= 45f;
        }

        transform.localRotation = Quaternion.Euler(0f, rotation, 0f);

        if (Physics.Raycast(transform.position, Vector3.down))
        {
            //Create fences
            if (Input.GetKeyDown(KeyCode.U))
            {
                GameObject fence = Instantiate(Fence[(int)(rotation / 90)]);
                fence.transform.position = new Vector3(fence.transform.position.x + (int)transform.position.x, .95f, fence.transform.position.z + (int)transform.position.z+1);
            }
            //create gates
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("I");
            }
            //ramone le sol
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("O");
            }
            //plante un truc
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("P");
            }
        }

        // Used to move the gameObject by using direction (not X and Z axis)
        if(z != 0 || x != 0)
        {
            m_animator.SetBool(m_walkingHash, true);
            m_controller.Move(transform.forward * m_speed * Time.deltaTime);
        }
        else
            m_animator.SetBool(m_walkingHash, false);

    }
}
