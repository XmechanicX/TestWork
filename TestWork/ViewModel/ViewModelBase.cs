using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWork.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void CallPropertyChanged(string propertyName)
        { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }

    }

    public class ViewModelBase<T> : ViewModelBase, IEquatable<ViewModelBase<T>>
    {
        private T _model;

        protected ViewModelBase(T model)
        {
            _model = model;
        }

        public virtual T Model
        {
            get { return _model; }
        }

        public bool Equals(ViewModelBase<T> other)
        {
            return _model.Equals(other._model);
        }
    }
}
