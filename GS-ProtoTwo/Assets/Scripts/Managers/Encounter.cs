using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Encounter", menuName = "Manager/Encounter")]
public class Encounter : ScriptableObject
{
    public List<GameObject> enemyPrefabs;

    public void EndEncounter()
    {
        enemyPrefabs.Clear();
    }

    public void AddEnemy(GameObject _enemy)
    {
        enemyPrefabs.Add(_enemy);
    }
}
