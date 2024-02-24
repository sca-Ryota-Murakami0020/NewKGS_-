using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinC : MonoBehaviour
{
    [Header("�R�C���̉��l"),SerializeField] private int coinPrice;
    [Header("�X�R�A"),SerializeField] private int getScore;
    [Header("�G�t�F�N�g"), SerializeField] private ParticleSystem partical;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Material[] cainMaterial;
    [SerializeField] private Renderer coinRenderer;
    //�R�C���̃��b�V��
    [SerializeField] private MeshRenderer mesh;

    //�v���C���[�ƃR�C���̋���
    private float playerDistance = 0.0f;
    //�v���C���[
    private GameObject playerObject;
    //�v���C���[�̍��W
    private Vector3 playerPosition = Vector3.zero;
    //�v���C���[�Ɍ��������x
    private float _speed = 2.0f;
    //���X�̑傫��
    private Vector3 defScale;
    //�k�����̃X�P�[���{��
    private Vector3 currentScaleMag;
    //�����ʒu
    private Vector3 defPos;
    //�v���C���[�̒ǐՂ��s��
    private bool chasePlayer;
    //PlayerC
    private PlayerC playerC;
    //�傫���̍ŏ��l
    private Vector3 minScale;

    private GameManager gameManager;
    MissionManager mission;

    void Start()
    {
        SelectMaterial();
        mission = GameObject.Find("MissionManager").GetComponent<MissionManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerObject = GameObject.Find("PlayerModel");
        playerC = playerObject.GetComponent<PlayerC>();
        defScale = this.transform.localScale;
        defPos = this.transform.position;
        currentScaleMag = defScale;
        playerPosition = playerObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(mesh.enabled)
        {
            CheckPlayerDistance();
            if (playerDistance <= 1.7f)
            {
                //Debug.Log("�֐��擾");
                chasePlayer = true;
            }

            if (chasePlayer)
            {
                MoveCoin();
            }

            //�v���C���[��������ɃR�C���̕����Ɉړ����Ă�����
            if (playerDistance <= 0.1f)
            {
                GetCoin();
            }
        }
    }

    //�F�I��
    private void SelectMaterial()
    {
        switch(coinPrice)
        {
            case 10:
                this.coinRenderer.material = cainMaterial[0];
                break;
            case 100:
                this.coinRenderer.material = cainMaterial[1];
                break;
            case 500:
                this.coinRenderer.material = cainMaterial[2];
                break;
        }
    }

    //�v���C���[�Ƃ̋����𑪒肷��
    private void CheckPlayerDistance()
    {
        playerPosition = playerObject.transform.position;
        playerDistance = Vector3.Distance(playerPosition, this.transform.localPosition);
    }

    //�߂Â����v���C���[�̕����ֈړ�����
    private void MoveCoin()
    {
        var target = playerPosition;
        var distance = (_speed * Time.deltaTime) / playerDistance;
        this.transform.position = Vector3.Lerp(this.transform.position,target,distance);
        currentScaleMag = new Vector3(currentScaleMag.x - 2.0f, currentScaleMag.y, currentScaleMag.z - 2.0f);
        //�k��������������v���C���[�̈ʒu�Ɉړ�����
        if((currentScaleMag.x <= 2.0f && currentScaleMag.z <= 2.0f) ||
            (currentScaleMag.x <= 2.0f || currentScaleMag.z <= 2.0f) ||
            (this.transform.localScale.x <= 0.1 || this.transform.localScale.z <= 0.1f))
        {
            currentScaleMag.x = 0.0f;
            currentScaleMag.z = 0.0f;
            this.transform.position = playerPosition;
        }
        this.transform.localScale = currentScaleMag;
    }

    //�R�C���̊l��
    private void GetCoin()
    {
        this.mesh.enabled = false;
        chasePlayer = false;
        gameManager.AddCoins(coinPrice, getScore);
        gameManager.AddActiveCoin(this.gameObject);
        //�G�t�F�N�g�Đ�
        ParticleSystem newPar = Instantiate(partical);
        newPar.transform.position = playerC.PopObject.transform.position;
        newPar.Play();
        if (mission.RADOMMISSIONCOUNT == 2)
        {
            mission.MISSIONVALUE[mission.RADOMMISSIONCOUNT] += coinPrice;
        }
        this.transform.localScale = defScale;
        this.transform.localPosition = defPos;
        //this.gameObject.SetActive(false);
    }
}
