using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Actor", menuName = "ScriptableObjects/Actor")]
public class ActorSO : ScriptableObject
{
    public string actorName;
    public Sprite actorPoitrait;
    public string defaultEmo = "Default";
    public List<EmotionSpriteEntry> emotionList;

    private Dictionary<string, Sprite> emotionalPoitrait;

    public void InitializeDictionary()
    {
        emotionalPoitrait = new Dictionary<string, Sprite>();
        foreach (var entry in emotionList) 
        { 
            if (!emotionalPoitrait.ContainsKey(entry.emotion))
                emotionalPoitrait.Add(entry.emotion, entry.sprite);
        }
    }

    public Sprite GetEmotionSprite(string emoName)
    {
        if (emotionalPoitrait == null)
            InitializeDictionary();

        if (string.IsNullOrEmpty(emoName) || !emotionalPoitrait.ContainsKey(emoName))
            emoName = defaultEmo;

        return emotionalPoitrait.ContainsKey(emoName) ? emotionalPoitrait[emoName] : actorPoitrait; 
    }
}
