using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonBackEnd;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public List<ColumnManager> cols;

    private JsonClass backEndData;

    private SlotGameComputations computations;
    public string jsonString;

    private Dictionary<string,int> lineDictionary = new Dictionary<string, int> { { "000", 0 }, { "111", 1 }, { "222", 2 }, { "010", 3 }, { "212", 4 } };

    [SerializeField] GameObject[] lines;
    private void Start()
    {
        computations = GetComponent<SlotGameComputations>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void GameStart(float totalBet, double gameBalance)
    {
        ResetBeforeSpin();
        jsonString = string.Empty;
        computations.SlotGameCalculations(gameBalance, totalBet);
        StartCoroutine(Spin());
    }
    public IEnumerator Spin()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(jsonString));
        backEndData = JsonConvert.DeserializeObject<JsonClass>(jsonString);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cols[i].actualCombination.Add(backEndData.wholeMatrix[j, i]);
            }
        }
        SoundManager.Instance.whileSpinSound.Play();
        foreach (var item in cols)
        {
            item.Spin();
        }
        yield return new WaitForSeconds(0.3f);
        UIManager.Instance.stopBtn.interactable = true;
        UIManager.Instance.stopAutoBtn.interactable = true;
    }
    public void AllSlotStopped()
    {
        UIManager.Instance.stopBtn.interactable = false;
        UIManager.Instance.stopAutoBtn.interactable = false;
        StartCoroutine(ISlotStopped());
    }
    IEnumerator ISlotStopped()
    {
        yield return null;
        if (backEndData.winLines.Count > 0)
        {
            StartCoroutine(ShowCombination());
        }
        else
        {
            UIManager.Instance.SetBalance(float.Parse(backEndData.totalAmount.ToString("0.00")));
            UIManager.Instance.SpinFinished();
        }
    }
    IEnumerator ShowCombination()
    {
        int i = 0;
        List<int> lineList = new List<int>();
        foreach (var item in backEndData.winLines)
        {
            int line = lineDictionary[item[0] + "" + item[1] + "" + item[2]];
            lineList.Add(line);
            float winAmount = backEndData.winAmount[i];
            UIManager.Instance.SetLineWinText(winAmount, (line + 1).ToString(), false);
            lines[line].SetActive(true);
            yield return new WaitForSeconds(1.5f);
            i++;
        }
        foreach (var item in lineList)
        {
            lines[item].SetActive(true);
        }
        UIManager.Instance.SetLineWinText(backEndData.totalWin, lineList.Count.ToString(), true);
        UIManager.Instance.SetWinBalance(backEndData.totalWin);
        UIManager.Instance.SetBalance((float.Parse(backEndData.totalAmount.ToString("0.00"))));
        UIManager.Instance.SpinFinished();
    }
    void ResetBeforeSpin()
    {
        foreach (var item in lines)
        {
            item.SetActive(false);
        }
        foreach (var item in cols)
        {
            item.actualCombination.Clear();
        }
    }

    public void SpinStop()
    {
        foreach (var item in cols)
        {
            item.duration += 10;
        }
    }
}
