using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnItems : MonoBehaviour
{
    public int index;
    private Image itemImg;
    private void Start()
    {
        index = int.Parse(transform.name);
        itemImg = GetComponentInChildren<Image>();
    }

    public void MoveY(GameObject moveTo,LeanTweenType type,float Speed)
    {
        LeanTween.moveLocalY(this.gameObject,moveTo.transform.position.y,Speed).setEase(type);
        this.transform.SetParent(moveTo.transform);
    }

    public void SetImage(Sprite gameItems)
    {
        itemImg.sprite = gameItems;
    }
}
