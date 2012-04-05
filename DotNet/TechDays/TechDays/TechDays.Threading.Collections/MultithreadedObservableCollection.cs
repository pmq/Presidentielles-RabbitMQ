using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows.Threading;

namespace TechDays.Threading.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultithreadedObservableCollection<T> : ObservableCollection<T>
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public MultithreadedObservableCollection()
        {
            collectionChangedHandlers = new Dictionary<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData>();
        }
        #endregion Constructors

        #region Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            KeyValuePair<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData>[] handlers = collectionChangedHandlers.ToArray();

            if (handlers.Length > 0)
            {
                foreach (KeyValuePair<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData> kvp in handlers)
                {
                    if (kvp.Value.Dispatcher == null)
                    {
                        kvp.Value.Action(e);
                    }
                    else
                    {
                        kvp.Value.Dispatcher.Invoke(kvp.Value.Action, DispatcherPriority.DataBind, e);
                    }
                }
            }
        }
        #endregion Handlers

        #region INotifyCollectionChanged Members
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<NotifyCollectionChangedEventHandler, CollectionChangedWrapperEventData> collectionChangedHandlers;
        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                //Dispatcher dispatcher = Dispatcher.CurrentDispatcher; // should always work
                Dispatcher dispatcher = Dispatcher.FromThread(Thread.CurrentThread); // experimental (can return null)...
                collectionChangedHandlers.Add(value, new CollectionChangedWrapperEventData(dispatcher, new Action<NotifyCollectionChangedEventArgs>((args) => value(this, args))));
            }
            remove
            {
                collectionChangedHandlers.Remove(value);
            }
        }
        #endregion INotifyCollectionChanged Members
    }
    /// <summary>
    /// 
    /// </summary>
    internal class CollectionChangedWrapperEventData
    {
        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public Dispatcher Dispatcher { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Action<NotifyCollectionChangedEventArgs> Action { get; set; }
        #endregion Public Properties

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="action"></param>
        public CollectionChangedWrapperEventData(Dispatcher dispatcher, Action<NotifyCollectionChangedEventArgs> action)
        {
            Dispatcher = dispatcher;
            Action = action;
        }
        #endregion Constructor
    }
}

