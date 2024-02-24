using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GutsGaugeC : MonoBehaviour
{
    [Header("�Q�[�W�i�΁A�ԁA���̏��j"),SerializeField]
    private Image[] gaugeImage;
    [SerializeField]
    private PlayerC playerPlam;
    [Header("�\������"),SerializeField]
    private int showGaugeTime;
    [Header("�X�^�~�i�̍ő�l"),SerializeField]
    private int maxGuts;

    //�b���v�Z
    private float currentShowTime = 0.0f;
    //���݂̗̑�
    private float nowGuts;�@
    //�񕜏I���̃T�C���i�\�����Ԃ��v�Z����j
    private bool finisfRecover = false;

    public enum GutsPlam
    {
        CanUse,
        Doing,
        Recover,
        CoolTime
    };
    private GutsPlam gutsPlam = GutsPlam.CanUse;

    public GutsPlam GPlam
    {
        get { return this.gutsPlam;}
        set { this.gutsPlam = value;}
    }

    //�v������
    // Start is called before the first frame update
    void Start()
    {
        for(int count = 0; count < gaugeImage.Length; count++)
        {
            gaugeImage[count].fillAmount = 1;
            gaugeImage[count].enabled = false;
        }
        nowGuts = maxGuts;
    }

    // Update is called once per frame
    void Update()
    {
        switch(gutsPlam)
        {
            case GutsPlam.Doing:
                if(playerPlam.IsGrand)
                PlayDash();
                break;
            case GutsPlam.Recover:
                RecoverGuts();
                break;
            case GutsPlam.CoolTime:
                CantDashTime();
                break;
            default:
                break;
        }
        if(finisfRecover) CountShowTime();
    }

    //����
    private void PlayDash()
    {
        if(finisfRecover) finisfRecover = false;
        for(int count = 0; count < gaugeImage.Length; count++)
        {
            if (!gaugeImage[count].enabled)
                gaugeImage[count].enabled = true;
        }
            
        nowGuts -= 0.1f;
        var rate = nowGuts/maxGuts;
        gaugeImage[0].fillAmount = rate;
        gaugeImage[1].fillAmount = rate + 0.05f;

        if(nowGuts <= 0.0f)
        {
            gutsPlam = GutsPlam.CoolTime;
            playerPlam.EndDash();
        }
    }

    public void JugeStamina()
    {
        if(nowGuts > 0.0f)
            gutsPlam = GutsPlam.Recover;
        else
            gutsPlam = GutsPlam.CoolTime;
    }

    //�����I��
    private void CantDashTime()
    {
        gaugeImage[0].enabled = false;
        RecoverGuts();
    }

    //��
    private void RecoverGuts()
    {
        if(gutsPlam == GutsPlam.Recover)
            gaugeImage[0].enabled = true;
        nowGuts += 0.1f;
        var rate = nowGuts / maxGuts;
        for(int count = 0; count < gaugeImage.Length - 1; count++)
            if(gaugeImage[count].enabled)
                gaugeImage[count].fillAmount = rate;

        if(rate >= 1)
        {
            if(!gaugeImage[0].enabled)
            {
                gaugeImage[0].enabled = true;
                gaugeImage[0].fillAmount = rate;
            }
            gutsPlam = GutsPlam.CanUse;
            finisfRecover = true;
        }
    }

    //�\�����Ԃ̌v�Z
    private void CountShowTime()
    {
        currentShowTime += 0.01f;
        if(currentShowTime >= showGaugeTime)
        {
            for(int count = 0; count < gaugeImage.Length; count++)
                gaugeImage[count].enabled = false;
            currentShowTime = 0.0f;
            finisfRecover = false;
        }
    }
}
