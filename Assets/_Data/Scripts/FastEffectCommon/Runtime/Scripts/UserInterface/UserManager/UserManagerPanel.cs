namespace Com.FastEffect.UI.UserManagement
{
    using Com.FastEffect.Events;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class UserManagerPanel : MonoBehaviour
    {
        [SerializeField]
        public RectTransform m_userItemListArea;
        [SerializeField]
        public IntSwitchInvoker m_userItemTemplate;

        private Dictionary<int, IntSwitchInvoker> m_userItemList = new Dictionary<int, IntSwitchInvoker>();
        public void AddUserItem(int userAccessorNumber)
        {
            IntSwitchInvoker newUserItem = Instantiate(m_userItemTemplate, m_userItemListArea);
            IntInvoker intVoker = null;
            if (newUserItem.TryGetComponent(out intVoker))
            {
                intVoker.Value = userAccessorNumber;
                intVoker.Invoke();
            }
            else
            {
                newUserItem.Value = userAccessorNumber;
            }

            if (m_userItemList.ContainsKey(userAccessorNumber))
            {
                Destroy(m_userItemList[userAccessorNumber].gameObject);
                m_userItemList[userAccessorNumber] = newUserItem;
            }
            else
            {
                m_userItemList.Add(userAccessorNumber, newUserItem);
            }

        }
        public void RemoveUserItem(int userAccessorNumber)
        {
            if (m_userItemList.ContainsKey(userAccessorNumber))
            {
                Destroy(m_userItemList[userAccessorNumber].gameObject);
                m_userItemList.Remove(userAccessorNumber);
            }
        }
        public GameObject GetUserItemObject(int userAccessorNumber)
        {
            if (m_userItemList.ContainsKey(userAccessorNumber))
            {
                return m_userItemList[userAccessorNumber].gameObject;
            }
            return null;
        }

        void Awake()
        {
            if(m_userItemTemplate == null)
            {
                Debug.LogError("No user item template assigned! deactivating.");
                enabled = false;
            } else if (!m_userItemTemplate.TryGetComponent(typeof(RectTransform),out _))
            {
                Debug.LogError("No RectTransform detected on template object! deactivating.");
                enabled = false;
            }

            if(m_userItemListArea == null)
            {
                Debug.LogWarning("No RectTransform provided for user item list! Defaulting to base object RectTransform!");
                m_userItemListArea = GetComponent<RectTransform>();
            }

        }
    }
}