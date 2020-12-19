using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AspectFit : MonoBehaviour
{
    [SerializeField] private float widthFit;
    
    // Start is called before the first frame update
    private void Start()
    {
        Camera.main.orthographicSize = ScreenFitter.WidthOrtoPortrait(widthFit, Camera.main);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var widthHalf = widthFit / 2;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right*widthHalf);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.right*widthHalf);
    }
}

public static class ScreenFitter
{
    // Portrait
    public static float WidthOrtoPortrait(float width, Camera camera)
    {    
        Vector2 screen = new Vector2((float)camera.pixelHeight,(float)camera.pixelWidth);

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

        if (screen.y != 0)
            aspectRatio = screen.x / screen.y;

        return WidthOrtoPortrait(width, aspectRatio);
    }

    // Portrait
    public static float WidthOrtoPortrait(float width, float aspectRatio)
    {
        return width * aspectRatio * 0.5f;
    }
}
