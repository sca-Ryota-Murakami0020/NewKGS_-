using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�R���|�[�l���g�̎g�p

public class Menu : MonoBehaviour
{
    Button cube;
    Button sphere;
    Button cylinder;
    // Start is called before the first frame update
    void Start()
    {
        // �{�^���R���|�[�l���g�̎擾
        cube = GameObject.Find("/Canvas/Button1").GetComponent<Button>();
        sphere = GameObject.Find("/Canvas/Button2").GetComponent<Button>();
        cylinder = GameObject.Find("/Canvas/Button3").GetComponent<Button>();

        // �ŏ��ɑI����Ԃɂ������{�^���̐ݒ�
        cube.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
