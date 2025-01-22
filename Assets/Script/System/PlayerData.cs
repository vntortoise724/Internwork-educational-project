using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public float[] pos;

    public PlayerData(PlayerMovement player)
    {
        player.level = level;

        pos = new float[2];
        pos[0] = player.transform.position.x;
        pos[1] = player.transform.position.y;
    }
}
