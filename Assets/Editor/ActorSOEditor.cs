using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActorSO))]
public class ActorSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ActorSO actorSO = (ActorSO)target;

        actorSO.emotionList ??= new List<EmotionSpriteEntry>();

        GUILayout.Space(10);
        GUILayout.Label("Emotion", EditorStyles.boldLabel);

        for (int i = 0; i < actorSO.emotionList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            actorSO.emotionList[i].emotion = EditorGUILayout.TextField(actorSO.emotionList[i].emotion, GUILayout.Width(100));
            actorSO.emotionList[i].sprite = (Sprite)EditorGUILayout.ObjectField(actorSO.emotionList[i].sprite, typeof(Sprite), false);
            if (GUILayout.Button("Remove"))
                actorSO.emotionList.RemoveAt(i);
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Add Emotion"))
            actorSO.emotionList.Add(new EmotionSpriteEntry { emotion = "New Emotion", sprite = null});

        GUILayout.Space(10);
        GUILayout.Label("Default Emotion", EditorStyles.boldLabel);
        actorSO.defaultEmo = EditorGUILayout.TextField("Default Emotion", actorSO.defaultEmo);

        if (GUI.changed)
            EditorUtility.SetDirty(actorSO);
    }
}
