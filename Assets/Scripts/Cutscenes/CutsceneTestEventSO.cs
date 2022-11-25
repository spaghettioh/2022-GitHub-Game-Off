using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (
    fileName = "CutsceneTestEvent", menuName = "Game Off/Cutscene Tester")]
public class CutsceneTestEventSO : ScriptableObject
{
    public UnityAction<CutsceneScreenSO> OnCutsceneChanged;

    public void Raise(CutsceneScreenSO screen, string elevator = "?")
    {
        if (OnCutsceneChanged != null)
            OnCutsceneChanged.Invoke(screen);
        else
            Debug.LogWarning($"{elevator} raised {name}, unheard, DGAF");
    }
}
