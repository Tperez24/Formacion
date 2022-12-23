using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.LevelsManager
{
    public class WaveFunctionRules : ScriptableObject
    {
        public List<ScriptableLevel> levels;
    }
}

[Serializable]
public class AccessMethod
{
    public string accessName;
    public int accesIndex;
}