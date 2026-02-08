using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct MapPart
{
    [SerializeField] private List<int> actsActive;
    [SerializeField] private GameObject _object;

    public List<int> ActsActive => actsActive;
    public GameObject _Object => _object;
}
