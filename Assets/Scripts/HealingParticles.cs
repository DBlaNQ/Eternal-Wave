using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteYourself(3f));
    }

    IEnumerator DeleteYourself(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
