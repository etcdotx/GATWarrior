using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSpawn : MonoBehaviour
{
    public static EndlessSpawn instance;


    public GameObject blueSlimePrefab;
    public GameObject greenSlimePrefab;
    public GameObject redSlimePrefab;
    public GameObject flyingPrefab;

    public Transform blueSlimeSpawnPos;
    public Transform greenSlimeSpawnPos;
    public Transform redSlimeSpawnPos;
    public Transform flyingSpawnPos;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
    // Start is called before the first frame update
    public void Spawn(string name) {
        StartCoroutine(SpawnTimer(name));
    }

    IEnumerator SpawnTimer(string name)
    {
        yield return new WaitForSeconds(10);
        if (name.Equals("blue"))
        {
            Instantiate(blueSlimePrefab, blueSlimeSpawnPos.position, Quaternion.identity, null);
        }
        else if (name.Equals("green"))
        {
            Instantiate(greenSlimePrefab, greenSlimeSpawnPos.position, Quaternion.identity, null);

        }
        else if (name.Equals("red"))
        {
            Instantiate(redSlimePrefab, redSlimeSpawnPos.position, Quaternion.identity, null);

        }
        else if (name.Equals("fly"))
        {
            Instantiate(flyingPrefab, flyingSpawnPos.position, Quaternion.identity, null);

        }
    }
}
