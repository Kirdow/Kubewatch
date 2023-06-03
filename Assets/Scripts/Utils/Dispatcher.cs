using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    public static void RunAsync(Action action)
    {
        ThreadPool.QueueUserWorkItem(p =>
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogError("Error in Dispatcher.RunAsync Callback");
                Debug.LogException(e);
            }
        });
    }

    public static void RunAsync<T>(Action<T> action, T state)
    {
        ThreadPool.QueueUserWorkItem(p =>
        {
            try
            {
                action((T)p);
            }
            catch (Exception e)
            {
                Debug.LogError("Error in Dispatcher.RunAsync<T> Callback");
                Debug.LogException(e);
            }
        }, (object)state);
    }

    public static void RunOnMainThread(Action action)
    {
        lock (_backlog) {
            _backlog.Add(action);
            _queued = true;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
            DontDestroyOnLoad(_instance.gameObject);
        }
    }

    private void Update()
    {
        if (_queued)
        {
            lock (_backlog)
            {
                var tmp = _actions;
                _actions = _backlog;
                _backlog = tmp;
                _queued = false;
            }

            foreach (var action in _actions)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogError("Error in Dispatcher.RunOnMainThread Callback");
                    Debug.LogException(e);
                }
            }

            _actions.Clear();
        }
    }

    static Dispatcher _instance;
    static volatile bool _queued = false;
    static List<Action> _backlog = new List<Action>(8);
    static List<Action> _actions = new List<Action>(8);
}