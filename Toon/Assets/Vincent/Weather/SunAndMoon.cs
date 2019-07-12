using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunAndMoon : MonoBehaviour {

    public int _hour;
    public int _minute;

    public float _speed;

    public GameObject _Sun;
    public GameObject _Moon;

    public Text hour;
    public Text minute;

    public ParticleSystem _Pstars;
    public ParticleSystem _Pcloud;

    public Gradient _DayNightSkyColor;
    public Gradient _DayNightHorizonColor;
    public Gradient _FogColor;
    public Gradient _CloudColor;

    private const float MINUTE = 1;
    private const float HOUR = 60 * MINUTE;
    private const float DAY = 24 * HOUR;

    public Material cloudMat;

    public Light _LMoon;

    void Start () {
        _hour = 6;
        _minute = 0;
        hour.text = _hour.ToString();
        minute.text = _minute.ToString();
        _LMoon = _Moon.GetComponent<Light>();
        StartCoroutine(e_timecycle());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator e_timecycle() {
        do
        {
            f_TimeCycle();
            yield return new WaitForSecondsRealtime(1f);
        } while (true);
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

        //Night
        if ((_hour >= 17 && _hour < 19) && RenderSettings.sun.intensity > 0) {
            RenderSettings.sun.intensity -= 0.5f * Time.fixedDeltaTime;
            _LMoon.intensity += 0.5f * Time.fixedDeltaTime;
            if (_LMoon.intensity > 1)
            {
                _LMoon.intensity = 1;
            }
        }

        //Morning
        if((_hour >=5 && _hour < 8) && RenderSettings.sun.intensity <1)
        {
            RenderSettings.sun.intensity += 1f * Time.fixedDeltaTime;
            _LMoon.intensity -= 0.5f * Time.fixedDeltaTime;
            if (RenderSettings.sun.intensity > 1) {
                RenderSettings.sun.intensity = 1;
            }

            if (_Pstars.isPlaying == true) {
                _Pstars.Stop();
                _Pcloud.Play();
            }
        }

        hour.text = _hour.ToString();
        minute.text = _minute.ToString();

        float t_dot = (_hour * 60 + _minute) / DAY;

        RenderSettings.skybox.SetColor("_SkyTint", _DayNightSkyColor.Evaluate(t_dot));
        RenderSettings.skybox.SetColor("_GroundColor", _DayNightSkyColor.Evaluate(t_dot));
        cloudMat.SetColor("_Sky", _CloudColor.Evaluate(t_dot));
        cloudMat.SetColor("_Ground", _DayNightHorizonColor.Evaluate(t_dot));
        RenderSettings.fogColor = _FogColor.Evaluate(t_dot);

        //Debug.Log("Hour :" + _hour);
        //Debug.Log("Minute :" + _minute);
    }

}
