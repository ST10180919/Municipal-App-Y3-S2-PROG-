using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    // Declaration of AI Use: ChatGPT helped me write the code in this file

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents a generic MaxHeap data structure for storing items in max-heap order.
    /// </summary>
    /// <typeparam name="T">The type of elements stored in the heap, which must implement <see cref="IComparable{T}"/>.</typeparam>
    public class MaxHeap<T> where T : IComparable<T>
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// List of items stored in the heap.
        /// </summary>
        private List<T> items = new List<T>();

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the number of items currently in the heap.
        /// </summary>
        public int Count => items.Count;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Inserts a new item into the heap and ensures the heap property is maintained.
        /// </summary>
        /// <param name="item">The item to insert into the heap.</param>
        public void Insert(T item)
        {
            items.Add(item);
            HeapifyUp(items.Count - 1);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Extracts and removes the maximum item from the heap.
        /// </summary>
        /// <returns>The maximum item in the heap.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the heap is empty.</exception>
        public T ExtractMax()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            T max = items[0];
            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            HeapifyDown(0);

            return max;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Peeks at the maximum item in the heap without removing it.
        /// </summary>
        /// <returns>The maximum item in the heap.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the heap is empty.</exception>
        public T Peek()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            return items[0];
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Ensures the heap property is maintained by moving an item up the heap.
        /// </summary>
        /// <param name="index">The index of the item to move up.</param>
        private void HeapifyUp(int index)
        {
            while (index > 0 && items[Parent(index)].CompareTo(items[index]) < 0)
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Ensures the heap property is maintained by moving an item down the heap.
        /// </summary>
        /// <param name="index">The index of the item to move down.</param>
        private void HeapifyDown(int index)
        {
            int maxIndex = index;

            int left = LeftChild(index);
            if (left < items.Count && items[left].CompareTo(items[maxIndex]) > 0)
                maxIndex = left;

            int right = RightChild(index);
            if (right < items.Count && items[right].CompareTo(items[maxIndex]) > 0)
                maxIndex = right;

            if (index != maxIndex)
            {
                Swap(index, maxIndex);
                HeapifyDown(maxIndex);
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the index of the parent of the specified item.
        /// </summary>
        private int Parent(int index) => (index - 1) / 2;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the index of the left child of the specified item.
        /// </summary>
        private int LeftChild(int index) => (2 * index) + 1;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Gets the index of the right child of the specified item.
        /// </summary>
        private int RightChild(int index) => (2 * index) + 2;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Swaps two items in the heap at the specified indices.
        /// </summary>
        private void Swap(int i, int j)
        {
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents an item stored in the heap with a timestamp and associated issue report data.
    /// </summary>
    public class HeapItem : IComparable<HeapItem>
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Timestamp for ordering heap items.
        /// </summary>
        public DateTime Timestamp { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The issue report associated with this heap item.
        /// </summary>
        public ISSUE_REPORT Data { get; set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Compares the current heap item with another based on their timestamps.
        /// </summary>
        /// <param name="other">The other heap item to compare with.</param>
        /// <returns>An integer indicating the relative order of the two items.</returns>
        public int CompareTo(HeapItem other)
        {
            return this.Timestamp.CompareTo(other.Timestamp);
        }
    }
}
//---------------------------------------EOF-------------------------------------------