using System;
using System.Collections;
using UnityEngine;
using InControl;

namespace MistRidge
{
    public class KeyboardAndMouseProfile : InControl.UnityInputDeviceProfile
    {
        public KeyboardAndMouseProfile()
        {
            Name = "Keyboard/Mouse";
            Meta = "Keyboard and Mouse profile";

            // This profile only works on desktops.
            SupportedPlatforms = new[]
            {
                "Windows",
                "Mac",
                "Linux"
            };

            Sensitivity = 1.0f;
            LowerDeadZone = 0.0f;
            UpperDeadZone = 1.0f;

            ButtonMappings = new[]
            {
                new InputControlMapping
                {
                    Handle = "Submit",
                    Target = InputControlType.Action1,
                    Source = KeyCodeButton( KeyCode.Z, KeyCode.Return )
                },
                new InputControlMapping
                {
                    Handle = "Cancel",
                    Target = InputControlType.Action2,
                    Source = KeyCodeButton( KeyCode.X )
                },
                new InputControlMapping
                {
                    Handle = "UseItem",
                    Target = InputControlType.LeftTrigger,
                    Source = KeyCodeButton( KeyCode.C )
                },
                new InputControlMapping
                {
                    Handle = "Jump",
                    Target = InputControlType.Action1,
                    Source = KeyCodeButton( KeyCode.Space )
                },
                new InputControlMapping
                {
                    Handle = "Pause",
                    Target = InputControlType.Pause,
                    Source = KeyCodeButton( KeyCode.P )
                },
            };

            AnalogMappings = new[]
            {
                new InputControlMapping
                {
                    Handle = "Use",
                    Target = InputControlType.LeftTrigger,
                    Source = KeyCodeButton( KeyCode.C )
                },
                new InputControlMapping
                {
                    Handle = "Move X",
                    Target = InputControlType.LeftStickX,
                    // KeyCodeAxis splits the two KeyCodes over an axis. The first is negative, the second positive.
                    Source = KeyCodeAxis( KeyCode.A, KeyCode.D )
                },
                new InputControlMapping
                {
                    Handle = "Move Y",
                    Target = InputControlType.LeftStickY,
                    // Notes that up is positive in Unity, therefore the order of KeyCodes is down, up.
                    Source = KeyCodeAxis( KeyCode.S, KeyCode.W )
                },
                new InputControlMapping {
                    Handle = "Move X Alternate",
                    Target = InputControlType.LeftStickX,
                    Source = KeyCodeAxis( KeyCode.LeftArrow, KeyCode.RightArrow )
                },
                new InputControlMapping {
                    Handle = "Move Y Alternate",
                    Target = InputControlType.LeftStickY,
                    Source = KeyCodeAxis( KeyCode.DownArrow, KeyCode.UpArrow )
                },
            };
        }
    }
}
