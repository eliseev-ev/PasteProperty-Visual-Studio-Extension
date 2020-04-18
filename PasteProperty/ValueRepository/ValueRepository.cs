
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteProperty.ValueRepository
{
    public class ValueRepository : IValueRepository
    {

        string[] _values = new string[4];
        int mainPosition = 0;

        public string GetMainValue()
        {
            return _values[mainPosition];
        }

        public string GetValue(int position)
        {
            CheckPositionAndThrowExeption(position);
            mainPosition = position;
            return _values[position];
        }

        public string SetValue(int position, string value)
        {
            CheckPositionAndThrowExeption(position);
            mainPosition = position;
            _values[position] = value;
            return value;
        }

        public void SelectPosition(int position)
        {
            CheckPositionAndThrowExeption(position);
            mainPosition = position;
        }

        private void CheckPositionAndThrowExeption(int position)
        {
            if (position < 0 || position >= _values.Length)
                throw new IndexOutOfRangeException();
        }
    }
}
