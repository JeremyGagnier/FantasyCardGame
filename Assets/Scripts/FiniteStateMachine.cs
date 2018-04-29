using UnityEngine;
using System;
using System.Collections.Generic;

public class Transition<T>
{
    public int from;
    public int to;
    public Func<T> transitionFn;

    public Transition(int from, int to, Func<T> transitionFn)
    {
        this.from = from;
        this.to = to;
        this.transitionFn = transitionFn;
    }
}

public class FiniteStateMachine<T>
{
    private int currentState;
    private Dictionary<long, Func<T>> transitions = new Dictionary<long,Func<T>>();

    public FiniteStateMachine(int initialState, List<Transition<T>> transitions)
    {
        currentState = initialState;
        foreach (Transition<T> transition in transitions)
        {
            long index = (((long)transition.from) << 32) + (long)transition.to;
            if (this.transitions.ContainsKey(index))
            {
                Debug.LogError(string.Format("Duplicate transition from state: {0} to state: {1}", transition.from, transition.to));
            }
            this.transitions.Add(index, transition.transitionFn);
        }
    }

    public T Transition(int to)
    {
        long index = (((long)currentState) << 32) + (long)to;
        if (!transitions.ContainsKey(index))
        {
            Debug.LogError(string.Format("Missing transition from state: {0} to state: {1}", currentState, to));
        }
        currentState = to;
        return transitions[index]();
    }
}
