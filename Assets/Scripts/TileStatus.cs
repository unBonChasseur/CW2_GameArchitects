using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour
{
    // Up Right Down Left
    public bool[] m_fenceExists; 
    public GameObject[] m_Fence;
    public GameObject m_InstantiateUp;
    public GameObject m_InstantiateRight;
    public GameObject m_InstantiateDown;
    public GameObject m_InstantiateLeft;

    public GameObject[] m_Plant;
    public GameObject m_instantiatePlant;
    public bool m_hasPlant = false;

    public Material GrassMaterial;
    public Material DirtMaterial;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractFence(int nbFence, bool isGate)
    {
        if (!m_fenceExists[nbFence])
        {
            switch (nbFence)
            {
                case 0:
                    m_InstantiateUp = Instantiate(m_Fence[nbFence]);
                    m_InstantiateUp.transform.position = new Vector3(m_InstantiateUp.transform.position.x + (int)transform.position.x, .95f, m_InstantiateUp.transform.position.z + (int)transform.position.z);
                    break;

                case 1:
                    m_InstantiateRight = Instantiate(m_Fence[nbFence]);
                    m_InstantiateRight.transform.position = new Vector3(m_InstantiateRight.transform.position.x + (int)transform.position.x, .95f, m_InstantiateRight.transform.position.z + (int)transform.position.z);
                    break;

                case 2:
                    m_InstantiateDown = Instantiate(m_Fence[nbFence]);
                    m_InstantiateDown.transform.position = new Vector3(m_InstantiateDown.transform.position.x + (int)transform.position.x, .95f, m_InstantiateDown.transform.position.z + (int)transform.position.z);
                    break;

                case 3:
                    m_InstantiateLeft = Instantiate(m_Fence[nbFence]);
                    m_InstantiateLeft.transform.position = new Vector3(m_InstantiateLeft.transform.position.x + (int)transform.position.x, .95f, m_InstantiateLeft.transform.position.z + (int)transform.position.z);
                    break;
            }

            m_fenceExists[nbFence] = !m_fenceExists[nbFence];
        }

    }

    public void InteractGround(bool planter, int nbPlant)
    {
        if (!m_hasPlant && planter)
        {
            Debug.Log("planter");
            GetComponent<Renderer>().material = DirtMaterial;
            m_instantiatePlant = Instantiate(m_Plant[nbPlant]);
            ;
            // Redemander à landelin pour les carrés
            m_instantiatePlant.transform.position = new Vector3((int)transform.position.x + .3f, .95f, (int)transform.position.z + .3f - 1f);

            m_hasPlant = true;
        }
        else if(m_hasPlant && !planter)
        {
            Debug.Log("Récolter");
            GetComponent<Renderer>().material = GrassMaterial;
            m_hasPlant = false;
        }
    }
}
