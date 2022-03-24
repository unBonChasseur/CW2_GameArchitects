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
    public bool m_isWorking = false;

    [Header("Ground")]
    private RaycastHit hit;


    public GameObject m_marteau;
    public GameObject m_outil;

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

        if (Physics.Raycast(transform.position, Vector3.down, out hit) && !m_isWorking)
        {

            GameObject hitObject = hit.transform.gameObject;

            //Create fences
            if (Input.GetKeyDown(KeyCode.F))
            {
                int angle = 0;

                angle += (int)(m_playerTransform.rotation.eulerAngles.y / 90);

                if (m_playerTransform.rotation.eulerAngles.y % 90 > 45 && angle < 3)
                    angle += 1;

                StartCoroutine(InteractAnimation(2));

                hitObject.GetComponent<TileStatus>().InteractFence(angle, false);
                
            }

            //ramone le sol
            if (Input.GetKeyDown(KeyCode.P))
            {
                hitObject.GetComponent<TileStatus>().InteractGround(true, 0);
                StartCoroutine(InteractAnimation(0));
            }

        }

        // Used to move the gameObject by using direction (not X and Z axis)
        m_animator.SetFloat(m_walkingXHash, x);
        m_animator.SetFloat(m_walkingZHash, z);

        

        if (!m_isWorking)
        {
            if (z > 0 || Mathf.Abs(x) > 0)
                m_animator.SetBool("IsWalking", true);
            else
                m_animator.SetBool("IsWalking", false);

            Vector3 move = transform.right * x + transform.forward * z;
            m_controller.Move(move * m_speed * Time.deltaTime);
        }
        else
            m_animator.SetBool("IsWalking", false);

    }

    IEnumerator InteractAnimation(int x)
    {
        m_isWorking = true;
        switch (x)
        {
            case 0:
                m_animator.SetBool("Planter", true);
                m_outil.SetActive(true);
                break;
            case 1:
                m_animator.SetBool("Recolter", true);
                break;
            case 2:
                m_animator.SetBool("Construire", true);
                m_marteau.SetActive(true);
                break;

        }

        yield return new WaitForSeconds(3f);

        m_isWorking = false;
        switch (x)
        {
            case 0:
                m_animator.SetBool("Planter", false);
                m_outil.SetActive(false);
                break;

            case 1:
                m_animator.SetBool("Recolter", false);
                break;

            case 2:
                m_animator.SetBool("Construire", false);
                m_marteau.SetActive(false);
                break;

        }
    }
}
