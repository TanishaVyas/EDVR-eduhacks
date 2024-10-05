using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
using XCharts.Runtime;
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
public class updategraph : MonoBehaviour
{   
    public Tableshowdata ts;
    void Start()
        {
          
            AddData();
        }

    public void AddData()
        {   string text = "Success";
            List<float> tandata = new List<float>();
            List<float> tanXaxis  = new List<float>();
            foreach (var ts in ts.datad)
            {
                tandata.Add(ts[1]);
                Debug.Log(ts[1]+"dsa"+tandata[0]);
                tanXaxis.Add(ts[4]);
                Debug.Log(ts[4]+"dsa"+ tanXaxis[0]);
            }

             for(int i = 1; i < tanXaxis.Count; i++)
             {
                    if(tanXaxis[i]<tanXaxis[i-1])
                    {
                        text = "Exp Failed Try Again";
                    }
             }
            //list of dates 
            
            //linechart settings
            var chart = gameObject.GetComponent<LineChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
                chart.Init();
            }
            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = "Light Dependant Resistor Graph "+text;

            chart.EnsureChartComponent<Tooltip>().show = true;
            chart.EnsureChartComponent<Legend>().show = false;

            var xAxis = chart.EnsureChartComponent<XAxis>();
            var yAxis = chart.EnsureChartComponent<YAxis>();
            xAxis.show = true;
            yAxis.show = true;
            xAxis.type = Axis.AxisType.Category;
            yAxis.type = Axis.AxisType.Value;

            xAxis.splitLine.show = false;
            yAxis.splitLine.show = false;

            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;

            chart.RemoveData();
            var lineSeries = chart.AddSerie<Line>("LineSeries");
            lineSeries.lineType = LineType.Smooth;
            lineSeries.lineStyle.color = Color.black;
            lineSeries.symbol.size = 15;
            var textColor = Color.black;
            xAxis.axisLabel.textStyle.color = textColor;
            yAxis.axisLabel.textStyle.color = textColor;
            //adding points
            for (int i = 0; i < tandata.Count; i++)
            {   
                Debug.Log(tanXaxis[i]);
                chart.AddXAxisData(tanXaxis[i]+"");
                chart.AddData("LineSeries", tandata[i]);
            }
        }
}

}