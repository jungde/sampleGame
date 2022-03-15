using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MapTile
    {
        Floor,
        Wall,
        Obstacle,
        Breakable,
    }

    public enum State
    {
        Idle,
        Moving,
        Die,        
    }

    public enum CharacterType
    {
        Player,
        Monster
    }
}
