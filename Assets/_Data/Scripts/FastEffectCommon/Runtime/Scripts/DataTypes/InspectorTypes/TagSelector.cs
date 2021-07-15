using UnityEngine;

namespace Com.FastEffect.DataTypes
{
    public class TagSelector : PropertyAttribute
    {
        public static string NoTag { get{ return "<NoTag>"; } }
        public bool UseDefaultTagFieldDrawer = false;
    }
}