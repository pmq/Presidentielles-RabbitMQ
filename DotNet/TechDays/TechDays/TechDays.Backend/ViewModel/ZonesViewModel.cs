using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using TechDays.Data;
using RabbitMQ.Client;
using TechDays.Extensions;
using TechDays.Messaging.RabbitMQ;
using TechDays.Threading.Collections;

namespace TechDays.Backend.ViewModel
{
    public class ZonesViewModel : ViewModelBase
    {
        private int round = 2;
        private Producer _producer;
        private BackgroundWorker _bw = null;

        #region ===== Uri of index =====
       
        private String _indexfile;
        public String IndexFile
        {
            get { return _indexfile; }
            set
            {
                if (_indexfile != value)
                {
                    _indexfile = value;
                    RaisePropertyChanged("IndexFile");
                }
            }
        }
        #endregion  ===== Uri of index =====

        #region ===== Status of service =====
       
        private bool _isstarted;
        public bool IsStarted
        {
            get { return _isstarted; }
            set
            {
                if (_isstarted != value)
                {
                    _isstarted = value;
                    RaisePropertyChanged("IsStarted");
                }
            }
        }
        #endregion  ===== Status of service =====

        #region ===== List of zones =====
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private MultithreadedObservableCollection<ZoneViewModel> _zones = new MultithreadedObservableCollection<ZoneViewModel>();
        /// <summary>
        /// Zones
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
        #endregion  ===== List of zones =====

        #region ===== Selected department =====
       
        private ZoneViewModel _SelectedZone;
        public ZoneViewModel SelectedZone
        {
            get { return _SelectedZone; }
            set
            {
                if (_SelectedZone != value)
                {
                   _SelectedZone = value;
                    RaisePropertyChanged("SelectedZone");
                }
            }
        }
        #endregion  ===== Selected department =====
        
        /// <summary>
        /// ZonesViewModel
        /// </summary>
        public ZonesViewModel()
        {
            Initialize();
        }

        #region ===== Private Functions ======
        
        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            IndexFile = DataConstants.INDEX_XML;
            InitializeProducer();
            InitializeBackgroundWorker();
            InitializeZones();
            Start();
        }
        /// <summary>
        /// InitializeZones
        /// </summary>
        private void InitializeZones()
        {
            XElement elzs = XElement.Load(IndexFile);
            foreach (var elz in elzs.Element("ListeDpt").Elements("Dpt"))
            {
                ZoneViewModel zvm = new ZoneViewModel(elz);
                _zones.Add(zvm);
            }
        }

        /// <summary>
        /// InitializeBackgroundWorker
        /// </summary>
        private void InitializeBackgroundWorker()
        {
            _bw = new BackgroundWorker();
            _bw.WorkerReportsProgress = true;
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += new DoWorkEventHandler(_bw_DoWork);
            _bw.ProgressChanged += new ProgressChangedEventHandler(_bw_ProgressChanged);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bw_RunWorkerCompleted);
        }

        /// <summary>
        /// InitializeProducer
        /// </summary>
        private void InitializeProducer()
        {
            _producer = new Producer(ConnectionRabbitMQConstants.HOST_NAME, ConnectionRabbitMQConstants.EXCHANGE_NAME, ExchangeType.Fanout);
        }

        /// <summary>
        /// UpdateZone
        /// </summary>
        /// <param name="el"></param>
        private void UpdateZone(XElement elz)
        {
            String CodDpt = elz.StringValue("CodDpt");
            var zone = _zones.Where(o => o.CodDpt == CodDpt).SingleOrDefault();
            if (zone != null)
            {
                if (zone.Update(elz))
                {
                    // Search the Winner
                    //
                    XElement elc = Winner(elz);
                    if (elc != null)
                    {
                        elz.Add(elc);
                    }
                    _producer.SendMessage(System.Text.Encoding.UTF8.GetBytes(elz.AsString()));
                }
            }
        }

        /// <summary>
        /// Winner
        /// </summary>
        /// <param name="elz"></param>
        /// <returns></returns>
        private XElement Winner(XElement elz)
        {
            XElement winner = null;

            String CodReg3Car = elz.StringValue("CodReg3Car");
            String CodReg = elz.StringValue("CodReg");
            String CodDpt3Car = elz.StringValue("CodDpt3Car");
            String CodDpt = elz.StringValue("CodDpt");

            NumberFormatInfo nbi = new NumberFormatInfo();
            nbi.NumberGroupSeparator = " ";

            String uri = String.Format(DataConstants.URL_XML, CodReg3Car, CodDpt3Car, String.Format("{0}{1}", CodReg, CodDpt));
            try
            {
                XElement res = XElement.Load(uri);
                winner = res.Descendants("Candidat")
                    .Where(o => o.Parent.Parent.Parent.StringValue("NumTour") == round.ToString())
                    .Where(o => o.Parent.Parent.StringValue("CodPeriod") == String.Format("SaisieResT{0}", round))
                    .OrderByDescending(o => o.IntValue("Voix", nbi, 0)).FirstOrDefault();
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex.Message);
            }
            return winner;
        }

        #endregion ===== Private Functions ======

        #region ===== Public Method =====
        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            if (_producer != null)
                _producer.ConnectTo();

            if (!_bw.IsBusy)
            {
                _bw.RunWorkerAsync();
            }

        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            if (_bw.IsBusy)
                _bw.CancelAsync();

            if (_producer != null)
                _producer.Disconnect();
        }
        /// <summary>
        /// Closed
        /// </summary>
        public void Closed()
        {
            ZoneViewModel zvm = SelectedZone;
            if (zvm != null)
            {
                XElement elzs = XElement.Load(IndexFile);
                XElement elz = elzs.Descendants("Dpt").Where(o => o.StringValue("CodDpt3Car") == zvm.CodDpt3Car).SingleOrDefault();
                if (elz != null)
                {
                    elz.SetElementValue("Clos", "CLOS");
                    elzs.Save(IndexFile);
                }
            }
        }
        /// <summary>
        /// Initialize
        /// </summary>
        public void Raz()
        {
            XElement elzs = XElement.Load(IndexFile);
            foreach (var elz in elzs.Element("ListeDpt").Elements("Dpt"))
            {
                elz.SetElementValue("Clos", "NON CLOS");
            }
            elzs.Save(IndexFile);
        }
        #endregion  ===== Public Method =====

        #region ===== BackgroundWorker =====
        /// <summary>
        /// _bw_DoWork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _bw_DoWork(object sender, DoWorkEventArgs e)
        {
            _bw.ReportProgress(1);
            while (!_bw.CancellationPending)
            {
                try
                {
                    XElement zones = XElement.Load(IndexFile);
                    foreach (var zone in zones.Element("ListeDpt").Elements("Dpt"))
                    {
                        try
                        {
                            UpdateZone(zone);
                        }
                        catch (Exception Ex)
                        {
                            Debug.WriteLine(Ex.Message);
                        }
                        if (_bw.CancellationPending)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(100);
                }
                catch (Exception Ex)
                {
                    Debug.WriteLine(Ex.Message);
                }
            }
            _bw.ReportProgress(2);
        }
        /// <summary>
        /// _bw_ProgressChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 1:
                    IsStarted = true;
                    break;

                case 2:
                    IsStarted = false;
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsStarted = false;
        }
        #endregion  ===== BackgroundWorker =====

        /// <summary>
        /// All
        /// </summary>
        public void All()
        {
            XElement elzs = XElement.Load(IndexFile);
            foreach (var elz in elzs.Descendants("Dpt").Where(o => o.StringValue("Clos") != "CLOS"))
            {
                elz.SetElementValue("Clos", "CLOS");
            }
            elzs.Save(IndexFile);
        }
    }
}
