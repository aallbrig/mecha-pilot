using System;
using Cinemachine;
using UnityEngine;

namespace Cameras
{
    // Responsible for detecting if landscape or portrait
    // and then orienting the camera based on that
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [ExecuteInEditMode]
    public class CameraAdjustsToOrientation : MonoBehaviour
    {
        public Vector3 portraitFollowOffset;
        public Vector3 landscapeFollowOffset;
        private ScreenOrientation _currentScreenOrientation;
        private bool _isMobile;
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineTransposer _virtualCameraTransposer;
        private void Start()
        {
            _virtualCamera ??= GetComponent<CinemachineVirtualCamera>();
            _virtualCamera ??= GetComponentInChildren<CinemachineVirtualCamera>();
            var parent = transform.parent;
            _virtualCamera ??= parent.GetComponent<CinemachineVirtualCamera>();
            _virtualCamera ??= parent.GetComponentInChildren<CinemachineVirtualCamera>();
            if (_virtualCamera == null) throw new NullReferenceException("Cinemachine virtual camera could not be found");
            _virtualCameraTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _isMobile = Application.isMobilePlatform;
            SyncCameraState();
        }
        private void Update()
        {
            if (Screen.orientation != _currentScreenOrientation) SyncCameraState();
        }
        private void SyncCameraState()
        {
            _currentScreenOrientation = Screen.orientation;
            if (_currentScreenOrientation == ScreenOrientation.Portrait ||
                _currentScreenOrientation == ScreenOrientation.PortraitUpsideDown)
                _virtualCameraTransposer.m_FollowOffset = portraitFollowOffset;
            else if (_currentScreenOrientation == ScreenOrientation.LandscapeLeft ||
                     _currentScreenOrientation == ScreenOrientation.LandscapeRight)
                _virtualCameraTransposer.m_FollowOffset = landscapeFollowOffset;
        }
    }
}
