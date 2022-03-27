using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private playerStatus status;

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
    [SerializeField] private GameObject m_hammerAttack;
    [SerializeField] private GameObject m_plow;

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<playerStatus>();
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

            // Create fences / update
            if (Input.GetKeyDown(KeyCode.F))
                Build(false, hitObject);

            // Create gates / open
            if (Input.GetKeyDown(KeyCode.G))
                Build(true, hitObject);

            //Destroy fence/gate
            if (Input.GetKeyDown(KeyCode.X))
                DestroyFence(hitObject);

            // Spade the ground // Plant 
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                Plant(0, hitObject);

            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                Plant(1, hitObject);

            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
                Plant(2, hitObject);

            // Harvest plants // Water
            if (Input.GetKeyDown(KeyCode.E))
                Harvest(hitObject);

            if (Input.GetMouseButtonDown(0))
                StartCoroutine(launchAnimation(3));
        }
    }

    /// <summary>
    /// Build / update || openGate
    /// </summary>
    /// <param name="isGate"></param>
    /// <param name="hitObject"></param>
    private void Build(bool isGate, GameObject hitObject)
    {
        int angle = (int)(m_playerTransform.rotation.eulerAngles.y / 90);
        if (m_playerTransform.rotation.eulerAngles.y % 90 > 45)
            angle = (angle + 1) % 4;

        angle = isGate ? angle + 4 : angle;
        
        if (hitObject.GetComponent<TileStatus>().HasFence(angle))
        {
            if (status.getWood() > 1)
            {
                hitObject.GetComponent<TileStatus>().CreateFence(angle);
                status.updateWood(-1);
                StartCoroutine(launchAnimation(2));
            }
        }

        else
        {
            if (!isGate)
            {
                int nbPlanksUpdate = hitObject.GetComponent<TileStatus>().UpdateFence(angle, status.getWood());
                if (nbPlanksUpdate != 0)
                {
                    status.updateWood(-nbPlanksUpdate);
                    StartCoroutine(launchAnimation(2));
                }
            }
            else
                hitObject.GetComponent<TileStatus>().InteractGate(angle);
        }
    }

    /// <summary>
    /// Destroy fence/gate
    /// </summary>
    /// <param name="hitObject"></param>
    private void DestroyFence(GameObject hitObject)
    {
        int angle = (int)(m_playerTransform.rotation.eulerAngles.y / 90);
        if (m_playerTransform.rotation.eulerAngles.y % 90 > 45)
            angle = (angle + 1) % 4;

        hitObject.GetComponent<TileStatus>().DestroyFence(angle);

    }

    /// <summary>
    /// Spade / Plant
    /// </summary>
    /// <param name="i"></param>
    /// <param name="hitObject"></param>
    private void Plant(int i, GameObject hitObject)
    {
        if (hitObject.GetComponent<TileStatus>().Spade())
            StartCoroutine(launchAnimation(0));
        else if (hitObject.GetComponent<TileStatus>().PlantGround(i))
            StartCoroutine(launchAnimation(1));
    }

    /// <summary>
    /// Harvest / Water
    /// </summary>
    /// <param name="hitObject"></param>
    private void Harvest(GameObject hitObject)
    {
        float nutritiveValue = hitObject.GetComponent<TileStatus>().Harvest();
        if (nutritiveValue != 0)
        {
            StartCoroutine(launchAnimation(1));
            status.updateHunger(nutritiveValue);
        }
        else if(status.getWater() > 0)
        {
            if (hitObject.GetComponent<TileStatus>().WaterPlants())
            {
                status.decreaseWater(1);
                StartCoroutine(launchAnimation(1));
            }
        }
    }
    private IEnumerator launchAnimation(int AnimationNumber)
    {
        m_isWorking = true;
        animationChangeStatus(AnimationNumber);

        yield return new WaitForSeconds(2.5f);

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
                m_animator.SetBool("Attack", m_isWorking);
                m_hammerAttack.SetActive(m_isWorking);
                break;
        }
    }
}
