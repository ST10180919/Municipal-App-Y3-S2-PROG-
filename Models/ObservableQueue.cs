using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    internal class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        // Event that will notify the UI when the collection changes
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        // Event that will notify the UI when a property changes (e.g., Count)
        public event PropertyChangedEventHandler PropertyChanged;

        // Override the Enqueue method to add notification behavior
        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            // Raise the CollectionChanged event for adding an item
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

            // Notify that the Count property has changed
            OnPropertyChanged(nameof(Count));
        }

        // Override the Dequeue method to add notification behavior
        public new T Dequeue()
        {
            var item = base.Dequeue();

            // Raise the CollectionChanged event for removing an item
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            // Notify that the Count property has changed
            OnPropertyChanged(nameof(Count));

            return item;
        }

        // Notify when properties (like Count) change
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Notify when the collection changes (items added or removed)
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        // Clear the queue and raise notification events
        public new void Clear()
        {
            base.Clear();

            // Raise the CollectionChanged event for resetting the collection
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            // Notify that the Count property has changed
            OnPropertyChanged(nameof(Count));
        }
    }
}
