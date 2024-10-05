using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Tableshowdata : MonoBehaviour
{
   [SerializeField]
   List<GameObject> Tabitem = new List<GameObject>();
    float lampval=2f;
    [SerializeField]
    GameObject Lamp;
	[SerializeField]
    GameObject light;
   int curindex=0;
	[SerializeField]
	GameObject graphcube;
	public List<List<float>> datad = new List<List<float>>();
   public void IncrementList(float dist,float res)
   {
		  if(curindex<=8)
		  {		
		  if( light.activeSelf)
				 {
				Tabitem[curindex].SetActive(true);
				List<float> innerList1 = new List<float>();
				innerList1.Add(curindex+1);
				innerList1.Add(res);
				innerList1.Add(dist);
				innerList1.Add(dist*dist);
				innerList1.Add(1/(dist*dist));
				Tabitem[curindex].GetComponent<TMP_Text>().text=(int)innerList1[0]+"                  "+innerList1[1]+"                  "+innerList1[2]+"                  "+innerList1[3]+"                  "+innerList1[4].ToString("F4");
				datad.Add(innerList1);
				curindex++;

				}

				if(curindex>8)
				{
					if(!graphcube.activeSelf)
					{
					graphcube.SetActive(true);
					}
				}
		  }


}
}


