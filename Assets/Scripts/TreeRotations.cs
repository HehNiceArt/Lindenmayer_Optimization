using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Andrei Quirante
public class TreeRotations : MonoBehaviour
{
    /// <summary> 3x3 RotationUp
    ///   Column          0             1          2     Row 
    ///             |  Cos(angle) ,  Sin(angle) ,  0  |   0
    /// Ru(angle) = | -Sin(angle) ,  Cos(angle) ,  0  |   1
    ///             |     0       ,     0       ,  1  |   2
    /// </summary>
    /// <param name="angle">Rotation 3x3</param>
    /// <returns>Gets the 3x3 rotation</returns>
    public Matrix4x4 RotationUp(float angle)
    {
        Matrix4x4 _rotationUp = Matrix4x4.zero;

        //First row
        _rotationUp[0, 0] = Mathf.Cos(angle);
        _rotationUp[0, 1] = Mathf.Sin(angle);
        _rotationUp[0, 2] = 0f;
        _rotationUp[0, 3] = 0f;
        //Second row
        _rotationUp[1, 0] = -Mathf.Sin(angle);
        _rotationUp[1, 1] = Mathf.Cos(angle);
        _rotationUp[1, 2] = 0f;
        _rotationUp[1, 3] = 0f;
        //Third row
        _rotationUp[2, 0] = 0f;
        _rotationUp[2, 1] = 0f;
        _rotationUp[2, 2] = 1;
        _rotationUp[2, 3] = 0f;
        // Fourth row
        _rotationUp[3, 0] = 0f;
        _rotationUp[3, 1] = 0f;
        _rotationUp[3, 2] = 0f;
        _rotationUp[3, 3] = 1f;
        return _rotationUp;
    }

    /// <summary> 3x3 RotationPitch
    ///   Column        0        1             2          Row 
    ///             |   1   ,    0       ,     0       |   0
    /// Rl(angle) = |   0   , Cos(angle) , -Sin(angle) |   1
    ///             |   0   , Sin(angle) ,  Cos(angle) |   2
    /// </summary>
    /// <param name="angle">Parameter value to pass</param>
    /// <returns></returns>
    public Matrix4x4 RotationPitch(float angle)
    {
        Matrix4x4 _rotationPitch = Matrix4x4.zero;

        //First row
        _rotationPitch[0, 0] = 1f;
        _rotationPitch[0, 1] = 0f;
        _rotationPitch[0, 2] = 0f;
        _rotationPitch[0, 3] = 0f;
        //Second row
        _rotationPitch[1, 0] = 0f;
        _rotationPitch[1, 1] = Mathf.Cos(angle);
        _rotationPitch[1, 2] = -Mathf.Sin(angle);
        _rotationPitch[1, 3] = 0f;
        //Third row
        _rotationPitch[2, 0] = 0f;
        _rotationPitch[2, 1] = Mathf.Sin(angle);
        _rotationPitch[2, 2] = Mathf.Cos(angle);
        _rotationPitch[2, 3] = 0f;
        // Fourth row
        _rotationPitch[3, 0] = 0f;
        _rotationPitch[3, 1] = 0f;
        _rotationPitch[3, 2] = 0f;
        _rotationPitch[3, 3] = 1f;

        return _rotationPitch;
    }
    /// <summary>
    /// 3x3 RotationRoll
    ///   Column         0          1        2           Row
    ///             | Cos(angle) ,  0  , -Sin(angle) |    0
    /// Rh(angle) = |    0       ,  1  ,     0       |    1
    ///             | Sin(angle) ,  0  ,  Cos(angle) |    2
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Matrix4x4 RotationHeading(float angle)
    {
        Matrix4x4 _rotationHeading = Matrix4x4.zero;

        //First row
        _rotationHeading[0, 0] = Mathf.Cos(angle);
        _rotationHeading[0, 1] = 0f;
        _rotationHeading[0, 2] = -Mathf.Sin(angle);
        _rotationHeading[0, 3] = 0f;
        //Second row
        _rotationHeading[1, 0] = 0f;
        _rotationHeading[1, 1] = 1f;
        _rotationHeading[1, 2] = 0f;
        _rotationHeading[1, 3] = 0f;
        //Third row
        _rotationHeading[2, 0] = Mathf.Sin(angle);
        _rotationHeading[2, 1] = 0f;
        _rotationHeading[2, 2] = Mathf.Cos(angle);
        _rotationHeading[2, 3] = 0f;
        // Fourth row
        _rotationHeading[3, 0] = 0f;
        _rotationHeading[3, 1] = 0f;
        _rotationHeading[3, 2] = 0f;
        _rotationHeading[3, 3] = 1f;
        return _rotationHeading;
    }
}
//Source: https://www.researchgate.net/profile/Noppadon-Khiripet/publication/268380530_Real-time_3D_Plant_Structure_Modeling_by_L-System_with_Actual_Measurement_Parameters/links/551b9d030cf2fdce8438a1bd/Real-time-3D-Plant-Structure-Modeling-by-L-System-with-Actual-Measurement-Parameters.pdf
