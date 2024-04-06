using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item",order = 0)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _name, _description;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Itemart _itemart;
    [SerializeField] private int _cost, _damage, _armor;

    private int _id;

    public void SetID(int id) => _id = id;

    public int ID => _id;

    public string Name => _name;

    public string Description => _description;

    public Sprite Sprite => _sprite;

    public Itemart Itemart => _itemart;

    public int Cost => _cost;

    public int Damage => _damage;

    public int Armor => _armor;
}

public enum Itemart
{
    Weapon,
    Armor,
    Potion
}