// GENERATED AUTOMATICALLY FROM 'Assets/00_Project Resources/UniversalControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @UniversalControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @UniversalControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UniversalControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c2921f13-6778-4a34-8451-33545d5d2e2f"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""f76480fb-a8bc-4cad-b24b-126c8583913f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Input"",
                    ""type"": ""Button"",
                    ""id"": ""a6ec2d54-a3f0-44f9-80f7-b130ce812860"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickDrop"",
                    ""type"": ""Button"",
                    ""id"": ""16d699b5-17bc-466c-9e17-1c7c3e7e7d21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickHold"",
                    ""type"": ""Button"",
                    ""id"": ""5098e6ad-d40a-4a33-b4f4-abea95dbb8be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""1878c2bd-f127-4e27-a803-6f8add878763"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""65467fcf-2e89-47c5-b223-78172b36d507"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/hat/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""cb697bab-bd72-40f3-9db9-fe29ec920fc0"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/hat/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b887634b-9b29-4df6-8903-8198d44d3bb7"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/hat/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1a4dd484-8a7b-4fc4-acc2-64d10e662dda"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/hat/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""826ff093-c1c1-4f6f-bf36-ff909e0d8d73"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6e0f458a-22de-4e1f-827a-01d10db382a9"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a2cc38a1-ad0c-44f3-a170-bb822e8ecbfc"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""184a752a-c7fd-46dd-918e-a49291ec0ff9"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""08eab7f5-3c89-4043-bf88-9116bcd02f89"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""25d7636b-cd0c-437a-b7e9-c4b2e128f79b"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1214ebd7-2eae-4648-b0b6-7b3f46d5cacd"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button 1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ccce0eb-7a10-4a5a-b73f-f69d38b4b496"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""062dda79-723d-436a-b967-8654ff5d9f1f"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d853e409-3350-4b6c-81a6-1ad18092d88b"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56affd64-5330-4834-82cb-277013741c49"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""772168c8-6eab-4fe7-b6ec-ac157940a1e8"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/hat/down"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickDrop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00d153d5-59dd-490e-bb5b-c8b7fecc97c9"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button2"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickHold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""8c1d7826-a2ba-40b2-8d5f-6c322cb52fe1"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""738d5a71-8d97-4e98-a6fd-a8871f4a92f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5bf01950-d495-479c-81c1-b4114a835f6d"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bb9cb17-fac9-4453-86d4-813e9683348c"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button14"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Jukebox"",
            ""id"": ""2ee22b7d-1910-4cb6-b7fc-d6c1ba08959b"",
            ""actions"": [
                {
                    ""name"": ""RWD"",
                    ""type"": ""Button"",
                    ""id"": ""13053b9e-0c58-41c0-9293-4a50fab0ba6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FWD"",
                    ""type"": ""Button"",
                    ""id"": ""a79d8094-dd27-4c9d-9546-b675bd0d3889"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""06525411-0880-4a6a-a80c-a74fc5a56f8a"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RWD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2517550d-5511-494d-8e68-8af4c244b6de"",
                    ""path"": ""<HID::Sony Interactive Entertainment DualSense Wireless Controller>/button8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FWD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Input = m_Player.FindAction("Input", throwIfNotFound: true);
        m_Player_QuickDrop = m_Player.FindAction("QuickDrop", throwIfNotFound: true);
        m_Player_QuickHold = m_Player.FindAction("QuickHold", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        // Jukebox
        m_Jukebox = asset.FindActionMap("Jukebox", throwIfNotFound: true);
        m_Jukebox_RWD = m_Jukebox.FindAction("RWD", throwIfNotFound: true);
        m_Jukebox_FWD = m_Jukebox.FindAction("FWD", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Input;
    private readonly InputAction m_Player_QuickDrop;
    private readonly InputAction m_Player_QuickHold;
    public struct PlayerActions
    {
        private @UniversalControls m_Wrapper;
        public PlayerActions(@UniversalControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Input => m_Wrapper.m_Player_Input;
        public InputAction @QuickDrop => m_Wrapper.m_Player_QuickDrop;
        public InputAction @QuickHold => m_Wrapper.m_Player_QuickHold;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Input.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInput;
                @Input.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInput;
                @Input.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInput;
                @QuickDrop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickDrop;
                @QuickDrop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickDrop;
                @QuickDrop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickDrop;
                @QuickHold.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickHold;
                @QuickHold.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickHold;
                @QuickHold.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnQuickHold;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Input.started += instance.OnInput;
                @Input.performed += instance.OnInput;
                @Input.canceled += instance.OnInput;
                @QuickDrop.started += instance.OnQuickDrop;
                @QuickDrop.performed += instance.OnQuickDrop;
                @QuickDrop.canceled += instance.OnQuickDrop;
                @QuickHold.started += instance.OnQuickHold;
                @QuickHold.performed += instance.OnQuickHold;
                @QuickHold.canceled += instance.OnQuickHold;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Pause;
    public struct UIActions
    {
        private @UniversalControls m_Wrapper;
        public UIActions(@UniversalControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Jukebox
    private readonly InputActionMap m_Jukebox;
    private IJukeboxActions m_JukeboxActionsCallbackInterface;
    private readonly InputAction m_Jukebox_RWD;
    private readonly InputAction m_Jukebox_FWD;
    public struct JukeboxActions
    {
        private @UniversalControls m_Wrapper;
        public JukeboxActions(@UniversalControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @RWD => m_Wrapper.m_Jukebox_RWD;
        public InputAction @FWD => m_Wrapper.m_Jukebox_FWD;
        public InputActionMap Get() { return m_Wrapper.m_Jukebox; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(JukeboxActions set) { return set.Get(); }
        public void SetCallbacks(IJukeboxActions instance)
        {
            if (m_Wrapper.m_JukeboxActionsCallbackInterface != null)
            {
                @RWD.started -= m_Wrapper.m_JukeboxActionsCallbackInterface.OnRWD;
                @RWD.performed -= m_Wrapper.m_JukeboxActionsCallbackInterface.OnRWD;
                @RWD.canceled -= m_Wrapper.m_JukeboxActionsCallbackInterface.OnRWD;
                @FWD.started -= m_Wrapper.m_JukeboxActionsCallbackInterface.OnFWD;
                @FWD.performed -= m_Wrapper.m_JukeboxActionsCallbackInterface.OnFWD;
                @FWD.canceled -= m_Wrapper.m_JukeboxActionsCallbackInterface.OnFWD;
            }
            m_Wrapper.m_JukeboxActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RWD.started += instance.OnRWD;
                @RWD.performed += instance.OnRWD;
                @RWD.canceled += instance.OnRWD;
                @FWD.started += instance.OnFWD;
                @FWD.performed += instance.OnFWD;
                @FWD.canceled += instance.OnFWD;
            }
        }
    }
    public JukeboxActions @Jukebox => new JukeboxActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInput(InputAction.CallbackContext context);
        void OnQuickDrop(InputAction.CallbackContext context);
        void OnQuickHold(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IJukeboxActions
    {
        void OnRWD(InputAction.CallbackContext context);
        void OnFWD(InputAction.CallbackContext context);
    }
}
