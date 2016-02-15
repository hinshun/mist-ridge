using UnityEngine;

// Workaround for bug in Unity due to loading scene with Continous Baking turned on
// Remove when postponed issue is resolved:
// https://issuetracker.unity3d.com/issues/loadlevel-gi-and-reflection-probes-are-not-loaded-with-the-scene-in-editor-if-auto-baking-mode-is-used
public class UnityFixGI : MonoBehaviour
{
    private void OnLevelWasLoaded(int level) {
#if UNITY_EDITOR
        if (UnityEditor.Lightmapping.giWorkflowMode == UnityEditor.Lightmapping.GIWorkflowMode.Iterative) {
            DynamicGI.UpdateEnvironment();
        }
#endif
    }
}
