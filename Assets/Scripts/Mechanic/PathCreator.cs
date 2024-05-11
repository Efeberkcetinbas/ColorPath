using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;



public class PathCreator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points=new List<Vector3>();

    public Action<IEnumerable<Vector3>> OnNewPathCreated=delegate{};

    public bool isCreated=false;
    public bool isOrderMe=false;

    private void Awake()
    {
        lineRenderer=GetComponent<LineRenderer>();
    }

    //Cisimlerin uzerine tiklanimca da pointsleri temizle
    //Yeni Levele gecerken points Clear yaparsin
    private void Update()
    {
        if(!isCreated)
        {
            if(isOrderMe)
            {

                if(Input.GetButtonDown("Fire1"))
                    points.Clear();
                
                if(Input.GetButton("Fire1"))
                {
                    Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if(Physics.Raycast(ray,out hitInfo))
                    {
                        if(DistanceToLastPoint(hitInfo.point)>1f)
                        {
                            points.Add(hitInfo.point);
                            lineRenderer.positionCount=points.Count;
                            lineRenderer.SetPositions(points.ToArray());
                        }
                        

                    }
                }

                else if(Input.GetButtonUp("Fire1"))
                {
                    OnNewPathCreated(points);
                    isCreated=true;
                }
            }
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if(!points.Any())
            return Mathf.Infinity;
        return Vector3.Distance(points.Last(), point);
    }
}
