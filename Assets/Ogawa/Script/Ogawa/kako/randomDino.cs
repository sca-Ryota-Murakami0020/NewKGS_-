using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomDino : MonoBehaviour
{
    [SerializeField] GameObject randomEvent1;
    [SerializeField] GameObject randomEvent2;
    [SerializeField] GameObject randomEvent3;

    [SerializeField] KakoPauseManager pauseManager;

    private int randomEvent;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        randomEvent = Random.Range(1, 4);
        Debug.Log(randomEvent + "randomEvent");
    }

    // Update is called once per frame
    void Update()
    {
        if(!pauseManager.LOAD) { 
            

            if(randomEvent == 1 && !isActive) {
                randomEvent2.SetActive(false);
                randomEvent3.SetActive(false);
                isActive = true;
            } else if(randomEvent == 2 && !isActive) {
                randomEvent1.SetActive(false);
                randomEvent3.SetActive(false);
                isActive = true;
            } else if(randomEvent == 3 && !isActive) {
                randomEvent1.SetActive(false);
                randomEvent2.SetActive(false);
                isActive = true;
            }
            pauseManager.LOAD = true;
        }
    }
}
