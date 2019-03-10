using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAndMoon : MonoBehaviour {

    public int _hour;
    public int _minute;

    public float _speed;

    public GameObject _Sun;
    public GameObject _Moon;

    public Gradient _DayNightSkyColor;
    public Gradient _DayNightHorizonColor;
    public Gradient _FogColor;

    private const float MINUTE = 1;
    private const float HOUR = 60 * MINUTE;
    private const float DAY = 24 * HOUR;

	void Start () {
        _hour = 6;
        _minute = 0;
        InvokeRepeating("f_TimeCycle",0,0.05f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void f_TimeCycle() {
        _minute++;

        if (_minute == 60) {
            _minute = 0;
            _hour++;
        }

        if (_hour == 24) {
            _hour = 0;
        }

        _Sun.transform.rotation *= Quaternion.Euler(_speed * Time.fixedDeltaTime, 0, 0);
        _Moon.transform.rotation *= Quaternion.Euler(-_speed * Time.fixedDeltaTime, 0, 0);

        if ((_hour >= 17 && _hour < 19) && RenderSettings.sun.intensity > 0) {
            RenderSettings.sun.intensity -= 0.5f * Time.fixedDeltaTime;
        }

        if((_hour >=5 && _hour < 7) && RenderSettings.sun.intensity <1)
        {
            RenderSettings.sun.intensity += 1f * Time.fixedDeltaTime;
            if (RenderSettings.sun.intensity > 1) {
                RenderSettings.sun.intensity = 1;
            }
        }

        float t_dot = (_hour * 60 + _minute) / DAY;

        RenderSettings.skybox.SetColor("_SkyTint", _DayNightSkyColor.Evaluate(t_dot));
        RenderSettings.skybox.SetColor("_GroundColor", _DayNightSkyColor.Evaluate(t_dot));
        RenderSettings.fogColor = _FogColor.Evaluate(t_dot);

        Debug.Log("Location Color :" + t_dot * 100);
        Debug.Log("Hour :" + _hour);
        Debug.Log("Minute :" + _minute);
    }

}
