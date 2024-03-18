using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace JsonBackEnd
{
    [Serializable]
    public class JsonClass
    {
        public int[,] wholeMatrix { get; set; }
        public List<List<int>> winLines { get; set; }
        public List<float> winAmount { get; set; }
        public double totalAmount { get; set; }
        public float totalWin { get; set; }


        public JsonClass(int[,] wholeMatrix, List<List<int>> winLines, List<float> winAmount, double totalAmount, float totalWin)
        {
            this.wholeMatrix = wholeMatrix;
            this.winLines = winLines;
            this.winAmount = winAmount;
            this.totalAmount = Math.Round(totalAmount, 2);
            this.totalWin = totalWin;
        }
    }
}
