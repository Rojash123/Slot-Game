using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace JsonBackEnd
{
    public class SlotGameComputations : MonoBehaviour
    {
        private int[] reels1 = { 1, 2, 3, 4, 5, 2, 1, 1, 2, 6 };
        private int[] reels2 = { 1, 2, 1, 3, 5, 6, 3, 1, 1, 2, 2 };
        private int[] reels3 = { 2, 1, 3, 5, 7, 1, 2, 3, 5 };
        private int[] linList = { 0, 1, 2 };

        private Dictionary<string, List<int>> lines = new Dictionary<string, List<int>>();
        [SerializeField] int noOfRows, noOfCols;

        #region DataSettingForJson
        double UserAmount { get; set; }
        float betAmount { get; set; }
        float winAmount { get; set; }
        float totalWinAmount { get; set; }

        private List<List<int>> winLinesList = new List<List<int>>();
        private List<float> winAmountList = new List<float>();

        bool bonus;
        #endregion
        private void Awake()
        {
            AddLines();
        }
        void Start()
        {
            winAmount = 0;
            UserAmount = 1000;
            totalWinAmount = 0;
        }
        void AddLines()
        {
            lines.Add("0", new List<int> { 0, 0, 0 });
            lines.Add("1", new List<int> { 1, 1, 1 });
            lines.Add("2", new List<int> { 2, 2, 2 });
            lines.Add("3", new List<int> { 0, 1, 0 });
            lines.Add("4", new List<int> { 2, 1, 2 });
        }
        public void SlotGameCalculations(double totalAmount, float totalBetAmount)
        {
            UserAmount = totalAmount;
            betAmount = totalBetAmount;
            UserAmount -= totalBetAmount;
            bonus = false;

            winLinesList.Clear();
            winAmountList.Clear();

            //GenerateAllPossibleLines();
            GenerateMatrices(totalBetAmount);
        }
        void GenerateAllPossibleLines()
        {
            string wholeString = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        string localString = i + "" + j + "" + k;
                        wholeString += "," + localString;
                    }
                }
            }
            Debug.Log(wholeString);
        }
        void CalCulateProbabilityofEachCases(List<int> list, float individualBet)
        {
            int firstElem, secondElem, thirdElem;
            firstElem = matrix[list[0], 0];
            secondElem = matrix[list[1], 1];
            thirdElem = matrix[list[2], 2];
            if (firstElem == secondElem && secondElem == thirdElem)
            {
                float value = CheckSimilarCases(firstElem) * individualBet;
                if (value > 0)
                {
                    winLinesList.Add(list);
                    winAmountList.Add(value);
                }
                winAmount += value;
            }
            if (firstElem == secondElem && secondElem != thirdElem)
            {
                float temp = CheckFixedAnyCases(firstElem);
                if (CheckFixedAnyCases(firstElem) == 30)
                {
                    float[] nums = new float[] { 50, 23, 29, 24 };
                    if (temp == 30)
                    {
                        temp = nums[Random.Range(0, nums.Length)];
                    }
                }
                float value = temp * individualBet;
                if (value > 0)
                {
                    winLinesList.Add(list);
                    winAmountList.Add(value);
                }
                winAmount += value;
            }
            totalWinAmount += winAmount;
            winAmount = 0;
        }
        float CheckSimilarCases(int first)
        {
            switch (first)
            {
                case 1: return 3;
                case 2: return 4;
                case 3: return 15f;
                case 4: return 8;
                case 5: return 20;
                default: return 0;
            }
        }
        float CheckFixedAnyCases(int first)
        {

            switch (first)
            {
                case 1: return 2;
                case 2: return 2;
                case 3: return 3.5f;
                case 4: return 8;
                case 5: return 5;
                case 6: return 10;
                default: return 0;
            }
        }

        int[,] matrix = new int[3, 3];
        void GenerateMatrices(float totalbetAmount)
        {
            for (int row = 0; row < noOfRows; row++)
            {
                for (int col = 0; col < noOfCols; col++)
                {
                    if (col == 0) matrix[row, col] = reels1[Random.Range(0, reels1.Length - 1)];
                    if (col == 1) matrix[row, col] = reels2[Random.Range(0, reels2.Length - 1)];
                    if (col == 2) matrix[row, col] = reels3[Random.Range(0, reels3.Length - 1)];
                }
            }
            for (int i = 0; i < 5; i++)
            {
                List<int> list = new List<int>();
                list = lines[i.ToString()];
                CalCulateProbabilityofEachCases(list, totalbetAmount / 5);
            }
            UserAmount += totalWinAmount;
            PlayerPrefs.SetFloat("Balance", (float)UserAmount);
            JsonClass json = new JsonClass(matrix, winLinesList, winAmountList, UserAmount, totalWinAmount);
            string jsonString = JsonConvert.SerializeObject(json);
            GameManager.Instance.jsonString = jsonString;
            totalWinAmount = 0;
            Debug.Log(jsonString);
        }
    }
}
