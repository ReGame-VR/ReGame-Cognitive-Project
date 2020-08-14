using System.Collections;
using System.Collections.Generic;

public static class ControllerHaptics
{
    public static void ActivateHaptics(float amplitude, float duration, bool isLeftController) 
    {
        XRHaptics(amplitude, duration, isLeftController);
    }

    public static void XRHaptics(float amplitude, float duration, bool isLeftController)
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        if (isLeftController) {

            UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, devices);
        }
        else {

            UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, devices);
        }

        foreach (var device in devices) {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities)) {
                if (capabilities.supportsImpulse) {
                    uint channel = 0;

                    device.SendHapticImpulse(channel, amplitude, duration);
                }
            }
        }
    }

    public static void OVRHaptics(float amplitude, float frequency, bool isLeftController)
    {
        if (isLeftController)
        {
            OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.LTouch);
        }
        else
        {
            OVRInput.SetControllerVibration(frequency, amplitude, OVRInput.Controller.RTouch);
        }
    }

    public static void ForceQuitOVRHaptics()
    {
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
    }
}
