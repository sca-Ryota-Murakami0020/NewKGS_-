using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MurakamiMiddleRezaltoC : MonoBehaviour
{
    [SerializeField]
    private Animator ani;
    [SerializeField]
    private GameManager gameManager;

    [Header("����"),SerializeField]
    private Sprite[] numberImage;
    [Header("�^�C���\�LImage"),SerializeField]
    private Image[] timeImage;
    [Header("�^�C���{�[�i�X�\�LImage"),SerializeField]
    private Image[] timeBounusImage;

    [Header("�c�@�\�LImage"),SerializeField]
    private Image[] remainImage;
    [Header("�c�@�{�[�i�X�\�LImage"),SerializeField]
    private Image[] remainBounusImage;
    
    [Header("��Փx�\�LImage"),SerializeField]
    private Image[] difImage;
    [Header("��Փx�{�[�i�X�\�LImage"),SerializeField]
    private Image[] difBounusImage;

    [Header("�g�[�^���X�R�A�\��Image"),SerializeField]
    private Image[] totalScoreImage;

    // Start is called before the first frame update
    void Start()
    {
        //������
        for(int i = 0; i < timeImage.Length; i++) ResetImage(timeImage[i]);

        for(int i = 0; i < timeBounusImage.Length; i++) ResetImage(timeBounusImage[i]);

        for(int i = 0; i < remainImage.Length; i++) ResetImage(remainImage[i]);

        for(int i = 0; i < remainBounusImage.Length; i++) ResetImage(remainBounusImage[i]);

        for(int i = 0; i < difImage.Length; i++) ResetImage(difImage[i]);

        for(int i = 0; i < difBounusImage.Length; i++) ResetImage(difBounusImage[i]);

        for(int i = 0; i < totalScoreImage.Length; i++) ResetImage(totalScoreImage[i]);
    }

    //Image�̏������֐�
    public void ResetImage(Image image)
    {
        image.sprite = numberImage[0];
        image.enabled = false;
    }

    public void StartMoveBack()
    {
        ani.SetTrigger("ShowRezalto");
    }

    //���U���g�p
    public void StartShowRezalto()
    {
        //�v�Z�J�n
        StartCoroutine(CountRezalt());
    }


    //���ԃ��U���g
    private IEnumerator CountRezalt()
    {
        //���ԕ\�L�p�ϐ�
        int time = gameManager.StageCount;
        //���ԃ{�[�i�X
        int timeScore = gameManager.StageCount * 10;
        //�c�@
        int remain = gameManager.CurrentRemain;
        //�c�@�{�[�i�X
        int remainScore = gameManager.CurrentRemain * 1000;
        //��Փx
        int dif = gameManager.DifLevel;
        //��Փx�{�[�i�X
        int difScore = dif * 1000;
        //�{�[�i�X���Z��̃g�[�^���X�R�A
        int stageTotalScore = gameManager.GetScore + timeScore + remainScore + difScore;

        //�^�C���\�L
        int count = 0;
        int localScore = time;
        while (count <= time)
        {
            for(int i = 0; i < timeImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if(count >= Mathf.Pow(10, i)) timeImage[i].enabled = true;
                timeImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
            }
            count++;
        }
        yield return new WaitForSeconds(1.2f);

        //�^�C���{�[�i�X�\�L
        count = 0;
        localScore = timeScore;
        while(count <= timeScore)
        {
            for (int i = 0; i < timeBounusImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if (count >= Mathf.Pow(10, i)) timeBounusImage[i].enabled = true;
                timeBounusImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
            }
            count++;
        }
        yield return new WaitForSeconds(1.2f);

        //�c�@�\�L
        count = 0;
        localScore = remainScore;
        while (count <= remainScore)
        {
            for (int i = 0; i < remainImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if (count >= Mathf.Pow(10, i)) remainImage[i].enabled = true;
                remainImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
                count++;
            }
        }
        yield return new WaitForSeconds(1.2f);

        //�c�@�{�[�i�X�\�L
        count = 0;
        localScore = remainScore;
        while (count <= remainScore)
        {
            for (int i = 0; i < remainBounusImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if (count >= Mathf.Pow(10, i)) remainBounusImage[i].enabled = true;
                remainBounusImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
            }
            count++;
        }
        yield return new WaitForSeconds(1.2f);

        //��Փx
        count = 0;
        localScore = dif;
        while (count <= dif)
        {
            for (int i = 0; i < difImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if (count >= Mathf.Pow(10, i)) difImage[i].enabled = true;
                difImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
            }
            count++;
        }
        yield return new WaitForSeconds(1.2f);

        //��Փx�{�[�i�X
        count = 0;
        localScore = difScore;
        while (count <= difScore)
        {
            for (int i = 0; i < difBounusImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if (count >= Mathf.Pow(10, i)) difBounusImage[i].enabled = true;
                difBounusImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
            }
            count++;
        }
        yield return new WaitForSeconds(1.2f);

        //�g�[�^���X�R�A
        count = 0;
        localScore = stageTotalScore;
        while (count <= stageTotalScore)
        {
            for (int i = 0; i < totalScoreImage.Length; i++)
            {
                int num = localScore % 10;
                //�ݏ悵�ĕ\������Image��I������B
                if (count >= Mathf.Pow(10, i)) totalScoreImage[i].enabled = true;
                totalScoreImage[i].sprite = numberImage[num];
                localScore /= 10;
                yield return new WaitForSeconds(0.001f);
            }
            count++;
        }
        yield return new WaitForSeconds(1.2f);
    }
}
