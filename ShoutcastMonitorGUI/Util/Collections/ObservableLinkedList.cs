using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ShoutcastMonitorGUI.Util.Collections
{
    /// <inheritdoc cref="IEnumerable" />
    /// <summary>
    ///     This class is a LinkedList that can be used in a WPF MVVM scenario. Composition was used instead of inheritance,
    ///     because inheriting from LinkedList does not allow overriding its methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// https://stackoverflow.com/questions/6996425/observable-linkedlist
    /// Modified by Michał Kleszczyński, 17.08.2018
    public class ObservableLinkedList<T> : INotifyCollectionChanged, IEnumerable
    {
        private readonly LinkedList<T> _innerLinkedList;

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_innerLinkedList as IEnumerable).GetEnumerator();
        }

        #endregion

        #region Variables accessors

        public int Count => _innerLinkedList.Count;

        public LinkedListNode<T> First => _innerLinkedList.First;

        public LinkedListNode<T> Last => _innerLinkedList.Last;

        #endregion

        #region Constructors

        public ObservableLinkedList()
        {
            _innerLinkedList = new LinkedList<T>();
        }

        public ObservableLinkedList(IEnumerable<T> collection)
        {
            _innerLinkedList = new LinkedList<T>(collection);
        }

        #endregion

        #region LinkedList<T> Composition

        public LinkedListNode<T> AddAfter(LinkedListNode<T> prevNode, T value)
        {
            var ret = _innerLinkedList.AddAfter(prevNode, value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            _innerLinkedList.AddAfter(node, newNode);
            OnNotifyCollectionChanged();
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            var ret = _innerLinkedList.AddBefore(node, value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            _innerLinkedList.AddBefore(node, newNode);
            OnNotifyCollectionChanged();
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            var ret = _innerLinkedList.AddFirst(value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            _innerLinkedList.AddFirst(node);
            OnNotifyCollectionChanged();
        }

        public LinkedListNode<T> AddLast(T value)
        {
            var ret = _innerLinkedList.AddLast(value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            _innerLinkedList.AddLast(node);
            OnNotifyCollectionChanged();
        }

        public void Clear()
        {
            _innerLinkedList.Clear();
            OnNotifyCollectionChanged();
        }

        public bool Contains(T value)
        {
            return _innerLinkedList.Contains(value);
        }

        public void CopyTo(T[] array, int index)
        {
            _innerLinkedList.CopyTo(array, index);
        }

        public bool LinkedListEquals(object obj)
        {
            return _innerLinkedList.Equals(obj);
        }

        public LinkedListNode<T> Find(T value)
        {
            return _innerLinkedList.Find(value);
        }

        public LinkedListNode<T> FindLast(T value)
        {
            return _innerLinkedList.FindLast(value);
        }

        public Type GetLinkedListType()
        {
            return _innerLinkedList.GetType();
        }

        public bool Remove(T value)
        {
            var ret = _innerLinkedList.Remove(value);
            OnNotifyCollectionChanged();
            return ret;
        }

        public void Remove(LinkedListNode<T> node)
        {
            _innerLinkedList.Remove(node);
            OnNotifyCollectionChanged();
        }

        public void RemoveFirst()
        {
            _innerLinkedList.RemoveFirst();
            OnNotifyCollectionChanged();
        }

        public void RemoveLast()
        {
            _innerLinkedList.RemoveLast();
            OnNotifyCollectionChanged();
        }

        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void OnNotifyCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}