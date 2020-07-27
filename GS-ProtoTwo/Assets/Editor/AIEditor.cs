using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyMovement))]
public class AIEditor : Editor {

    string[] options = new string[]
 {
     "None", "Path Points", "Path Points with NAVMESH" , "Wander" 
 };
    public override void OnInspectorGUI() 
    {
        //base.OnInspectorGUI();
        EnemyMovement thisTarget = (EnemyMovement)target;
        int index = (int)thisTarget.Behaviour;
        thisTarget.Behaviour = (EnemyMovement.BehaviourType)EditorGUILayout.Popup((int)thisTarget.Behaviour, options);

        switch (thisTarget.Behaviour)
        {
            case EnemyMovement.BehaviourType.TO_FROM_NAVMESH: //Give these two the same vars
            case EnemyMovement.BehaviourType.TO_FROM:
            {
                    //thisTarget.To = EditorGUILayout.Vector3Field("To", thisTarget.To);
                    //thisTarget.From = EditorGUILayout.Vector3Field("From", thisTarget.From);
                    //thisTarget.lockY = thisTarget.Targets.Toggle("Lock Y", thisTarget.lockY);

                    if (GUILayout.Button("Add new point"))
                    {
                        thisTarget.Targets.Add(Vector3.zero);
                    }

                    if (GUILayout.Button("Delete Last") && thisTarget.Targets.Count != 0)
                    {
                        thisTarget.Targets.RemoveAt(thisTarget.Targets.Count - 1);
                    }
                    break;
            }

            default:
            {

                break;
            }
        }
    }

    float sizeOfHandle = 0.5f;
    private void OnSceneGUI() 
    {
        EnemyMovement thisTarget = (EnemyMovement)target;
        float size = HandleUtility.GetHandleSize(thisTarget.To) * sizeOfHandle;
        Vector3 snap = Vector3.one * 0.5f;

        switch (thisTarget.Behaviour)
        {
            case EnemyMovement.BehaviourType.TO_FROM_NAVMESH: //Give these two the same vars
            case EnemyMovement.BehaviourType.TO_FROM:
            {
                for (int i = 0; i < thisTarget.Targets.Count; i++)
                {
                        EditorGUI.BeginChangeCheck();
                        Vector3 toPos;

                        if (Application.isPlaying)
                        {
                            toPos = thisTarget.Targets[i];
                        }
                        else
                        {
                            toPos = thisTarget.transform.position + thisTarget.Targets[i];
                        }

                    Vector3 newTo = Handles.FreeMoveHandle(toPos ,
                        Quaternion.identity, sizeOfHandle, snap, Handles.SphereHandleCap) - thisTarget.transform.position;
                    if (thisTarget.lockY)
                    {
                        newTo.y = 0;
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(thisTarget, "Move ToFrom");
                        thisTarget.Targets[i] = newTo;
                    }
                }

                
                break;
            }

            default:
            {

                break;
            }
        }
        
        
        
    }
}

