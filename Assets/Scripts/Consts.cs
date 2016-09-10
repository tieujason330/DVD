using UnityEngine;
using System.Collections;

public static class Consts {

    public const int ANIMATION_BASE_LAYER = 0;
    public const int ANIMATION_STRAFE_LAYER = 1;
    public const int ANIMATION_ATTACK_LAYER = 2;

    public const string TAG_MAIN_CAMERA = "MainCamera";
    public const string TAG_PLAYER = "Player";
}

public enum Faction
{
    Player,
    Faction1,
    Faction2
}

public enum Role
{
    None,
    Soldier,
    Captain,
    Commander
}

public enum CharacterStatus
{
    Alive,
    Dead
}

public enum CommandType
{
    Attack,
    Move
}

public enum CharacterAction
{
    None,
    Attack,
    Follow,
    MoveTo
}
