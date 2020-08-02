using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelLine : MonoBehaviour
{
    private Vector2 start;
    private Vector2 end;
    public Line line;
    private SpriteRenderer sRender;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void config(Vector2 _start,Vector2 _end)
    {
        //LineRenderer lr = GetComponent<LineRenderer>();
        //lr.startColor = Color.grey;
        //lr.endColor = Color.grey;
        //lr.shadowBias = 0.0f;
        //lr.startWidth = 0.2f;
        //lr.endWidth = 0.2f;
        //lr.positionCount = 2;
        //lr.SetPosition(0, start);
        //lr.SetPosition(1, end);
        transform.position = _start;
        float lineStepDistance = 1;
        sRender = GetComponent<SpriteRenderer>();
        if(_start.x == _end.x)
        {
            sRender.size = new Vector2(sRender.size.y,lineStepDistance);
            if (_start.y > _end.y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y- lineStepDistance/2.0f,0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + lineStepDistance / 2.0f, 0);
            }

        }else if(_start.y == _end.y)
        {
            sRender.size = new Vector2(lineStepDistance,sRender.size.x);
            if (_start.x > _end.x)
            {
                transform.position = new Vector3(transform.position.x - lineStepDistance / 2.0f, transform.position.y, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + lineStepDistance / 2.0f, transform.position.y, 0);
            }
        }
        sRender.color = new Color(sRender.color.r, sRender.color.g, sRender.color.b, 0);
    }

    public void ChangeColor(Color _color)
    {
        sRender.color = _color;
    }

    public Color CurrColor()
    {
        return sRender.color;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
