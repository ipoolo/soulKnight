using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : System.Object
{
    private static EntityManager _manager;
    private static Dictionary<string, Entity> registerDictionary;

    public static  EntityManager  ManagerInstance()
    {
        if (_manager == null)
        {
            _manager = new EntityManager();
            registerDictionary = new Dictionary<string, Entity>();
        }
        return _manager;
    }

    public string RegisterEntity(Entity _t)
    {
        string key = GetRandomKey();
        if (!registerDictionary.ContainsValue(_t)) { 
            registerDictionary.Add(key, _t);
        }
        return key;
    }

    public Entity EntityById(string _stringID)
    {
        return (Entity)registerDictionary[_stringID];
    }

    private string GetRandomKey()
    {
       return string.Format("{0}{1}", DateTime.Now.Ticks, UnityEngine.Random.Range(0, 10000));
    }

    public void removeByEntity(Entity _t)
    {
        string removeKey = null;
        foreach (KeyValuePair<string,Entity> kv in registerDictionary)
        {
            if(kv.Value == _t)
            {
                removeKey = kv.Key;
            }
        }
        if (!string.IsNullOrEmpty(removeKey))
        {
            registerDictionary.Remove(removeKey);
        }
         
    }

    public void removeByIdString(String _idString)
    {

        if (!string.IsNullOrEmpty(_idString))
        {
            registerDictionary.Remove(_idString);
        }

    }
}
