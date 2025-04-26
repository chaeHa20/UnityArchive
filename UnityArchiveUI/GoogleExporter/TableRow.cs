using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnityPMSManager
{
    public class TableRow
    {
        private int m_id = 0;

        public int id { get { return m_id; } }

        public virtual void parse(List<string> cells, ref int i)
        {
            if (null == cells)
                UnityEngine.Debug.LogErrorFormat("cells is null");

            m_id = toInt(cells, ref i);
        }

        protected string toString(List<string> cells, ref int i, bool isReplaceNewLine = false)
        {
            string str = cells[i++];
            if (isReplaceNewLine)
            {
                if (string.IsNullOrEmpty(str))
                    return str;
                else
                    return str.Replace("\\n", "\n");
            }
            else
            {
                return str;
            }
        }

        protected bool toBool(List<string> cells, ref int i)
        {
            return 0 != toInt(cells, ref i);
        }

        protected int toInt(List<string> cells, ref int i)
        {
            try
            {
                return Convert.ToInt32(cells[i++], CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0;
        }

        protected long toLong(List<string> cells, ref int i)
        {
            try
            {
                return System.Convert.ToInt64(cells[i++], CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0;
        }

        protected float toFloat(List<string> cells, ref int i)
        {
            try
            {
                // 소수점 2자리까지 오차 보정
                float v = System.Convert.ToSingle(cells[i++], CultureInfo.InvariantCulture);
                return (float)Math.Truncate((v + 0.000001f) * 100) / 100.0f;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0.0f;
        }

        protected decimal toDecimal(List<string> cells, ref int i)
        {
            try
            {
                return System.Convert.ToDecimal(cells[i++], CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return 0;
        }

        protected List<T> toList<T>(List<string> cells, ref int i, char seperator = ',')
        {
            try
            {
                List<T> list = new List<T>();
                List<string> s = SystemManager.fsplit(cells[i++], seperator);
                for (int c = 0; c < s.Count; ++c)
                {
                    list.Add((T)Convert.ChangeType(s[c], typeof(T), CultureInfo.InvariantCulture));
                }

                return list;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return null;
        }

        protected HashSet<T> toHashSet<T>(List<string> cells, ref int i, char seperator = ',')
        {
            try
            {
                HashSet<T> hashSet = new HashSet<T>();
                List<string> s = SystemManager.fsplit(cells[i++], seperator);
                for (int c = 0; c < s.Count; ++c)
                {
                    hashSet.Add((T)Convert.ChangeType(s[c], typeof(T), CultureInfo.InvariantCulture));
                }

                return hashSet;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogErrorFormat("id {0}, value : {1}, column {2}, {3}", m_id, cells[i - 1], i - 1, e.ToString());
            }

            return null;
        }

        protected Color toHtmlColor(List<string> cells, ref int i)
        {
            var html = toString(cells, ref i);
            if (ColorUtility.TryParseHtmlString(html, out Color color))
                return color;

            UnityEngine.Debug.LogErrorFormat("Failed toHtmlColor {0}", html);

            return Color.white;
        }

        protected T toType<T>(List<string> cells, ref int i)
        {
            return (T)Convert.ChangeType(cells[i++], typeof(T), CultureInfo.InvariantCulture);
        }
    }
}