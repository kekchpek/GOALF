using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootFireChecker : MonoBehaviour {

    public GameObject footFire;

	void OnTriggerStay2D(Collider2D other)
    {
        footFire.SetActive(false);
    }
}
