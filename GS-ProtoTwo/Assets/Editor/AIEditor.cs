using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyMovement))]
public class AIEditor : Editor {

    string[] options = new string[]
 {
     "None", "Path Points", "Path Points with NAVMESH" , "Wander" 
 };

    bool showPosition = true;

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
            case EnemyMovement.BehaviourType.WANDER:
                {
                    thisTarget.WanderRadius = EditorGUILayout.FloatField("Radius of wandering", thisTarget.WanderRadius);
                    break;
                }

                    default:
            {

                break;
            }
        }

        showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, "Enemy Awareness");

        if (showPosition)
        {
            EditorGUILayout.Separator();
            thisTarget.PlayerFoundObj = (GameObject)EditorGUILayout.ObjectField("Player Found Indicator", thisTarget.PlayerFoundObj, typeof(Object), true);
            thisTarget.PlayerLostObj = (GameObject)EditorGUILayout.ObjectField("Player Lost Indicator", thisTarget.PlayerLostObj, typeof(Object), true);
            thisTarget.DetectRange = EditorGUILayout.FloatField("FOV Distance", thisTarget.DetectRange);
            thisTarget.EnemyFov = EditorGUILayout.FloatField("FOV Angle", thisTarget.EnemyFov);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    float sizeOfHandle = 0.5f;
    private void OnSceneGUI() 
    {
        Handles.color = Color.white;
        EnemyMovement thisTarget = (EnemyMovement)target;
        //float size = HandleUtility.GetHandleSize(thisTarget.To) * sizeOfHandle;
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


            case EnemyMovement.BehaviourType.WANDER:
            {
                    EditorGUI.BeginChangeCheck();
                    if (Application.isPlaying)
                    {
                        Handles.DrawWireDisc(thisTarget.storedHandlePos, Vector3.up, thisTarget.WanderRadius);

                    }
                    else
                    {
                        Handles.DrawWireDisc(thisTarget.transform.position, Vector3.up, thisTarget.WanderRadius);

                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        //Undo.RecordObject(thisTarget, "Move ToFrom");
                        //thisTarget.Targets[i] = newTo;
                    }

                    break;

            }
            default:
            {

                break;
            }
        }

        
        Handles.color = Color.red;
        Handles.DrawWireDisc(thisTarget.transform.position, Vector3.up, thisTarget.DetectRange);

        Vector3 viewAngleA = thisTarget.DirFromAngle(-thisTarget.EnemyFov / 2, false);

        Handles.DrawSolidArc(thisTarget.transform.position, thisTarget.transform.up, viewAngleA, thisTarget.EnemyFov, thisTarget.DetectRange);

        Handles.color = Color.blue;
        if (thisTarget.PlayerObj != null)
        {
            Handles.DrawLine(thisTarget.transform.position, thisTarget.PlayerObj.transform.position);
        }

    }


}

