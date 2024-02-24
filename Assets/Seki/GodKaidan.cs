using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodKaidan : MonoBehaviour
{
    //������LB�Ő_�̖ڂ��g����UI��\������
    [SerializeField] GameObject godUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            //������UI���o��
            godUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player") {
            //������UI������
            godUI.SetActive(false);
        }
    }
}
