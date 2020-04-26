using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteProperty.Common
{
    public class InsertableList<T> where T : class
    {
        private T[] _values;
        private int _mainIndex = 0;
 
        public delegate void ValuesChangedHandler();
        public event ValuesChangedHandler ValuesChangedEvent;
    
        public InsertableList(int lenght)
        {
            _values = new T[lenght];
        }

        public T GetMain() => this.Get(_mainIndex);

        public void SetMainIndex(int index)
        {
            if (index < _values.Length)
            {
                _mainIndex = index;
                ValuesChangedEvent.Invoke();
            }
        }

        public T this[int index]
        {
            get
            {                
                return Get(index);
            }
        }

        public T Get(int index)
        {                
            return _values[index];
        }

        public void Insert(T newVal)
        {

            for (int i = _values.Length - 1; i >= 0; i--)
            {
                if (i == _values.Length - 1)
                    continue;

                _values[i + 1] = _values[i];
            }
            _values[0] = newVal;
            ValuesChangedEvent.Invoke();
        }
    }
}
