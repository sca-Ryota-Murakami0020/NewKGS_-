using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpliteC : MonoBehaviour
{
    [Header("���Z����X�s�[�h"),SerializeField]
    private float addSpeedMag;
    [Header("���Z����W�����v��"),SerializeField]
    private float addJumpMag;
    [Header("�y������d��"),SerializeField]
    private float subGravity;

    public float AddSpeedMag
    {
        get { return this.addSpeedMag;}
        set { this.addSpeedMag = value;}
    }

    public float AddJumpMag
    {
        get { return this.addJumpMag;}
        set { this.addJumpMag = value;}
    }

    public float SubGravity
    {
        get { return this.subGravity;}
        set { this.subGravity = value;}
    }
}
