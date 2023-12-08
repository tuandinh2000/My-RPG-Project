using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSchedular : MonoBehaviour
{
    private IAction _currentAction;

    public void StartAction(IAction action)
    {
        if (_currentAction == action) return;

        CancelAction();
        _currentAction = action;
    }

    public void CancelAction()
    {
        if (_currentAction == null) return;

        _currentAction.CancelAction();
        _currentAction = null;
    }
}
