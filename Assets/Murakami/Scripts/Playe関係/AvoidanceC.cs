using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvoidanceC : MonoBehaviour
{
    [Header("����Q�[�W�i���g�A���g�i�ԁj�A�O�̏��Łj"),SerializeField]
    private Image[] avoiGauge;
    [Header("����̃N�[���^�C��"),SerializeField]
    private int avoiCoolTime;
    [Header("�Q�[�W�̕\������"),SerializeField]
    private int showAvoiGaugeTime;

    public enum AvoiPlam
    {
        CanUse,
        Doing,
        CoolTime
    };

    private AvoiPlam avoiPlam = AvoiPlam.CanUse;

    //���s���̃Q�[�W����
    private float currentAvoiGauge = 0.0f;
    //���s���̃Q�[�W�\������
    private float currentShowGaugeTime = 0.0f;
    //�\�����Ԍv�Z
    private bool countShowTime = false;

    public AvoiPlam AvoiP
    {
        get { return this.avoiPlam;}
        set { this.avoiPlam = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int count = 0; count < avoiGauge.Length; count++)
            avoiGauge[count].enabled = false;
        currentAvoiGauge = avoiCoolTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(avoiPlam == AvoiPlam.CoolTime)
            RecoverAvoidanceGauge();
        if(countShowTime)
            CountShowGauge();
    }

    //�Q�[�W�g�p
    public void UsingAvoidanceGauge()
    {
        //�Ԃ̃Q�[�W�����ƍ��̃Q�[�W�̂ݕ\������B
        for(int count = 1; count < avoiGauge.Length; count++)
            avoiGauge[count].enabled = true;
        currentAvoiGauge = 0.0f;
        avoiGauge[1].fillAmount = 0.0f;
    }

    //�Q�[�W��
    private void RecoverAvoidanceGauge()
    {
        currentAvoiGauge += 0.05f;
        var rate = currentAvoiGauge / avoiCoolTime;
        avoiGauge[1].fillAmount = rate;
        if(rate >= 1)
        {
            currentAvoiGauge = avoiCoolTime;
            avoiGauge[0].enabled = true;
            countShowTime = true;
            avoiPlam = AvoiPlam.CanUse;
        }
    }

    //�Q�[�W�\�����Ԍv��
    private void CountShowGauge()
    {
        currentShowGaugeTime += 0.07f;
        if(currentShowGaugeTime >= showAvoiGaugeTime)
        {
            for(int count = 0; count < avoiGauge.Length; count++)
                avoiGauge[count].enabled = false;
            currentShowGaugeTime = 0.0f;
            countShowTime = false;
        }
    }
}
