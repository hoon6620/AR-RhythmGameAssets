using UnityEngine;

public class Utils
{
    public static Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
                -Camera.main.transform.position.z));
            return result;
        }
    }

    public static Vector3 FullPosToPanel(Vector3 pos)
    {
        return (pos * 0.8f) + new Vector3(-4.1f, 2.3f, 0);
    }

    public static Vector3 PanelPosToFull(Vector3 pos)
    {
        return (pos + new Vector3(4.1f, -2.3f, 0)) * 1.25f;
    }
}
