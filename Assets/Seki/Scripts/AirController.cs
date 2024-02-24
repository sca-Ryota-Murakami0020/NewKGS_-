using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirController : MonoBehaviour
{
    [SerializeField] GameObject Pos;
    [Header("��s�@�X�s�[�h"), SerializeField] float speed;
    public float AIRSPEED {
        set {
            this.speed = value;
        }
        get {
            return this.speed;
        }
    }
    bool hit = false;

    int hitCount = 0;
    public int HITCOUNT {
        set {
            this.hitCount = value;
        }
        get {
            return this.hitCount;
        }
    }

    [SerializeField] GameManager gameManager;
    [SerializeField] PlayerC player;
    [SerializeField] GameObject missimage;
    [SerializeField] GameObject[] missIcon;
    [SerializeField] PauseManager pause;
    [SerializeField] MissionManager missionManager;
    //�����ɔ����̃G�t�F�N�g��p��

    bool air = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(!pause.PAUSE && !player.FALLING && !missionManager.MISSIONFLAG) {

        
        if(!hit) {
            var delta = this.transform.position - Pos.transform.position;

            // �Î~���Ă����Ԃ��ƁA�i�s���������ł��Ȃ����߉�]���Ȃ�
            if(delta == Vector3.zero)
                return;

            // �i�s�����i�ړ��ʃx�N�g���j�Ɍ����悤�ȃN�H�[�^�j�I�����擾
            //var rotation = Quaternion.LookRotation(delta, Vector3.up);

            // �I�u�W�F�N�g�̉�]�ɔ��f
            //this.transform.rotation = rotation;


            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Pos.transform.position, step);
        } else {
            for(int i = 0; i < missIcon.Length; i++) {
                missIcon[i].SetActive(false);
            }

            missimage.SetActive(false);
        }
        }

        if(hitCount >= 3 && !air) {
            
            //�����ɔ����̃G�t�F�N�g���o��
            StartCoroutine(WaitActive());
            air = true;
        }
    }

    IEnumerator WaitActive() {
        yield return null;
        missionManager.MISSIONVALUE[missionManager.RADOMMISSIONCOUNT] = 1;
        missionManager.KeyActive(missionManager.RADOMMISSIONCOUNT);
        missionManager.MiSSIONCOUNT++;
        this.gameObject.SetActive(false);
    }
    

    private void OnCollisionEnter(Collision collision) {    
        if(collision.gameObject.tag == "air") {
            hit = true;
            gameManager.GAMEOVER = true;
            gameManager.CurrentRemain = -1;
            
            player.FALLING = true;
            Debug.Log("HITT");
        }
        
    }
}
