using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDebufC : MonoBehaviour
{
    [Header("�f�o�t���ɌĂяo���G�t�F�N�g"),SerializeField]
    private ParticleSystem debufEffect;
    [Header("�W�����v�f�o�t�A�C�R��"),SerializeField]
    private Image jumpDebufIcon;
    [Header("�ړ��f�o�t�A�C�R��"),SerializeField]
    private Image moveDebufIcon;
    [SerializeField] private PlayerC playerC;

    //���Ԋ֌W
    private float currentMoveDebufTime = 0.0f;
    private int maxMoveDebufTime = 0;
    private bool flashingMoveIcon = false;
    private float currentJumpDebufTime = 0.0f;
    private int maxJumpDebufTime = 0;
    private bool flashingJumpIcon = false;

    //�{���֌W
    private float moveDebufMag = 1.0f;
    private float oldMoveDebufMag = 0.0f;
    private float jumpDebufMag = 1.0f;
    private float oldJumpDebufMag = 0.0f;

    //�t���O�֌W
    private bool onMoveDebuf = false;
    private bool onJumpDebuf = false;

    //PlayerManager
    private PlayerManager playerManager;

    public float MoveDebufMag
    {
        get { return this.moveDebufMag; }
    }
    public float JumpDebufMag
    {
        get { return this.jumpDebufMag;}
    }

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        jumpDebufIcon.enabled = false;
        moveDebufIcon.enabled = false;
        moveDebufMag = playerManager.DefSpeedMag;
        jumpDebufMag = playerManager.DefJumpMag;
    }

    // Update is called once per frame
    void Update()
    {
        if(onMoveDebuf)
            CountMoveDebuf();
        if(onJumpDebuf)
            CountJumpDebuf();
    }

    #region//�ړ��f�o�t
    //�ړ��p�f�o�t�N��
    public void ActiveMoveDebuf(int time, float mag)
    {
        //�ŏ��i���߂ăf�o�t���󂯂����j�̏���
        if(moveDebufMag == playerManager.DefSpeedMag && !onMoveDebuf)
        {
            onMoveDebuf = true;
            //�v���C���[�����̃f�o�t���󂯂Ă��Ȃ���ԂȂ�
            if (!onJumpDebuf)
                playerC.PopDebufEffect();
            moveDebufIcon.enabled = true;
            //�����̌v�Z
            var def = 0.0f;
            if(moveDebufMag > mag) def = moveDebufMag - mag;
            else def = mag - moveDebufMag;

            moveDebufMag = def;
            oldMoveDebufMag = moveDebufMag;

            maxMoveDebufTime = time;
            Debug.Log( "���ԁF" + maxMoveDebufTime);
        }
        //�f�o�t�����擾�����{���̕��������ꍇ
        else if (onMoveDebuf)
        { 
            //�{���̔�r
            var mag1 = oldMoveDebufMag;
            var mag2 = playerManager.DefSpeedMag - mag;
            //����̔{�����O��̔{���������Ă���i���l�I�ɂ͏��������j�Ȃ�
            if(mag1 > mag2)
            {
                moveDebufMag = mag2;
                //�t�^�����\��̌��ʎ��Ԃ��O��̕t�^���Ԃ������Ă���Ȃ�
                if (maxMoveDebufTime < time)
                {
                    //���s���Ԃ̃��Z�b�g
                    currentMoveDebufTime = 0.0f;
                    maxMoveDebufTime = time;
                }
            }
            //������Ă���Ȃ���ɉ������Ȃ�
            else moveDebufMag = mag1;
        }
    }
    //���Ԍv�Z�i�ړ��p�j
    private void CountMoveDebuf()
    {
        currentMoveDebufTime += 0.01f;
        Debug.Log("�o�ߎ���" + currentMoveDebufTime);
        if(currentMoveDebufTime == maxMoveDebufTime * 0.8 && !flashingMoveIcon)
        {
            StartCoroutine(FlashingMoveDebuf());
            flashingMoveIcon = true;
        }
        if(currentMoveDebufTime >= maxMoveDebufTime)
            EndMoveDebuf();
    }

    //�A�C�R���̓_��
    private IEnumerator FlashingMoveDebuf()
    {
        while (currentMoveDebufTime <= maxMoveDebufTime * 0.99)
        {
            yield return new WaitForSeconds(0.1f);
            moveDebufIcon.enabled = false;
            yield return new WaitForSeconds(0.1f);
            moveDebufIcon.enabled = true;
        }
    }

    //�ړ��f�o�t�̏I��
    private void EndMoveDebuf()
    {
        flashingMoveIcon = false;
        onMoveDebuf = false;
        moveDebufIcon.enabled = false;
        moveDebufMag = playerManager.DefSpeedMag;
        currentMoveDebufTime = 0.0f;
        oldMoveDebufMag = 0.0f;
        maxMoveDebufTime = 0;
        if (!onJumpDebuf && !onMoveDebuf)
            playerC.DeleteDebufEffect();
    }
    #endregion

    #region//�W�����v�p
    //�W�����v�p�f�o�t�N��
    public void ActiveJumpDebuf(int time, float mag)
    {
        //�ŏ��i���߂ăf�o�t���󂯂����j�̏���
        if (jumpDebufMag == playerManager.DefJumpMag && !onJumpDebuf)
        {
            onMoveDebuf = true;
            //�v���C���[�����̃f�o�t���󂯂Ă��Ȃ���ԂȂ�
            if (!onMoveDebuf)
                playerC.PopDebufEffect();
            jumpDebufIcon.enabled = true;
            //�����̌v�Z
            var def = 0.0f;
            if (jumpDebufMag > mag) def = jumpDebufMag - mag;
            else def = mag - jumpDebufMag;

            jumpDebufMag = def;
            oldJumpDebufMag = jumpDebufMag;

            maxMoveDebufTime = time;
        }
        //�f�o�t�����擾�����{���̕��������ꍇ
        else if (onJumpDebuf)
        {
            //�{���̔�r
            var mag1 = oldJumpDebufMag;
            var mag2 = playerManager.DefJumpMag - mag;
            //����̔{�����O��̔{���������Ă���i���l�I�ɂ͏��������j�Ȃ�
            if (mag1 > mag2)
            {
                jumpDebufMag = mag2;
                //�t�^�����\��̌��ʎ��Ԃ��O��̕t�^���Ԃ������Ă���Ȃ�
                if (maxJumpDebufTime < time)
                {
                    //���s���Ԃ̃��Z�b�g
                    currentJumpDebufTime = 0.0f;
                    maxJumpDebufTime = time;
                }
            }
            //������Ă���Ȃ���ɉ������Ȃ�
            else jumpDebufMag = mag1;
        }

    }
    //���Ԍv���i�W�����v�p�j
    private void CountJumpDebuf()
    {
        currentJumpDebufTime += 0.01f;
        if (currentJumpDebufTime == maxJumpDebufTime * 0.8 && !flashingJumpIcon)
        {
            StartCoroutine(FlashingMoveDebuf());
            flashingJumpIcon = true;
        }
        if (currentJumpDebufTime >= maxJumpDebufTime)
            EndJumpDebuf();
    }

    //�_�ŏ���
    private IEnumerator FlashigJumpIcon()
    {
        while (currentJumpDebufTime <= maxJumpDebufTime)
        {
            yield return new WaitForSeconds(0.1f);
            jumpDebufIcon.enabled = false;
            yield return new WaitForSeconds(0.1f);
            jumpDebufIcon.enabled = true;
        }
    }

    //�W�����v�f�o�t�̏I��
    private void EndJumpDebuf()
    {
        flashingJumpIcon = false;
        onJumpDebuf = false;
        jumpDebufIcon.enabled = false;
        jumpDebufMag = playerManager.DefJumpMag;
        currentJumpDebufTime = 0.0f;
        oldJumpDebufMag = 0.0f;
        maxJumpDebufTime = 0;
        if(!onJumpDebuf && !onMoveDebuf)
            playerC.DeleteDebufEffect();
    }
    #endregion
}
