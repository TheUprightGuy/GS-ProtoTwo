using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EncounterStart : MonoBehaviour
{
    public string CombatSceneToLoad;
    public Encounter EncounterSO;
    public GameObject transitionCanvas;

    private void Start()
    {
        EncounterSO.enemyPrefabs.Clear();
    }

    public void GoToCombat()
    {
        SceneTransition.instance.GoToCombat();
    }

    public void EncounterOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyPrefabs>())
        {
            foreach (GameObject item in collision.gameObject.GetComponent<EnemyPrefabs>().EnemyGroup)
            {
                EncounterSO.AddEnemy(item);
            }

            Destroy(collision.gameObject);
            GoToCombat();
        }
    }
}
