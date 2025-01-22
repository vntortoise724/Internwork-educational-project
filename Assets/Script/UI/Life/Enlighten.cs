using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enlighten : MonoBehaviour
{
    private Image enlightImage;
    private Button btn;
    private void Awake()
    {
        enlightImage = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {

    }
}
