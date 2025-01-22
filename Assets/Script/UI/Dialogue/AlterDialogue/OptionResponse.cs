using UnityEngine;

[System.Serializable]
public class OptionResponse
{
    [SerializeField]
    private string responseText;

    [SerializeField]
    private AdvancedDialogueSO adDialSO;

    public string ResponseText => responseText;

    public AdvancedDialogueSO AdDialSO => adDialSO;
}
