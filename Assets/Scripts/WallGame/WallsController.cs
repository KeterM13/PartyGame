using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WallsController : NetworkBehaviour
{
    [SerializeField] int randomNumber;
    [SerializeField] int randomSpawn;

    public List<Transform> spawnPoint;
    public List<GameObject> myPlatforms;

    
    IEnumerator SpawnPlatform()
    {
        randomNumber = Random.Range(0, myPlatforms.Count);
        randomSpawn = Random.Range(0, spawnPoint.Count);

        var intance= Instantiate(myPlatforms[randomNumber], spawnPoint[randomSpawn]);

        

        NetworkServer.Spawn(intance);
        yield return new WaitForSeconds(20f);
        StartCoroutine(SpawnPlatform());
    }

    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform")
        {
            StartCoroutine(SpawnPlatform());
            Destroy(other.gameObject);
        }
    }
}
