using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;

public class AspectFit : MonoBehaviour
{
    public enum FitMode
    {
        Width,
        Height
    }
    [SerializeField] private float widthFit;
    [SerializeField] private float heightFit;
    public FitMode fitMode;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        ChangeScreen();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var widthHalf = widthFit / 2;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * widthHalf);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.right * widthHalf);
        
        Gizmos.color = Color.blue;
        var heightHalf = heightFit / 2;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * heightHalf);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * heightHalf);
    }

    [Button("Change screen")]
    private void ChangeScreen()
    {
        switch (fitMode)
        {
            case FitMode.Width:
                Camera.main.orthographicSize = ScreenFitter.WidthOrtoPortrait(widthFit, Camera.main);
                break;
            case FitMode.Height:
                Camera.main.orthographicSize = ScreenFitter.HeightOrtoPortrait(heightFit);
                break;
        }
    }
}

public static class ScreenFitter
{
    // WIDTH
    // Width = height * aspectRatio
    // Portrait
    public static float WidthOrtoPortrait(float width, Camera camera)
    {    
        Vector2 screen = new Vector2((float)camera.pixelWidth,(float)camera.pixelHeight);

        return WidthOrtoPortrait(width, screen);
    }

    // Portrait
    public static float WidthOrtoPortrait(float width, Resolution resolution)
    {
        var aspectRatio = 0f;

        if (resolution.width != 0)
            aspectRatio = (float)resolution.height / (float)resolution.width;

        return WidthOrtoPortrait(width, aspectRatio);
    }

    // Portrait
    public static float WidthOrtoPortrait(float width, Vector2 screen)
    {
        var aspectRatio = 0f;

        if (screen.x!= 0)
            aspectRatio = screen.y / screen.x;

        return WidthOrtoPortrait(width, aspectRatio);
    }

    // Portrait
    public static float WidthOrtoPortrait(float width, float aspectRatio)
    {
        return width * aspectRatio * 0.5f;
    }

    
    // HEIGHT
    // Height = 2f * orhtoSize
    public static float HeightOrtoPortrait(float height)
    {
        return height/2;
    }
}
