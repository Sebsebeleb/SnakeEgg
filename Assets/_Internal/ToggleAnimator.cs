using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Internal
{
    public class ToggleAnimator : MonoBehaviour
    {
        private float f;

        private Toggle toggle;

        public Image Icon, Circle;

        public Color backgroundColor, white, dark, transparent;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
        }

        private void Update()
        {
            if (toggle.isOn)
            {
                f += Time.deltaTime * 5;
            }
            else
            {
                f -= Time.deltaTime * 5;
            }

            f = Mathf.Clamp01(f);

            Circle.color = Color.Lerp(transparent, dark, f);
            Icon.color = Color.Lerp(white, backgroundColor, f);
            Icon.transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, 0, 90), f);
        }
    }
}