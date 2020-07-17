using System.Collections.Generic;

public static class ControllerHaptics
{
    public static void ActivateHaptics(float amplitude, float duration, bool isLeftController) {

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
}
