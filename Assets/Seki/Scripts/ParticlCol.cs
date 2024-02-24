using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlCol : MonoBehaviour
{
    [SerializeField] AirController airplane;
   
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "airplane")
        {
            //airplane.HITCOUNT++;
            Debug.Log("“–‚½‚Á‚½");
            this.gameObject.SetActive(false);
        }
    }
}
