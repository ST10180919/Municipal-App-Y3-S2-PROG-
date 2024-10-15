using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Municipal_App.Models
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Disclosure of AI: This Class was generated almost entirely by ChatGPT Model 4o
    /// 
    /// Basically just a Queue that can notify the UI when an item is enqueued or when 
    /// a property changes by implementing the INotifyCollectionChanged and 
    /// INotifyPropertyChanged interfaces
    /// </summary>
    /// <typeparam name="T"> Type </typeparam>
    internal class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event that is triggered whenever the collection changes 
        /// (items are added, removed, or reset).
        /// Notifies the UI to update its view of the collection.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event that is triggered whenever a property of the queue changes, 
        /// such as the Count property.
        /// Notifies the UI to update the bound property.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Adds an item to the queue and triggers the necessary notifications for the UI.
        /// Raises the CollectionChanged event to indicate an item was added and raises 
        /// PropertyChanged for the Count property.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            // Raise the CollectionChanged event for adding an item
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

            // Notify that the Count property has changed
            OnPropertyChanged(nameof(Count));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Removes and returns the item at the beginning of the queue and triggers 
        /// the necessary notifications for the UI.
        /// Raises the CollectionChanged event to indicate an item was removed and raises 
        /// PropertyChanged for the Count property.
        /// </summary>
        /// <returns>The item that was dequeued from the front of the queue.</returns>
        public new T Dequeue()
        {
            var item = base.Dequeue();

            // Raise the CollectionChanged event for removing an item
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            // Notify that the Count property has changed
            OnPropertyChanged(nameof(Count));

            return item;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Triggers the PropertyChanged event to notify the UI that a property value has changed.
        /// This is typically used for properties such as Count.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Triggers the CollectionChanged event to notify the UI that the collection 
        /// has changed (items were added, removed, or reset).
        /// </summary>
        /// <param name="e">The arguments that describe the collection change event.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Clears all items from the queue and triggers the necessary notifications for the UI.
        /// Raises the CollectionChanged event to indicate the collection has been reset and 
        /// raises PropertyChanged for the Count property.
        /// </summary>
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
//---------------------------------------EOF-------------------------------------------