using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;


[Serializable]
public class ToSaveObject
{
    private List<Parameter> values = new List<Parameter>();

    public ToSaveObject()
    {
        values.Clear();
    }

    public ToSaveObject(List<Parameter> v)
    {
        values = new List<Parameter>(v);
    }

    public void addObject(string father, string key, object value)
    {
        values.Add(new Parameter(father, key, value));
    }

    public void addObject(Parameter p )
    {
        values.Add(p);
    }

    public object getObject(string key)
    {
        return values.Find((e) =>
        {
            return e.key == key;
        }).value;
    }

    public int getSize()
    {
        return values.Count;
    }

    public void setObject(string father, string key, object value)
    {
        if (!values.Exists((e) =>
        {
            return (e.key == key) && (e.father == father);
        }))
        {
            values.Add(new Parameter(father, key, value));
        }
        values.Find((e) =>
        {
            return (e.key == key) && (e.father == father);
        }).value = value;
    }

    public void setObject(Parameter p)
    {
        if (!values.Exists((e) =>
        {
            return (e.key == p.key) && (e.father == p.father);
        }))
        {
            values.Add(p);
        }
        values.Find((e) =>
        {
            return (e.key == p.key) && (e.father == p.father);
        }).value = p.value;
    }

    public void saveThis()
    {
        Transform.FindObjectOfType<SavesManager>().addLevelObjects(values);
    }
}

[Serializable]
public class Parameter
{
    public string father;
    public string key;
    public object value;
    public Parameter()
    {
        this.father = "";
        this.key = "";
        this.value = null;
    }

    public Parameter(string father,string key, object value)
    {
        this.father = father;
        this.key = key;
        this.value = value;
    }
}