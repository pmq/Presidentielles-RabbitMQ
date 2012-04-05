using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TechDays.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public static class UIChildFinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="panel"></param>
        public static void ClearAllBindings<T>(this Panel panel) where T : UIElement 
        {
            foreach (var c in panel.Children)
            {
                if (c is T)
                {
                    var dep = c as T;
                    BindingOperations.ClearAllBindings(dep);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="childTag"></param>
        /// <param name="childType"></param>
        /// <returns></returns>
        public static DependencyObject FindChildByTag(this DependencyObject reference, string childTag, Type childType)
        {
            DependencyObject foundChild = null;
            if (reference != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    // If the child is not of the request child type child
                    if (child.GetType() != childType)
                    {
                        // recursively drill down the tree
                        foundChild = FindChildByTag(child, childTag, childType);
                    }
                    else if (!string.IsNullOrEmpty(childTag))
                    {
                        var frameworkElement = child as FrameworkElement;
                        // If the child's name is set for search
                        if ((frameworkElement != null) && ((frameworkElement.Tag as string) == childTag))
                        {
                            // if the child's name is of the request name
                            foundChild = child;
                            break;
                        }
                    }
                    else
                    {
                        // child element found.
                        foundChild = child; break;
                    }
                }
            } return foundChild;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="childName"></param>
        /// <param name="childType"></param>
        /// <returns></returns>
        public static DependencyObject FindChildByName(this DependencyObject reference, string childName, Type childType)
        {
            DependencyObject foundChild = null;
            if (reference != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    // If the child is not of the request child type child
                    if (child.GetType() != childType)
                    {
                        // recursively drill down the tree
                        foundChild = FindChildByName(child, childName, childType);
                    }
                    else if (!string.IsNullOrEmpty(childName))
                    {
                        var frameworkElement = child as FrameworkElement;
                        // If the child's name is set for search
                        if (frameworkElement != null && frameworkElement.Name == childName)
                        {
                            // if the child's name is of the request name
                            foundChild = child;
                            break;
                        }
                    }
                    else
                    {
                        // child element found.
                        foundChild = child; break;
                    }
                }
            } return foundChild;
        }
    }
}
