using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Dawn
{
    public class DawnController : MonoBehaviour
    {
        private FieldInfo _defaultKeyLightIntensityField;
        private float _defaultKeyLightIntensity;
        private LightMoodController _lightMoodController;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            _lightMoodController = FindObjectOfType<LightMoodController>();

            _defaultKeyLightIntensityField = typeof (LightMoodController).GetField("defaultKeyLightIntensity",
                BindingFlags.NonPublic | BindingFlags.Instance);

            _defaultKeyLightIntensity = LightIntensity; // TODO too early call, call later...
            // Debug.Log("Default _defaultKeyLightIntensity: " + _defaultKeyLightIntensity);
            StartCoroutine(UpdateTime());
        }

        private float LightIntensity
        {
            get { return (float) _defaultKeyLightIntensityField.GetValue(_lightMoodController); }
            set { _defaultKeyLightIntensityField.SetValue(_lightMoodController, value); }
        }

        private void OnDestroy()
        {
            LightIntensity = _defaultKeyLightIntensity;
            _lightMoodController.keyLight.intensity = 1.2f;//_defaultKeyLightIntensity;
        }

        private IEnumerator UpdateTime()
        {
            for (;;)
            {
                float time = ParkInfo.ParkTime;

                const int dayDuration = 300;

                var dayA = (((int) time/dayDuration)*dayDuration);
                var dayB = dayA + dayDuration;
                var night = Mathf.Lerp(dayA, dayB, 0.5f);
                var val = time < night ? 1f - (time - dayA)/(night - dayA) : (time - night)/(dayB - night);

                LightIntensity = Mathf.Lerp(0.1f, 1.2f/*_defaultKeyLightIntensity*/, val);

                //Debug.Log("Default _defaultKeyLightIntensity: " + _defaultKeyLightIntensity);
                //Debug.Log("Perc: " + val + " Targ: " + LightIntensity + " Cur: " + _lightMoodController.keyLight.intensity);

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}