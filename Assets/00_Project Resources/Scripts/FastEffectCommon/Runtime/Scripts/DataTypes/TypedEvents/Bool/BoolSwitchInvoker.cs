using UnityEngine;
using UnityEngine.Events;
using Com.FastEffect.DataTypes;

public class BoolSwitchInvoker : MonoBehaviour
{
    [SerializeField]
    private BoolReference m_storedValue = new BoolReference();
    [SerializeField]
    private UnityEvent m_onTrue = new UnityEvent();
    [SerializeField]
    private UnityEvent m_onFalse = new UnityEvent();

    public void InvokeValue()
    {
        if(m_storedValue.Value)
        {
            m_onTrue.Invoke();
        }
        else
        {
            m_onFalse.Invoke();
        }
    }

    public void InvokeValue(bool value)
    {
        if(value)
        {
            m_onTrue.Invoke();
        }
        else
        {
            m_onFalse.Invoke();
        }
    }
}
