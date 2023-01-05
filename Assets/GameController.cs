using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public double baseRate = 1;
    public double startNumber = 1;
    public double baseCost = 1;
    public double currentRate = 1;
    public double currentCost = 1;
    public float timeElapsed = 0;
    public float baseTime = 2;
    public float currentTime;
    public double currentNumber = 1;
    public bool managed = false;
    public bool adding = false;
    public double managerPrice = 1000;
    
    public TextMeshProUGUI numberLabel;
    public TextMeshProUGUI rateLabel;
    public TextMeshProUGUI costLabel;
    public TextMeshProUGUI manualClickLabel;
    public TextMeshProUGUI buyManagerLabel;
    public TextMeshProUGUI countdownLabel;
    public GameObject unmanagedPanel;
    public Image progressBar;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (managed && !adding)
        {
            if (Time.deltaTime > 1 / currentRate)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= 0.1)
                {
                    //currentNumber += currentRate * 0.1;
                    StartCoroutine(addNumber(currentRate));
                    timeElapsed = 0;
                }
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= 1 / currentRate)
                {
                    StartCoroutine(addNumber(currentRate));
                    timeElapsed = 0;
                }
            }

        }
        updateUI();

       /* progressBar.fillAmount += 0.03f;
        if(progressBar.fillAmount >= 1.0f)
        {
            progressBar.fillAmount = 0f;
        }*/

    }

    private void updateUI()
    {
        numberLabel.text = $"{currentNumber:n0}";
        rateLabel.text = $"{currentRate:n0} number per second";
        costLabel.text = $"MAKE NUMBER GO UP FASTER<br>(COST: {currentCost:n0} NUMBER)";
        if(currentCost > currentNumber)
        {
            costLabel.alpha = 0.5f;
            
        }
        else
        {
            costLabel.alpha = 1.0f;
        }

        if(!managed && currentNumber < managerPrice)
        {
            buyManagerLabel.alpha = 0.5f;
        }
        else
        {
            buyManagerLabel.alpha = 1f;
        }

        if (adding)
        {
            countdownLabel.enabled = true;
        }
        else
        {
            countdownLabel.enabled = false;
        }
    }

    public void onClick()
    {
        if (currentCost <= currentNumber)
        {
            currentNumber -= currentCost;
            currentRate *= 2;
            currentCost *= 2.32;
        }
    }

    public void onManualClick()
    {
        if (!managed && !adding)
        {
            StartCoroutine(addNumber(currentRate));
        }
    }

    public void onManagerBuyClick()
    {
/*        Debug.Log(managerPrice);
        Debug.Log(currentNumber);*/
        if(managerPrice <= currentNumber)
        {
            currentNumber -= managerPrice;
            managed = true;
            unmanagedPanel.SetActive(false);
        }
    }

    IEnumerator addNumber(double amount)
    {
        progressBar.fillAmount = 0.0f;
        adding = true;
        float value = 0.0f;
        float timeleft = currentTime;

        while (value <= 1f)
        {
            // Award takes 2 seconds, so that means if we want to update the progress bar 20 times
            // 2/20 = 1/10 = 0.1f seconds wait between updates
            // and the progress bar is 100%, and we want it updated 20 times, so 100/20 = increment of 5%
            // once you get to larger numbers it's not very smooth
            yield return new WaitForSeconds(currentTime/(currentTime*20));
            value += 1/(currentTime*20);
            UpdateSlider(value);
            timeleft -=  currentTime / (currentTime * 20);
            countdownLabel.text = $"{Mathf.Clamp(timeleft,0,currentTime):n2} sec";
        }

        currentNumber += amount;
        // let user see filled bar for split second
        yield return new WaitForSeconds(0.1f);
        progressBar.fillAmount = 0f;
        adding = false;
    }

    void UpdateSlider(float value)
    {
        progressBar.fillAmount = value;
    }

}
