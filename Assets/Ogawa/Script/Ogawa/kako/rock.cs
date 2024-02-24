using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    [SerializeField] GameObject obstacleObject;//��
    [SerializeField] GameObject player;
    [SerializeField] int shotDistance;     //��̔��ˊԊu
    [SerializeField] int shotPower;        //��̏��� 

    private int height = 10;
    private int distance = 60;
    [SerializeField] RunOnlyPlayerC playerC;
    [SerializeField] KakoPauseManager pause;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    float createCount = 0f;
    // Update is called once per frame
    void Update()
    {
        if(!playerC.FALLING && !pause.PAUSE) { 
            //��̐���
            if(shotDistance < createCount)
            {
                Roll();
                createCount = 0f;
            }
            createCount += Time.deltaTime;
        }
    }

    //���]����
    private void Roll()
    {
        //��̐���
        float spawnX = Random.Range(-1.6f,1.6f);
        Vector3 spawn = new Vector3(spawnX,player.transform.position.y + height ,player.transform.position.z + distance);
        GameObject rockObject = Instantiate(obstacleObject,spawn,Quaternion.identity);
        Rigidbody rockRigidbody = rockObject.GetComponent<Rigidbody>();
        rockRigidbody.velocity = new Vector3(0f,0f,-shotPower);
    }
}
