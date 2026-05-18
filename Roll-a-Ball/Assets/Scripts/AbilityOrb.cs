using UnityEngine;

public class AbilityOrb : MonoBehaviour
{
    void OnDestroy()
    {
        AbilityOrbSpawner[] spawners = Object.FindObjectsByType<AbilityOrbSpawner>(FindObjectsSortMode.None);
        foreach (var spawner in spawners)
        {
            spawner.currentOrb = null;
        }
    }
}
