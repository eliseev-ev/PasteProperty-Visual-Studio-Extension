
using Microsoft.VisualStudio.Debugger.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteProperty
{
    public class ValueRepository 
    {

        private ObservableCollection<string> _values = new ObservableCollection<string>()
        {
            "","","",""
        };

        public delegate void ValuesChangedHandler();
        public event ValuesChangedHandler ValuesChangedEvent;

        public ValueRepository()
        {
            _values.CollectionChanged += (_, __) => { ValuesChangedEvent.Invoke(); };
        }


       int mainPosition = 0;

        public string GetMainValue()
        {
                return _values[mainPosition];
        }

        public string GetValue(int position)
        {
            CheckPositionAndThrowExeption(position);
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
            if (position < 0 || position >= _values.Count)
                throw new IndexOutOfRangeException();
        }


    }
}
