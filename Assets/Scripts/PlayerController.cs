using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Hashes useful for animations
    [Header("Animations")]
    private Animator m_animator;
    private int m_walkingHash;
    private int m_walkingXHash;
    private int m_walkingZHash;

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
        m_walkingXHash = Animator.StringToHash("IsWalking");
        m_walkingXHash = Animator.StringToHash("WalkingX");
        m_walkingZHash = Animator.StringToHash("WalkingZ");
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
        float IsWalkingZ = m_animator.GetFloat("WalkingX");
        float IsWalkingX = m_animator.GetFloat("WalkingZ");

        if (x != 0 && z != 0)
        {
            x /= 1.5f;
            z /= 1.5f;
        }

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
                GameObject gate = Instantiate(Fence[(int)(rotation / 90)]);
                gate.transform.position = new Vector3(gate.transform.position.x + (int)transform.position.x, .95f, gate.transform.position.z + (int)transform.position.z + 1);
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
        m_animator.SetFloat(m_walkingXHash, x);
        m_animator.SetFloat(m_walkingZHash, z);

        if (z > 0 || Mathf.Abs(x) > 0)
            m_animator.SetBool("IsWalking", true);
        else
            m_animator.SetBool("IsWalking", false);

        Vector3 move = transform.right * x + transform.forward * z;
        m_controller.Move(move * m_speed * Time.deltaTime);

    }
}
