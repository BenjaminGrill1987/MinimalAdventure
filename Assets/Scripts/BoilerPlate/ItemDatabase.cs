using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] private List<ItemData> _items = new List<ItemData>();


    private void Start()
    {
        var index = 0;

        foreach(ItemData item in Resources.LoadAll("Items", typeof(ItemData)))
        {
            item.SetID(index);
            _items.Add(item);
            ++index;
        }
    }
    public static ItemData GetItem(int id) => Instance._items.Find(ItemData => ItemData.ID == id);

    public static ItemData GetItem(string name) => Instance._items.Find(ItemData => ItemData.Name == name);
}