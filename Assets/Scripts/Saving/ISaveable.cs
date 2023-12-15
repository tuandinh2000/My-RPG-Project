using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public object CaptureState();

    public void RestoreState(object state);
}
