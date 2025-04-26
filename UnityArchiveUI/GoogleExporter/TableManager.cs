using System.Collections.Generic;

namespace UnityPMSManager
{
    public class TableManager<T> : NonMonoSingleton<T> where T : class, new()
    {
        private Dictionary<int, ITable> m_tables = new Dictionary<int, ITable>();

        public int tableCount => m_tables.Count;

        public virtual void initialize(Crypto crypto)
        {
            load(crypto);
        }

        protected virtual void load(Crypto crypto)
        {
            clear();
        }

        protected void load<B>(int key, string filename, Crypto crypto) where B : ITable, new()
        {
            if (Logx.isActive)
            {
                if (key < 0)
                    UnityEngine.Debug.LogErrorFormat("Invalid key {0}", key);
                if (string.IsNullOrEmpty(filename))
                    UnityEngine.Debug.LogErrorFormat("Invalid filename");

                UnityEngine.Debug.LogErrorFormat(!m_tables.ContainsKey(key), "{0} ame key has already been added {1}", filename, key);
            }

            B b = new B();
            b.load(filename, crypto);

            m_tables.Add(key, b);
        }

        public B getTable<B>(int key) where B : class, ITable
        {
            if (key < 0)
                UnityEngine.Debug.LogErrorFormat("Invalid key {0}", key);

            if (!m_tables.TryGetValue(key, out ITable t))
                throw new KeyNotFoundException(string.Format("Failed getTable(), {0} is not found", key));

            return t as B;
        }

        public R getRow<R>(int key, int row) where R : TableRow
        {
            if (key < 0)
                UnityEngine.Debug.LogErrorFormat("invalid key {0}", key);
            if (row <= 0)
                UnityEngine.Debug.LogErrorFormat("invalid row {0}", row);

            ITable t = getTable<ITable>(key);
            TableRow tableRow = t.getRow(row);
            if (null == tableRow)
                return null;

            return tableRow as R;
        }

        public bool existRow(int key, int row)
        {
            if (key < 0)
                UnityEngine.Debug.LogErrorFormat("invalid key {0}", key);
            if (row <= 0)
                UnityEngine.Debug.LogErrorFormat("invalid row {0}", row);

            ITable t = getTable<ITable>(key);
            return t.existRow(row);
        }

        public int getRowCount(int key)
        {
            if (key < 0)
                UnityEngine.Debug.LogErrorFormat("Invalid key {0}", key);

            ITable t = getTable<ITable>(key);
            if (null == t)
                return 0;

            return t.rowCount;
        }

        public Dictionary<int, TableRow>.Enumerator getRowEnumerator(int key)
        {
            ITable t = getTable<ITable>(key);
            if (null == t)
                return new Dictionary<int, TableRow>().GetEnumerator();

            return t.getEnumerator();
        }

        private void clear()
        {
            m_tables.Clear();
        }
    }
}