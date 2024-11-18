using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Models
{
    public class MaxHeap<T> where T : IComparable<T>
    {
        private List<T> items = new List<T>();

        public int Count => items.Count;

        public void Insert(T item)
        {
            items.Add(item);
            HeapifyUp(items.Count - 1);
        }

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

        public T Peek()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            return items[0];
        }

        private void HeapifyUp(int index)
        {
            while (index > 0 && items[Parent(index)].CompareTo(items[index]) < 0)
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }

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

        private int Parent(int index) => (index - 1) / 2;
        private int LeftChild(int index) => (2 * index) + 1;
        private int RightChild(int index) => (2 * index) + 2;

        private void Swap(int i, int j)
        {
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }

    public class HeapItem : IComparable<HeapItem>
    {
        public DateTime Timestamp { get; set; }
        public ISSUE_REPORT Data { get; set; }

        // Implement IComparable to define the heap order
        public int CompareTo(HeapItem other)
        {
            return other.Timestamp.CompareTo(this.Timestamp);
        }
    }
}
