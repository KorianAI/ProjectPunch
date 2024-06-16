using UnityEngine;
using UnityEditor;

public class BehaviourFinder : MonoBehaviour
{
    public MonoBehaviour[] behaviours;

#if UNITY_EDITOR
    [CustomEditor(typeof(BehaviourFinder))]
    public class BehaviourFinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            BehaviourFinder behaviourFinder = (BehaviourFinder)target;

            if (GUILayout.Button("Find MonoBehaviours"))
            {
                // Call a method to find all MonoBehaviour components
                behaviourFinder.FindAllMonoBehaviours();
            }
        }
    }
#endif

    // Method to find all MonoBehaviour components
    public void FindAllMonoBehaviours()
    {
        behaviours = FindObjectsOfType<MonoBehaviour>();
    }
}
