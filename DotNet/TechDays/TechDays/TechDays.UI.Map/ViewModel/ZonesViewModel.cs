using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using RabbitMQ.Client;
using TechDays.Data;
using TechDays.Extensions;
using TechDays.Messaging.RabbitMQ;
using TechDays.Threading.Collections;
using TechDays.UI.Map.View;

namespace TechDays.UI.Map.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class ZonesViewModel : ViewModelBase
	{
		private Consumer _consumer;

		#region ===== Public Properties =====

		#region ===== Round =====
		/// <summary>
		/// N° du tour
		/// </summary>
		private int _round;
		/// <summary>
		/// N° du tour
		/// </summary>
		public int Round
		{
			get { return _round; }
			set
			{
				if (_round != value)
				{
					_round = value;
					RaisePropertyChanged("Round");
				}
			}
		}
		#endregion  ===== Round =====
   
		#region ===== List of zones =====
		/// <summary>
		/// Variable privée associée à la propriété
		/// </summary>
		private MultithreadedObservableCollection<ZoneViewModel> _zones = new MultithreadedObservableCollection<ZoneViewModel>();
		/// <summary>
		/// 
		/// </summary>
		public MultithreadedObservableCollection<ZoneViewModel> Zones
		{
			get { return _zones; }
			set
			{
				if (_zones != value)
				{
					_zones = value;
					RaisePropertyChanged("Zones");
				}
			}
		}
		#endregion  ===== List of department =====

		#region ===== Selected Zone =====
		/// <summary>
		/// Variable privée associée à la propriété
		/// </summary>
		private ZoneViewModel _selectedzone;
		/// <summary>
		/// Département sélectionné
		/// </summary>
		public ZoneViewModel SelectedZone
		{
			get { return _selectedzone; }
			set
			{
				if (_selectedzone != value)
				{
					_selectedzone = value;
					RaisePropertyChanged("SelectedZone");
				}
			}
		}
		#endregion  ===== Selected Zone =====

		#endregion

		#region ===== Constructors =====
		/// <summary>
		/// 
		/// </summary>
		public ZonesViewModel()
		{
			Initialize();
		}
		#endregion

		#region ===== Private Functions =====

		/// <summary>
		/// Initialize
		/// </summary>
		private void Initialize()
		{
			InitializeZones();
			InitializeMessaging();
		}
		/// <summary>
		/// InitializeMessaging
		/// </summary>
		/// <returns></returns>
		private bool InitializeMessaging()
		{
			_consumer = new Consumer(ConnectionRabbitMQConstants.HOST_NAME, ConnectionRabbitMQConstants.EXCHANGE_NAME, ExchangeType.Fanout);
			if (!_consumer.ConnectTo())
				return false;
			_consumer.onMessageReceived += new Consumer.onReceiveMessage(_consumer_onMessageReceived);
			_consumer.StartConsuming();
			return true;
		}

		/// <summary>
		/// InitializeZones
		/// </summary>
		private void InitializeZones()
		{
			XElement elzs = XElement.Load(DataConstants.INDEX_XML);
			foreach (var elz in elzs.Element("ListeDpt").Elements("Dpt"))
			{
				ZoneViewModel z = new ZoneViewModel(elz);
				Zones.Add(z);
			}
		}
		/// <summary>
		/// _consumer_onMessageReceived
		/// </summary>
		/// <param name="message"></param>
		void _consumer_onMessageReceived(byte[] message)
		{
			try
			{
				String msg = System.Text.Encoding.UTF8.GetString(message);
				XElement el = XElement.Parse(msg);
				String CodDpt = el.StringValue("CodDpt");
				var zone = Zones.Where(o => o.CodDpt == CodDpt).SingleOrDefault();
				if (zone != null)
				{
					zone.Update(el);
					zone.SetColor(Brushes.LightGreen, 10);
					zone.SetWinner(el);
					Application.Current.ExecOnUiThread(() =>
					{
						zone.Tooltip = GetToolTip(zone);
					});
				}
			}
			catch (Exception Ex)
			{
				Debug.WriteLine(Ex.Message);
			}
		}

		/// <summary>
		/// GetToolTip
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
		#endregion

		#region ===== Public Functions ======
		/// <summary>
		/// 
		/// </summary>
		public void Stop()
		{
			if (_consumer != null)
			{
				_consumer.Disconnect();
			}
		}
		#endregion
	}

	public static class Extensions
	{
		public static void ExecOnUiThread(this Application app, Action action)
		{
			var dispatcher = app.Dispatcher;
			if (dispatcher.CheckAccess())
				action();
			else
				dispatcher.BeginInvoke(action);
		}

	}
	
}

