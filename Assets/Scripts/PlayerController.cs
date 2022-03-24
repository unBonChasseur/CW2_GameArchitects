using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Hashes useful for animations
    [Header("Animations")]
    [SerializeField] private Animator m_animator;
    private int m_walkingHash;
    private int m_walkingXHash;
    private int m_walkingZHash;

    [Header("Movement")]
    [SerializeField] private float m_speed;
    [SerializeField] private CharacterController m_controller;
    [SerializeField] private Transform m_playerTransform;
    private bool m_isWorking = false;

    [Header("Ground")]
    private RaycastHit m_hit;

    [Header("Tools")]
    [SerializeField] private GameObject m_hammer;
    [SerializeField] private GameObject m_plow;

    // Start is called before the first frame update
    void Start()
    {
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


        //////////////////////
        ///   INTERACTIONS ///
        //////////////////////

        if (Physics.Raycast(transform.position, Vector3.down, out m_hit) && !m_isWorking)
        {
            GameObject hitObject = m_hit.transform.gameObject;

            // Create fences
            if (Input.GetKeyDown(KeyCode.F))
                build(false, hitObject);

            // Create gates
            if (Input.GetKeyDown(KeyCode.G))
                build(true, hitObject);

            // Spade the ground // Plant 
            if (Input.GetKeyDown(KeyCode.Keypad1))
                plant(0, hitObject);

            if (Input.GetKeyDown(KeyCode.Keypad2))
                plant(1, hitObject);

            if (Input.GetKeyDown(KeyCode.Keypad3))
                plant(2, hitObject);

            // Harvest plants
            if (Input.GetKeyDown(KeyCode.E))
                harvest(hitObject);
        }
    }

    /// <summary>
    /// Build / update
    /// </summary>
    /// <param name="isGate"></param>
    /// <param name="hitObject"></param>
    private void build(bool isGate, GameObject hitObject)
    {
        int angle = (int)(m_playerTransform.rotation.eulerAngles.y / 90);
        if (m_playerTransform.rotation.eulerAngles.y % 90 > 45)
            angle = (angle + 1) % 4;

        angle = isGate ? angle + 4 : angle;

        if(hitObject.GetComponent<TileStatus>().CreateFence(angle))
            StartCoroutine(InteractAnimation(2));

        else
        {
            int nbPlanksUpdate = hitObject.GetComponent<TileStatus>().UpdateFence(angle, GetComponent<playerStatus>().getWood());
            if(nbPlanksUpdate != 0)
            {
                GetComponent<playerStatus>().updateWood(-nbPlanksUpdate);
                StartCoroutine(InteractAnimation(2));
            }
        }
    }

    /// <summary>
    /// Spade / Plant
    /// </summary>
    /// <param name="i"></param>
    /// <param name="hitObject"></param>
    private void plant(int i, GameObject hitObject)
    {
        if (hitObject.GetComponent<TileStatus>().Spade())
            StartCoroutine(InteractAnimation(0));
        else if (hitObject.GetComponent<TileStatus>().PlantGround(i))
            StartCoroutine(InteractAnimation(1));
    }

    /// <summary>
    /// Harvest / Water
    /// </summary>
    /// <param name="hitObject"></param>
    private void harvest(GameObject hitObject)
    {
        float nutritiveValue = hitObject.GetComponent<TileStatus>().Harvest();
        if (nutritiveValue != 0)
        {
            StartCoroutine(InteractAnimation(1));
            GetComponent<playerStatus>().updateHunger(nutritiveValue);
        }
        else if(GetComponent<playerStatus>().getWater() > 0)
        {
            StartCoroutine(InteractAnimation(1));
            if(hitObject.GetComponent<TileStatus>().WaterPlants())
                GetComponent<playerStatus>().decreaseWater(1);
        }
    }

    private IEnumerator InteractAnimation(int AnimationNumber)
    {
        m_isWorking = true;
        animationChangeStatus(AnimationNumber);

        yield return new WaitForSeconds(3f);

        m_isWorking = false;
        animationChangeStatus(AnimationNumber);
    }

    private void animationChangeStatus(int x)
    {
        switch (x)
        {
            case 0:
                m_animator.SetBool("Labourer", m_isWorking);
                m_plow.SetActive(m_isWorking);
                break;

            case 1:
                m_animator.SetBool("Recolter", m_isWorking);
                break;

            case 2:
                m_animator.SetBool("Construire", m_isWorking);
                m_hammer.SetActive(m_isWorking);
                break;

            case 3:

                break;
        }
    }
}
