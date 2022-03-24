using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour
{
    // Up Right Down Left
    [Header("Fences")]
    [SerializeField] private GameObject[] m_fence;
    [SerializeField] private bool[] m_fenceExists;
    private GameObject m_instantiateUp;
    private GameObject m_instantiateRight;
    private GameObject m_instantiateDown;
    private GameObject m_instantiateLeft;

    [Header("Plant")]
    [SerializeField] private GameObject[] m_plant;
    private bool m_hasPlant = false;
    private GameObject m_instantiatePlant;

    [Header("Ground")]
    private bool m_isSpaded = false;
    private float m_waterTimeMax = 30;
    private float m_waterTime = 0;
    [SerializeField] private float m_ToGrassDuration = 3;
    [SerializeField] private Material m_grassMaterial;
    [SerializeField] private Material m_dirtMaterial;
    [SerializeField] private Material m_wetDirtMaterial;


    public bool CreateFence(int nbFence)
    {
        if (!m_fenceExists[nbFence%4])
        {
            switch (nbFence%4)
            {
                case 0:
                    m_instantiateUp = Instantiate(m_fence[nbFence]);
                    m_instantiateUp.transform.position = new Vector3(m_instantiateUp.transform.position.x + (int)transform.position.x, .95f, m_instantiateUp.transform.position.z + (int)transform.position.z);
                    break;

                case 1:
                    m_instantiateRight = Instantiate(m_fence[nbFence]);
                    m_instantiateRight.transform.position = new Vector3(m_instantiateRight.transform.position.x + (int)transform.position.x, .95f, m_instantiateRight.transform.position.z + (int)transform.position.z);
                    break;

                case 2:
                    m_instantiateDown = Instantiate(m_fence[nbFence]);
                    m_instantiateDown.transform.position = new Vector3(m_instantiateDown.transform.position.x + (int)transform.position.x, .95f, m_instantiateDown.transform.position.z + (int)transform.position.z);
                    break;

                case 3:
                    m_instantiateLeft = Instantiate(m_fence[nbFence]);
                    m_instantiateLeft.transform.position = new Vector3(m_instantiateLeft.transform.position.x + (int)transform.position.x, .95f, m_instantiateLeft.transform.position.z + (int)transform.position.z);
                    break;
            }

            m_fenceExists[nbFence%4] = true;
            return true;
        }
        return false;

    }

    public int UpdateFence(int nbFence, int nbWood)
    {
        int nbPlanks = 0;
        switch (nbFence)
        {
            case 0:
                nbPlanks = m_instantiateUp.GetComponentInChildren<fenceStatus>().getNbPlanksToUpgrade();
                break;

            case 1:
                nbPlanks = m_instantiateRight.GetComponentInChildren<fenceStatus>().getNbPlanksToUpgrade();
                break;

            case 2:
                nbPlanks = m_instantiateDown.GetComponentInChildren<fenceStatus>().getNbPlanksToUpgrade();
                break;

            case 3:
                nbPlanks = m_instantiateLeft.GetComponentInChildren<fenceStatus>().getNbPlanksToUpgrade();
                break;
        }

        if (nbWood >= nbPlanks && nbPlanks != -1) 
        { 
            switch (nbFence)
            {
                case 0:
                    m_instantiateUp.GetComponentInChildren<fenceStatus>().updateCurrentLevel();
                    break;

                case 1:
                    m_instantiateUp.GetComponentInChildren<fenceStatus>().updateCurrentLevel();
                    break;

                case 2:
                    m_instantiateUp.GetComponentInChildren<fenceStatus>().updateCurrentLevel();
                    break;

                case 3:
                    m_instantiateUp.GetComponentInChildren<fenceStatus>().updateCurrentLevel();
                    break;
            }
            return nbPlanks; 
        }
        else
            return 0;
    }

    public bool Spade()
    {
        if (!m_isSpaded)
        {
            m_isSpaded = true;
            GetComponent<Renderer>().material = m_dirtMaterial;
            StartCoroutine(DirtToGrass());
            return true;
        }
        return false;
    }

    public bool PlantGround(int nbPlant)
    {
        if (!m_hasPlant)
        {
            m_instantiatePlant = Instantiate(m_plant[nbPlant]);
            m_instantiatePlant.transform.position = new Vector3((int)transform.position.x + .5f, .95f, (int)transform.position.z + .5f - 1f);
            m_hasPlant = true;
            if (m_waterTime != 0)
                m_instantiatePlant.GetComponentInChildren<plantStatus>().WaterPlant(m_waterTime);
            return true;
        }
        return false;
    }

    public float Harvest()
    {
        if (m_hasPlant)
        {
            if (m_instantiatePlant.GetComponentInChildren<plantStatus>().getCurrentTime() <= 0)
            {
                m_hasPlant = false;
                float nutritiveValue = m_instantiatePlant.GetComponentInChildren<plantStatus>().getNutritiveValue();
                Destroy(m_instantiatePlant);
                StartCoroutine(DirtToGrass());
                return nutritiveValue;
            }
        }
        return 0;
    }

    public bool WaterPlants()
    {
        if (m_isSpaded)
        {
            if (m_hasPlant)
            {
                m_instantiatePlant.GetComponentInChildren<plantStatus>().WaterPlant(m_waterTimeMax);
            }
            GetComponent<Renderer>().material = m_wetDirtMaterial;

            m_waterTime = m_waterTimeMax;
            StartCoroutine(WetDirtToDirt());

            return true;
        }
        return false;
    }

    private IEnumerator WetDirtToDirt()
    {
        m_waterTime = m_waterTimeMax;
        WaitForSeconds wait = new WaitForSeconds(1);
        while(m_waterTime > 0)
        {
            yield return wait;
            m_waterTime--;
        }
        if(GetComponent<Renderer>().material != m_grassMaterial)
            GetComponent<Renderer>().material = m_dirtMaterial;
    }

    private IEnumerator DirtToGrass()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        float timeReset = m_ToGrassDuration * 60;
        while (m_isSpaded && !m_hasPlant)
        {
            yield return wait;
            timeReset--;
            if(timeReset == 0)
            {
                m_isSpaded = false;
                GetComponent<Renderer>().material = m_grassMaterial;
            }
        }
    }
}
