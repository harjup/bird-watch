using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFlash : Singleton<ScreenFlash>
{
    private Image _image;

    void Start()
    {
        _image = GetComponent<Image>();
    }

    public IEnumerator Flash()
    {
        var inital = new Color(
            _image.color.r, 
            _image.color.g, 
            _image.color.b, 
            .5f);

        var target = new Color(
            _image.color.r,
            _image.color.g,
            _image.color.b,
            0f);


        _image.color = inital;


        yield return new WaitForSeconds(.1f);

        while (_image.color.a >= 0.05f)
        {
            // Lerp the colour of the texture between itself and transparent.
            _image.color = Color.Lerp(_image.color, target, 8f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _image.color = target; // Color.clear;
    }

}
