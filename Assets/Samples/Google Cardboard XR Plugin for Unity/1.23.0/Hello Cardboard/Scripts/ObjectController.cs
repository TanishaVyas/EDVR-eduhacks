//-----------------------------------------------------------------------
// <copyright file="ObjectController.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;

/// <summary>
/// Controls target objects behaviour.
/// </summary>
public class ObjectController : MonoBehaviour
{
    /// <summary>
    /// The material to use when this object is inactive (not being gazed at).
    /// </summary>
    public GameObject LabelText;

    /// <summary>
    /// The material to use when this object is active (gazed at).
    /// 
    /// </summary
    /// 
    [SerializeField]
    Vector3 location;
    [SerializeField]
    Transform Lamploc;

    [SerializeField]
    Material OnMaterial;
    [SerializeField]
     Material OffMaterial;

    [SerializeField]
    GameObject ToSwitch;
    [SerializeField]
    GameObject colorChange;
    // The objects are about 1 meter in radius, so the min/max target distance are
    // set so that the objects are always within the room (which is about 5 meters
    // across).
    private const float _minObjectDistance = 2.5f;
    private const float _maxObjectDistance = 3.5f;
    private const float _minObjectHeight = 0.5f;
    private const float _maxObjectHeight = 5f;

    [SerializeField]
     Renderer _myRenderer;
    private Vector3 _startingPosition;
    [SerializeField]
    InstructionList li;

    [SerializeField]
    GameObject graphh;
    public enum objectType
    {
        button,
        labobject,
        movable,
        scalebutton,
        chart,
        rightchange,
        leftchange,
        plotgraph

    }

    public objectType type;
    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    /// 

    bool buttonstate = false ;
    public void Start()
    {
        //_startingPosition = transform.parent.localPosition;
        //_myRenderer = colorChange.GetComponent<Renderer>();
        if(type!=objectType.chart)
            LabelText.SetActive(false);
        if(type==objectType.button)
        {
            buttonstate = false ;
            _myRenderer.material.color =Color.red;
            //_myRenderer.material.color= OffMaterial;
            ToSwitch.SetActive(false);
        }

    }

    /// <summary>
    /// Teleports this instance randomly when triggered by a pointer click.
    /// </summary>

    /// <summary>
    /// This method is called by the Main Camera when it starts gazing at this GameObject.
    /// </summary>
    public void OnPointerEnter()
    {
       // SetMaterial(true);
       if(type!=objectType.chart)
        LabelText.SetActive(true);
    else
    gameObject.transform.localScale=new Vector3(3f,3f,1f);
    }

    /// <summary>
    /// This method is called by the Main Camera when it stops gazing at this GameObject.
    /// </summary>
    public void OnPointerExit()
    {
        //SetMaterial(false);
    if(type!=objectType.chart)
       LabelText.SetActive(false);
    else
        gameObject.transform.localScale=new Vector3(1.5f,1.5f,1f);

    }

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerClick()
    {   
        Debug.Log("Pointer clicker");
        if(type==objectType.button)
        {   buttonstate=!buttonstate;
            if(buttonstate)
            {   
                Debug.Log("1");
               // _myRenderer.material= OnMaterial;
               _myRenderer.material.color =Color.green;
                ToSwitch.SetActive(true);
            }
            else{
                Debug.Log("0");
               // _myRenderer.material= OffMaterial;
               _myRenderer.material.color =Color.red;
                ToSwitch.SetActive(false);
            }
        }
        else if(type==objectType.scalebutton)
        {
            Lamploc.position=location;
        }
        else if(type==objectType.rightchange){
            li.NextInstruction();
        }

        else if(type==objectType.leftchange){
            li.PreviousInstruction();
        }
        else if(type==objectType.plotgraph){
            graphh.SetActive(true);
        }

    }

    /// <summary>
    /// Sets this instance's material according to gazedAt status.
    /// </summary>
    ///
    /// <param name="gazedAt">
    /// Value `true` if this object is being gazed at, `false` otherwise.
    /// </param>
}
