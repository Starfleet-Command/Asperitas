using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnAfterSeconds : MonoBehaviour
{
    public float timeTilDespawn=5f;
    public void StartCountdown()
    {
        StartCoroutine("DespawnAfterTime");
    }

    IEnumerator DespawnAfterTime()
    {
        yield return new WaitForSeconds(timeTilDespawn);
        Destroy(this.gameObject);
    }
}
