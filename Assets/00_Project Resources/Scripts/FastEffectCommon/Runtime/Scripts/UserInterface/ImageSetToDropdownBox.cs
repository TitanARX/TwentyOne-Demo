using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.FastEffect.DataTypes;
namespace Com.FastEffect.UI
{

    public class ImageSetToDropdownBox : MonoBehaviour
    {
        public SpriteSetReference m_imageSet;
        private Dropdown m_dropdown;

        void Awake()
        {
            if(TryGetComponent<Dropdown>(out m_dropdown) && m_imageSet.Count > 0)
            {
                m_dropdown.ClearOptions();
                List<Dropdown.OptionData> oList = new List<Dropdown.OptionData>();
                foreach(Sprite sp in m_imageSet)
                {
                    oList.Add(new Dropdown.OptionData(sp.name, sp));
                }
                m_dropdown.AddOptions(oList);
            }
        }
    }
}