using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnlightenManager : MonoBehaviour
{
    [Header("Life Setup")]
    public GameObject enlighten;
    public Transform enliPanel;

    [Header("Number of Life")]
    public int enliNum = 3;
    private int currentEnlighten;

    [SerializeField]
    private MinigameCompletion minigame;

    private void Start()
    {
        currentEnlighten = enliNum;
        GenerateEnlight();
    }

    private void Update()
    {
        if (currentEnlighten != enliNum)
        {
            GenerateEnlight();
            currentEnlighten = enliNum;

            if (currentEnlighten == 0)
            {
                Debug.Log("Game Over");
            }
        }
    }

    private void GenerateEnlight()
    {
        foreach (Transform t in enliPanel)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < enliNum; i++)
        {
            Instantiate(enlighten, enliPanel);
        }
    }
}
