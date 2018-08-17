namespace ShoutcastMonitorGUI.Util.Collections
{
    public class LimitedStack<T> : ObservableLinkedList<T>
    {
        /// <summary>
        ///     Max size of the collection
        /// </summary>
        public int MaxSize { get; set; }

        public LimitedStack(int maxSize)
        {
            MaxSize = maxSize;
        }

        /// <summary>
        ///     Add item to the stack
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            AddFirst(item);

            if (Count > MaxSize) RemoveLast();
        }

        /// <summary>
        ///     Pop item from the stack
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            var item = First.Value;
            RemoveFirst();
            return item;
        }
    }
}