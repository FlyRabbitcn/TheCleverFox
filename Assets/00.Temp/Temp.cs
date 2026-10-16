using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using FlyRabbit.SaveSystem;
using FlyRabbit.AssetManagement;
using FlyRabbit.UIFramework;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Temp : MonoBehaviour
{
    private TMP_Text m_Text;

    private void Awake()
    {
        m_Text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        m_Text.text = $"{Screen.currentResolution.width} x {Screen.currentResolution.height}";
    }
}
