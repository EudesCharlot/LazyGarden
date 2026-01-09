using System;
using UnityEngine;

public class HideShop : MonoBehaviour
{
    public GameObject shopUI;

    private void Start()
    {
        shopUI.SetActive(!shopUI.activeSelf);
    }

    public void ToggleShop()
    {
        if (shopUI == null) return;
        
        shopUI.SetActive(!shopUI.activeSelf);
    }
}