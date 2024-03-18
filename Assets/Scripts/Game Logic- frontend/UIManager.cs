using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [SerializeField] TextMeshProUGUI currentBalance, winAmount, lineWinText;
    float gameBalance;


    [Space(10)]

    [SerializeField] GameObject helpMenu, helpImage1, helpImage2;
    [SerializeField] Button previousButton, nextButton, closeButton, closeAnyWhereBUtton, helpMenuOpen;

    [Space(10)]

    [SerializeField] TextMeshProUGUI lineBetTxt, totalBetTxt;
    float lineBet, totalBet;
    int noOfLines = 5;
    [SerializeField] Button plusBtn, minusBtn;

    [Space(10)]

    public Button spinButton,stopBtn,autoBtn,stopAutoBtn;
    private bool IsAutoSpin;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitalizeBalance();
        SetFunctionsToButton();
        BetInitialization();
    }
    void SetFunctionsToButton()
    {
        previousButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);
        closeButton.onClick.AddListener(CloseHelpMenu);
        closeAnyWhereBUtton.onClick.AddListener(CloseHelpMenu);
        helpMenuOpen.onClick.AddListener(OpenHelpMenu);

        plusBtn.onClick.AddListener(PlusBet);
        minusBtn.onClick.AddListener(MinusBet);

        spinButton.onClick.AddListener(Spin);
        stopBtn.onClick.AddListener(StopSpin);
        autoBtn.onClick.AddListener(AutoSpin);
        stopAutoBtn.onClick.AddListener(StopAutoSpin);
    }

    #region BalanceAndWin Display
    private void InitalizeBalance()
    {
        if (!PlayerPrefs.HasKey("Balance"))
        {
            gameBalance = 1000f;
            PlayerPrefs.SetFloat("Balance", (float)gameBalance);
            SetBalance(gameBalance);
        }
        else
        {
            gameBalance = PlayerPrefs.GetFloat("Balance");
            SetBalance(gameBalance);
        }
        SetWinBalance(0);
        lineWinText.text = "";
    }
    public void SetLineWinText(float amount, string lineno,bool canEdit)
    {
        SoundManager.Instance.WinSound();
        if (canEdit)
        {
            lineWinText.text = "You won a total of $" + amount.ToString() + " on " + lineno + " lines";
        }
        else
        {
            lineWinText.text = "You have won $" + amount.ToString("f2") + " on line " + lineno;
        }
    }
    public void SetBalance(float totalBalance)
    {
        gameBalance = totalBalance;
        currentBalance.text ="$"+totalBalance.ToString("f2");
    }
    public void SetWinBalance(float win)
    {
        winAmount.text = "$"+win.ToString("f2"); ;
    }

    #endregion

    #region HelpMenu
    private void OpenHelpMenu()
    {
        SoundManager.Instance.ButtonClickSound();
        helpMenu.SetActive(true);
        PreviousPage();
    }
    private void CloseHelpMenu()
    {
        SoundManager.Instance.ButtonClickSound();
        helpMenu.SetActive(false);
    }
    private void PreviousPage()
    {
        SoundManager.Instance.ButtonClickSound();
        helpImage1.SetActive(true);
        helpImage2.SetActive(false);
    }
    private void NextPage()
    {
        SoundManager.Instance.ButtonClickSound();
        helpImage1.SetActive(false);
        helpImage2.SetActive(true);
    }
    #endregion

    #region BetPanel
    public void BetInitialization()
    {
        lineBet = 0.1f;
        totalBet = lineBet * noOfLines;
        SetBetText();
    }
    private void SetBetText()
    {
        lineBetTxt.text = "$" + lineBet.ToString("f2");
        totalBetTxt.text = "$" + totalBet.ToString("f2");
    }
    public void PlusBet()
    {
        SoundManager.Instance.ButtonClickSound();
        lineBet = lineBet switch
        {
            0.1f => 0.2f,
            0.2f => 0.3f,
            0.3f => 0.4f,
            0.4f => 0.5f,
            0.5f => 1f,
            1f => 5f,
            _ => 0.1f
        };
        totalBet = noOfLines * lineBet;
        SetBetText();
    }
    public void MinusBet()
    {
        SoundManager.Instance.ButtonClickSound();
        lineBet = lineBet switch
        {
            5f => 1f,
            1f => 0.5f,
            0.5f => 0.4f,
            0.4f => 0.3f,
            0.3f => 0.2f,
            0.2f => 0.1f,
            _ => 5f
        };
        totalBet = noOfLines * lineBet;
        SetBetText();
    }
    #endregion

    #region GameFunctionality
    public void Spin()
    {
        SoundManager.Instance.SpinSound();
        DeactivateButtonBeforeSpin();
        SetWinBalance(0);
        lineWinText.text = "";
        currentBalance.text="$"+(gameBalance-totalBet).ToString("f2");
        GameManager.Instance.GameStart(totalBet, gameBalance);
    }
    public void StopSpin()
    {
        SoundManager.Instance.StopSound();
        stopBtn.interactable = false;
        stopAutoBtn.interactable = false;
        GameManager.Instance.SpinStop();
    }
    public void AutoSpin()
    {
        IsAutoSpin = true;
        Spin();
        stopAutoBtn.interactable = true;
        stopAutoBtn.gameObject.SetActive(true);

    }
    public void StopAutoSpin()
    {
        SoundManager.Instance.ButtonClickSound();
        IsAutoSpin = false;
        autoBtn.interactable = false;
        stopAutoBtn.interactable = false;
        stopAutoBtn.gameObject.SetActive(false);
    }
    void DeactivateButtonBeforeSpin()
    {
        plusBtn.interactable = false;
        minusBtn.interactable = false;
        spinButton.interactable = false;
        stopBtn.interactable = false;
        autoBtn.interactable = false;
        stopBtn.gameObject.SetActive(true);
    }
    public void SpinFinished()
    {
        ActivateAfterSpin();
    }
    void ActivateAfterSpin()
    {
        if (IsAutoSpin) 
        {
            Invoke(nameof(Spin),1.5f);
            return;
        }
        plusBtn.interactable = true;
        minusBtn.interactable = true;
        spinButton.interactable = true;
        autoBtn.interactable = true;
        stopBtn.gameObject.SetActive(false);
    }
    #endregion
}
