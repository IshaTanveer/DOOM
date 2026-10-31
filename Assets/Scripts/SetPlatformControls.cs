using UnityEngine;
using UnityEngine.InputSystem;

public class SetPlatformControls : MonoBehaviour
{
    [SerializeField] InputActionReference lookAction;
    [SerializeField] GameObject MobileControls;
    [SerializeField] bool ForceMobile;
    void Awake()
    {
        bool mobile = ForceMobile || Application.isMobilePlatform;
        MobileControls.SetActive(mobile);
        lookAction.action.bindingMask = InputBinding.MaskByGroup(mobile ? "Gamepad" : "Keyboard&Mouse");
    }
}
