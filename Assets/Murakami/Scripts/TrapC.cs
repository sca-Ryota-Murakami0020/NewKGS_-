using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapC : MonoBehaviour
{
    public enum DebufKind
    {
        Null,
        DefMove,
        DefJump,
    }
    [Header("�f�o�t�̎��"),SerializeField] 
    private DebufKind debufKind = DebufKind.Null;

    [Header("�f�o�t�̃��x��"),SerializeField]
    private float debufMag;

    [Header("�f�o�t�̕t�^����"),SerializeField]
    private int debufTime;

    //�v���p�e�B
    public DebufKind DebufKinds
    {
        get { return this.debufKind;}
    }

    public float DebufMag
    {
        get { return this.debufMag;}
    }

    public int DebufTime
    {
        get { return this.debufTime;}
    }

}
