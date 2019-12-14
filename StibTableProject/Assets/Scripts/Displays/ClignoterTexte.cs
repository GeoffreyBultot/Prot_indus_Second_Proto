using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClignoterTexte : MonoBehaviour {
    public float TextBlinkTime = 1f;

    private Color _textColorON;
    private Color _textColorOFF;
    private bool _fadingIn;
    private float _timer;
    private float _tmpDeltaTime;
    // Use this for initialization
    void Start () {
        _textColorON = Color.white;
        _textColorOFF = new Color(_textColorON.r, _textColorON.g, _textColorON.b, 0);

        _fadingIn = false;
        _timer = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        _tmpDeltaTime = (Time.time - _timer) / TextBlinkTime;
        if (_fadingIn)
        {
            GetComponent<Text>().color = Color.Lerp(_textColorOFF, _textColorON, _tmpDeltaTime);
        }
        else
        {
            GetComponent<Text>().color = Color.Lerp(_textColorON, _textColorOFF, _tmpDeltaTime);
        }

        if (_tmpDeltaTime > 1)
        {
            _fadingIn = !_fadingIn;
            _timer = Time.time;
        }
    }
}
