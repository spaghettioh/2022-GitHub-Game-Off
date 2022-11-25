﻿using System.Collections.Generic;
using UnityEngine;

public class ComponentPoolBase<T> : ScriptableObject where T : Component
{
    [SerializeField] private T _prefab;

    private Stack<T> _stack = new Stack<T>();
    private Transform _parent;

    /// <summary>
    /// Spins up an object pool of disabled objects
    /// </summary>
    /// <param name="count">The number of emitters to create</param>
    /// <param name="parent">The parent object for the emitters</param>
    public void PreWarm(int count, Transform parent = null)
    {
        _parent = parent;
        // Create a pool of disabled audio emitters
        for (var i = 0; i < count; i++)
        {
            Create();
        }
    }

    /// <summary>
    /// Pulls an object from the pool, creates one if none left
    /// </summary>
    /// <returns></returns>
    public T Request()
    {
        if (_stack.Count == 0) Create();
        return _stack.Pop();
    }

    /// <summary>
    /// Puts an object back into the pool
    /// </summary>
    /// <param name="returning"></param>
    public void Return(T returning) => _stack.Push(returning);

    /// <summary>
    /// Clears the stack whenever Play stops (because the pool is an asset)
    /// </summary>
    private void OnDisable() => _stack.Clear();

    /// <summary>
    /// Instantiates an object and adds it to the pool
    /// </summary>
    /// <returns>An object</returns>
    private T Create()
    {
        T t = Instantiate(_prefab, _parent);
        t.gameObject.SetActive(false);
        _stack.Push(t);

        return t;
    }
}