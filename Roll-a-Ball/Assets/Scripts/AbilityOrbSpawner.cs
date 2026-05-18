using UnityEngine;
using System.Collections;

public class AbilityOrbSpawner : MonoBehaviour
{
    public GameObject abilityOrbPrefab;   
    public Transform ground;              
    public float orbLifetime = 10f;     
    public LayerMask obstacleMask;
    public float initialDelay = 5f;
    public float respawnDelay = 5f;

    [HideInInspector] public GameObject currentOrb;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            if (currentOrb == null)
            {
                yield return new WaitForSeconds(respawnDelay);
                Vector3 groundSize = ground.localScale;
                float halfSizeX = groundSize.x * 5f; 
                float halfSizeZ = groundSize.z * 5f;

                Vector3 randomPos;
                int attempts = 0;

                do
                {
                    randomPos = new Vector3(
                        Random.Range(-halfSizeX, halfSizeX),
                        0.5f,
                        Random.Range(-halfSizeZ, halfSizeZ)
                    );
                    attempts++;
                }
                while (Physics.CheckSphere(randomPos, 0.5f, obstacleMask) && attempts < 20);

                currentOrb = Instantiate(abilityOrbPrefab, randomPos, Quaternion.identity);
                Destroy(currentOrb, orbLifetime);
            }

            yield return null;
        }
    }
}
