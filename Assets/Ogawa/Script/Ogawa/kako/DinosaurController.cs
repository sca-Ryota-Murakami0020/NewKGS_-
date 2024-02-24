using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurController : MonoBehaviour
{
    [SerializeField] Transform startPoint;       //�����̐����ʒu
    [SerializeField] Transform endPoint;         //�����̈ړ�����ʒu
    [SerializeField] Transform bootPoint;        //�����Ƀv���C���[���N�������狰���𓮂���

    [SerializeField] GameObject player;

    private float elapsedTime = 0f;              //�o�ߎ���
    [SerializeField] float moveSpeed;        //startPos����endPos�̈ړ��ɂ����鎞��

    [SerializeField] float genelateDistance = 0.5f;//���b��1�̐������邩
    private float distanceCount = 0f;    //�J�E���g

    void Start()
    {
        startPoint.position = this.transform.position;
    }

    private void Update()
    {
        
        
        if (player.transform.position.z >= bootPoint.position.z )
        {
            distanceCount+=Time.deltaTime;
            if (genelateDistance < distanceCount)
            {
                Spawn();
                distanceCount = 0f;
            }
            if(once == false)
            {
                Move();
            }

        }

    }

    bool once = true;
    int count = 0;
    void Spawn()
    {
        //�����𐶐�
        if (once)
        {
            Instantiate(this, startPoint.position, Quaternion.Euler(0f, 90f, 0f));
            once = false;
            count++;
        }


        
    }
    void Move()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < moveSpeed)
        {
            float t = elapsedTime / moveSpeed;
            this.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

        }
        else
        {
            // �ړ��������̏���
            this.gameObject.SetActive(false);

        }
    }
}
