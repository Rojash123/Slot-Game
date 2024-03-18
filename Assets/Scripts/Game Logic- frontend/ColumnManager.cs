using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class ColumnManager : MonoBehaviour
{
    public GameObject[] posHolder;
    public ColumnItems[] colItems;

    [SerializeField] float speed, totalSpinTime;
    public float duration;

    public List<int> actualCombination;
    public LeanTweenType tweenType;

    private int i = 7, j;
    public bool spin, is1stCol;

    public GameItems items;

    private void Start()
    {
        SetRandomImages();
    }
    void SetRandomImages()
    {
        foreach (var item in colItems)
        {
            int value = Random.Range(0, items.gameItems.Length - 1);
            item.SetImage(items.gameItems[value]);
        }
    }
    public void Spin()
    {
        StartCoroutine(ISpin());
    }
    private IEnumerator ISpin()
    {
        spin = true;
        j = 2;
        duration = 0;

        while (spin == true)
        {
            yield return new WaitForSeconds(speed);
            if (duration < totalSpinTime)
            {
                duration += Time.deltaTime;
                _Spin();
            }
            else if (duration > totalSpinTime)
            {
                spin = false;
                EndOfSpin();
            }
        }
    }
    private void _Spin()
    {
        foreach (var item in colItems)
        {
            int currentIndex = item.index;
            if (currentIndex == 1)
            {
                if (spin)
                {
                    int value = Random.Range(0, items.gameItems.Length - 1);
                    item.SetImage(items.gameItems[value]);
                }
                else
                {
                    int value = actualCombination[j] - 1;
                    item.SetImage(items.gameItems[value]);
                    j--;
                }
            }
            currentIndex = (currentIndex + 1) % i;
            item.index = currentIndex;

            if (currentIndex == 0)
            {
                item.transform.position = posHolder[currentIndex].transform.position;
                item.transform.SetParent(posHolder[currentIndex].transform);
            }
            if (currentIndex == 1)
            {
                item.transform.position = posHolder[currentIndex].transform.position;
                item.transform.SetParent(posHolder[currentIndex].transform);
            }
            else
            {
                item.MoveY(posHolder[currentIndex], tweenType, speed);
            }
        }
    }
    public void EndOfSpin()
    {
        for (int i = 0; i < 3; i++)
        {
            _Spin();
        }
        if (this.transform.name == "Col3")
        {
            SoundManager.Instance.whileSpinSound.Stop();
            GameManager.Instance.AllSlotStopped();
        }
    }

}
