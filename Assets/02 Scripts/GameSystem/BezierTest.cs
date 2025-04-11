using UnityEditor;
using UnityEngine;

//베지어 곡선 테스트를 위한 코드

public class BezierTest : MonoBehaviour
{
    public GameObject GameObject;
    [Range(0f, 1f)]
    public float value;
    public Vector3 P1;
    public Vector3 P2;
    public Vector3 P3;
    public Vector3 P4;

    void Update()
    {
        transform.position = BezierUtils.Bezier(P1, P2, P3, P4, value);
    }

}
/*[CanEditMultipleObjects]
[CustomEditor(typeof(BezierTest))]
public class BezierTest2 : Editor
{
    void OnSceneGUI()
    {
        BezierTest Generator = (BezierTest)target;
        Generator.P1 = Handles.PositionHandle(Generator.P1, Quaternion.identity);
        Generator.P2 = Handles.PositionHandle(Generator.P2, Quaternion.identity);
        Generator.P3 = Handles.PositionHandle(Generator.P3, Quaternion.identity);
        Generator.P4 = Handles.PositionHandle(Generator.P4, Quaternion.identity);

        Handles.DrawLine(Generator.P1, Generator.P2);
        Handles.DrawLine(Generator.P3, Generator.P4);

        Handles.DrawBezier(Generator.P1, Generator.P4, Generator.P2, Generator.P3, Color.white, null, 2f);

    }
}*/