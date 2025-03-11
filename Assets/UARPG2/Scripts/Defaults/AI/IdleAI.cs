using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAI : AI
{
    protected virtual void Awake()
    {
        _states = new StateActions[]
        {
            new StateActions
            (
                (a, c) => { },
                (a, c) => { },
                (a, c) => { }
            )
        };
    }
}
