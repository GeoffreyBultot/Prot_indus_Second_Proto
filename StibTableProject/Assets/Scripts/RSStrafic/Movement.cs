using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public GameObject text;
    public GameObject startPosition;
    public GameObject endPosition;
    public int speed;

    RectTransform _textTransform;

    //Start and End Position

    private float start;
    private float end;

    public delegate void MyEventHandler();
    public MyEventHandler finDeDefilement;
    // Start is called before the first frame update
    void Start()
    {


        _textTransform = text.gameObject.GetComponent<RectTransform>();
        float size = LayoutUtility.GetPreferredWidth(_textTransform);
        start = startPosition.GetComponent<RectTransform>().position.x;
        end = endPosition.GetComponent<RectTransform>().position.x - size;
        //Debug.Log(LayoutUtility.GetPreferredWidth(_textTransform) + " vs " + _textTransform.rect.width);
        _textTransform.position = new Vector3(start, _textTransform.position.y, _textTransform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (_textTransform.localPosition.x >= end)
        {
            _textTransform.localPosition = _textTransform.localPosition + new Vector3(-speed * Time.deltaTime, 0, 0);

        }
        else
        {
            _textTransform.position = new Vector3(start, _textTransform.position.y, _textTransform.position.z);
            onFinDeDefilement();
        }
    }

    private void onFinDeDefilement()
    {
        if (finDeDefilement != null)
        {
            finDeDefilement();
        }
    }
}
