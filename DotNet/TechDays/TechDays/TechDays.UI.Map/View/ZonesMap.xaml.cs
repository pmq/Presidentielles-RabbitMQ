using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechDays.Tools;
using TechDays.UI.Map.ViewModel;


namespace TechDays.UI.Map.View
{
    /// <summary>
    /// Logique d'interaction pour ZonesMap.xaml
    /// </summary>
    public partial class ZonesMap : UserControl
    {
        ZonesViewModel _zvm = new ZonesViewModel();

        /// <summary>
        /// 
        /// </summary>
        public ZonesMap()
        {
            InitializeComponent();
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(ZonesMap_DataContextChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ZonesMap_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ResetBinding();
            InitializeBindings();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeBindings()
        {
            foreach (var zone in _zvm.Zones.OrderBy(o => o.CodDpt3Car))
                InitializeBinding(zone);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        private void InitializeBinding(ZoneViewModel zone)
        {
            /*ToolTip tt = GetToolTip(zone);
            zone.Tooltip = tt;*/

            var path = layerDep.FindChildByTag(zone.CodDpt3Car, typeof(Path)) as Path;
            if (path != null)
            {
                var color = new Binding("DisplayColor") { Source = zone, Mode = BindingMode.OneWay };
                path.SetBinding(Path.FillProperty, color);

                var tooltip = new Binding("Tooltip") { Source = zone, Mode = BindingMode.OneWay };
                ToolTipService.SetShowDuration(path, 15000);
                path.SetBinding(Path.ToolTipProperty, tooltip);
            }
            var textBlock = layerNumDep.FindChildByTag(zone.CodDpt3Car, typeof(TextBlock)) as TextBlock;
            if (textBlock != null)
            {
                /* var tooltip = new Binding("Tooltip") { Source = zone, Mode = BindingMode.OneWay };
                ToolTipService.SetShowDuration(path, 15000);
                path.ToolTip = tt;
                path.SetBinding(Path.ToolTipProperty, tooltip); */
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        private ToolTip GetToolTip(ZoneViewModel zone)
        {
            ToolTip tt = new ToolTip();
            WinnersTooltip uc = new WinnersTooltip();
            uc.DataContext = zone;
            tt.Content = uc;
            return tt;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ResetBinding()
        {
            layerDep.ClearAllBindings<Path>();
            layerDepCorse.ClearAllBindings<Path>();
            layerNumDep.ClearAllBindings<TextBlock>();
            layerNumDepCorse.ClearAllBindings<TextBlock>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _zvm;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (_zvm != null)
                _zvm.Stop();
        }
    }
}
