using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DistanceCalc : MonoBehaviour
{   [SerializeField]
    public GameObject light;
    [SerializeField]
    TMP_Text Distance;
    [SerializeField]
    TMP_Text Resistances;
    [SerializeField]
    TMP_Text ResistancesHUD;
    [SerializeField]
    Transform ldr;

    [SerializeField]
   Transform lamp;
    [SerializeField]
   Transform lampobj;
   float originalDistance;
  public float resistance;
  public float dist;
   void Start()
   {
     originalDistance=Vector3.Distance (ldr.position, lamp.position);
   }

    // Update is called once per frame
    void Update()
    {   
        dist=((Vector3.Distance (ldr.position, lamp.position)/originalDistance)*100)-10;
        Distance.text = (int)dist +"cm";
        resistance =  0.0020f*dist*dist - 0.0077f*dist + 0.4517f;
        if(light.activeSelf){
        Resistances.text=Mathf.Round(resistance* 100) / 100.0+"Ω";
        ResistancesHUD.text=Mathf.Round(resistance* 100) / 100.0+"Ω";}
        else{
            Resistances.text="Off";
            ResistancesHUD.text="Off";
        }
    }
}
