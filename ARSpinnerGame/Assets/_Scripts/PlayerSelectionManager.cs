
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{

    public Transform playerSwitcherTransform;
    public Button button_next;
    public Button button_last;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void NextPlayer()
    {
        button_next.enabled = false;
        button_last.enabled = false;
        //1 second to make turn between players
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform ,90, 1.0f));
    }

    public void LastPlayer()
    {
        button_next.enabled = false;
        button_last.enabled = false;
        //1 second to make turn between players
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform ,-90, 1.0f));
    }

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {

        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation*Quaternion.Euler(axis*angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation,elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;
        button_next.enabled = true;
        button_last.enabled = true;


    }



}
