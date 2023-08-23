using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public bool useable;
}

public enum ItemType
{
    Empty,
    OldLantern,
    BoxLantern,
    Lighter,
    Flashlight,
    Torch,
};
