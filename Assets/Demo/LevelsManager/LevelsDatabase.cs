using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.LevelsManager
{
    [CreateAssetMenu(fileName = "LevelsDB", menuName = "ScriptableObjects/LevelsDB", order = 1)]
    public class LevelsDatabase : ScriptableObject
    {
        public List<ScriptableLevel> levels;
    }
}

[Serializable]
public struct EntranceType
{
    public EntrancesTypes entrance;
    public EntrancesTypes exit;
    public enum EntrancesTypes
    {
        GoldenDoorTop,
        DoorTop,
        GoldenDoorBottom,
        DoorBottom,
        GoldenDoorRight,
        DoorRight,
        GoldenDoorLeft,
        DoorLeft,
        StairsUp,
        StairsDown,
        StoneDoorTop,
        StoneDoorRoofTop,
        StoneDoorBottom,
        StoneDoorRoofBottom,
    }
}