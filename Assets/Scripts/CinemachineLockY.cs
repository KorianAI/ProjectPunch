using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineLockY : CinemachineExtension
{
    [Tooltip("Lock the camera's Y position to this value")]
    public float m_YPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Finalize)
        {
            var pos = state.RawPosition;
            pos.y = m_YPosition;
            state.RawPosition = pos;
        }
    }
}
