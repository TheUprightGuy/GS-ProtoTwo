using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EncounterStart : MonoBehaviour
{
    public string CombatSceneToLoad;
    public Encounter EncounterSO;

    private void Start()
    {
        EncounterSO.enemyPrefabs.Clear();
    }

    public void GoToCombat()
    {
        //Un-comment for scene loading :)
        //SceneManager.LoadScene(CombatSceneToLoad);

        Debug.Log("Ping");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<EnemyPrefabs>())
        {
            foreach (GameObject item in collision.gameObject.GetComponent<EnemyPrefabs>().EnemyGroup)
            {
                EncounterSO.AddEnemy(item);
            }

            GoToCombat();
        }
    }
}
