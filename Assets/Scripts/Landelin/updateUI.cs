using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class updateUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    private playerStatus status;
    [SerializeField] private GameObject time_obj;
    private LightingManager dayManager; 

    [SerializeField] private Text wood;
    [SerializeField] private Slider hunger;
    [SerializeField] private Slider water;
    [SerializeField] private Text time;

    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject pauseUI;

    private bool paused;
    private bool isDeathUI; 
    void Start()
    {
        status = player.GetComponent<playerStatus>();
        dayManager = time_obj.GetComponent<LightingManager>();
        paused = false;
        isDeathUI = false;
        inGameUI.SetActive(true);
        deathUI.SetActive(false);
        pauseUI.SetActive(false);
        //hunger / life
        hunger.maxValue = status.getMaxHunger();
        hunger.value = status.getHunger();

        //water
        water.maxValue = status.getMaxWater();
        water.value = status.getWater();
    }

    // Update is called once per frame
    void Update()
    {
        playerDeathUI();
        pauseMenu();
        wood.text = status.getWood().ToString();
        hunger.value = status.getHunger();
        water.value = status.getWater();
        time.text = Mathf.FloorToInt(dayManager.getTimeOfDay()).ToString();
    }

    private void playerDeathUI()
    {
        if (!status.getPlayerIsDead())
            return;   
        else if(isDeathUI == false)
        {
            inGameUI.SetActive(false);
            deathUI.SetActive(true);
            pauseUI.SetActive(false);

            isDeathUI = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    private void pauseMenu()
    {
        if(Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape))
        {
            if (!deathUI.activeInHierarchy)
                paused = !paused;
        }
        if (!deathUI.activeInHierarchy)
        {
            if (paused == false)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                pauseUI.SetActive(false);
            }
            else if (paused == true)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                pauseUI.SetActive(true);
            }
        }
        
    }

    public void resume()
    {
        paused = false;
    }
}
