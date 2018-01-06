using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseZone : MonoBehaviour {

    public GameScreenController controller;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            controller.Lose();
        }
        if (other.tag != "Respawn" && other.tag != "Player" && other.tag != "Finish")
        {
            Destroy(other.gameObject);
        }
        if(other.tag == "Finish")
        {
            StartCoroutine(Coroutine1(other.gameObject));
        }
    }

    IEnumerator Coroutine1(GameObject gObj)
    {
        yield return new WaitForSeconds(0.5f);
        gObj.SetActive(false);
    }

}
