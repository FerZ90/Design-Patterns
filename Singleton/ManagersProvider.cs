using System.Collections.Generic;
using System;

public delegate void ManagerAdded(object manager);

public sealed class ManagersProvider
{
    #region __SINGLETON_IMPL__
    // Singleton implementation first.
    private static readonly Lazy<ManagersProvider> Lazy = new(() => new ManagersProvider());

    public static ManagersProvider Instance => Lazy.Value;

    private ManagersProvider()
    {
        _providers = new Dictionary<Type, object>();
    }
    #endregion __SINGLETON_IMPL__

    #region __SERVICE_PROVIDER_IMPL__

    public event ManagerAdded OnManagerAdded;

    private Dictionary<Type, object> _providers;

    public bool Contains<T>() where T : class
    {
        return _providers.ContainsKey(typeof(T));
    }

    public OperationResult Get<T>() where T : class
    {
        if (Contains<T>()) return OperationResult.CreateResultOperation(Result.Success, (T)_providers[typeof(T)]);

        UnityEngine.Debug.LogError($"Trying to get the manager {typeof(T)}, but it does not exist (yet)!.");
        return OperationResult.CreateResultOperation(Result.Error, default);

    }

    public OperationResult Register<T>(Func<T> creatorFunc) where T : class
    {
        if (Contains<T>())
        {
            return OperationResult.CreateWarningOperation($"Instance of {typeof(T)} already registered.");
        }

        T instance;
        try
        {
            // method "creatorFunc" can thrown an exception.
            instance = creatorFunc();
        }
        catch (Exception e)
        {
            return OperationResult.CreateErrorOperation($"Exception calling {creatorFunc}: {e.Message}");
        }

        _providers.Add(typeof(T), instance);
        OnManagerAdded?.Invoke(instance);
        return OperationResult.CreateSuccessOperation();
    }

    public OperationResult Register<T>() where T : class, new()
    {
        if (Contains<T>())
        {
            return OperationResult.CreateWarningOperation($"Instance of {typeof(T)} already registered.");
        }

        T instance;
        try
        {
            // ctor of T type can thrown an exception.
            instance = new T();
        }
        catch (Exception e)
        {
            return OperationResult.CreateErrorOperation($"Exception creating new instance of {typeof(T)}: {e.Message}");
        }

        _providers.Add(typeof(T), instance);
        OnManagerAdded?.Invoke(instance);
        return OperationResult.CreateSuccessOperation();
    }
    #endregion __SERVICE_PROVIDER_IMPL__

    #region __STATIC_HELPERS__

    public static T GET<T>() where T : class
    {
        return (T)Instance.Get<T>().Data;
    }


    #endregion __STATIC_HELPERS__
}

