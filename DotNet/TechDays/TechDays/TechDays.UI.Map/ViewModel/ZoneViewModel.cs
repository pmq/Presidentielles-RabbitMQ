using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Media;
using System.Xml.Linq;
using TechDays.Extensions;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using TechDays.UI.Map.View;


namespace TechDays.UI.Map.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoneViewModel : ViewModelBase
    {
        #region ===== Private variables =====
        private Timer _timer = null;
        private DateTime _dtStopBlink;
        #endregion

        #region ===== IsBlinking =====

        private bool _isblinking;
        public bool IsBlinking
        {
            get { return _isblinking; }
            set
            {
                if (_isblinking != value)
                {
                    _isblinking = value;
                    RaisePropertyChanged("IsBlinking");
                }
            }
        }
        #endregion

        #region ===== AlternateColor =====
        /// <summary>
        /// 
        /// </summary>
        private Brush _alternatecolor;
        /// <summary>
        /// 
        /// </summary>
        public Brush AlternateColor
        {
            get { return _alternatecolor; }
            set
            {
                _alternatecolor = value;
                RaisePropertyChanged("AlternateColor");
            }
        }
        #endregion

        #region ==== TimeStartBlinking =====
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private DateTime _timestartblinking;
        /// <summary>
        ///
        /// </summary>
        public DateTime TimeStartBlinking
        {
            get { return _timestartblinking; }
            set
            {
                _timestartblinking = value;
                RaisePropertyChanged("TimeStartBlinking");
            }
        }
        #endregion

        #region ===== Fixed Color =====
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private Brush _fixedcolor;
        /// <summary>
        /// 
        /// </summary>
        public Brush FixedColor
        {
            get { return _fixedcolor; }
            set
            {
                if (_fixedcolor != value)
                {
                    AlternateColor = _fixedcolor;
                    _fixedcolor = value;
                    DisplayColor = _fixedcolor;
                    StopBlinking();
                    RaisePropertyChanged("FixedColor");
                }
            }
        }
        #endregion

        #region ===== DisplayColor =====
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private Brush _displaycolor;
        /// <summary>
        /// Couleur affichée
        /// </summary>
        public Brush DisplayColor
        {
            get { return _displaycolor; }
            set
            {
                if (_displaycolor != value)
                {
                    _displaycolor = value;
                    RaisePropertyChanged("DisplayColor");
                }
            }
        }
        #endregion

        #region ===== Closed =====

        private bool _closed;
        public bool Closed
        {
            get { return _closed; }
            set
            {
                if (_closed != value)
                {
                    _closed = value;
                    RaisePropertyChanged("Closed");
                    RaisePropertyChanged("BackgroundColor");
                    RaisePropertyChanged("ForegroundColor");
                }
            }
        }
        #endregion

        #region ===== Timestamp =====
        /// <summary>
        /// 
        /// </summary>
        private DateTime _timestamp;
        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    RaisePropertyChanged("Timestamp");
                    RaisePropertyChanged("Horodatage");
                }
            }
        }
        #endregion  ===== Timestamp =====

        #region ===== CodReg =====
        private String _CodReg;
        public String CodReg
        {
            get { return _CodReg; }
            set
            {
                if (_CodReg != value)
                {
                    _CodReg = value;
                    RaisePropertyChanged("CodReg");
                }
            }
        }
        #endregion  ===== CodReg =====

        #region ===== CodMinDpt =====
        private String _CodMinDpt;
        /// <summary>
        /// CodMinDpt
        /// </summary>
        public String CodMinDpt
        {
            get { return _CodMinDpt; }
            set
            {
                if (_CodMinDpt != value)
                {
                    _CodMinDpt = value;
                    RaisePropertyChanged("CodMinDpt");
                }
            }
        }
        #endregion ===== CodMinDpt =====

        #region ===== CodDpt =====
        private String _CodDpt;
        /// <summary>
        /// CodDpt
        /// </summary>
        public String CodDpt
        {
            get { return _CodDpt; }
            set
            {
                if (_CodDpt != value)
                {
                    _CodDpt = value;
                    RaisePropertyChanged("CodDpt");
                }
            }
        }
        #endregion ===== CodDpt =====

        #region ===== CodDpt3Car =====
        private String _CodDpt3Car;
        /// <summary>
        /// CodDpt3Car
        /// </summary>
        public String CodDpt3Car
        {
            get { return _CodDpt3Car; }
            set
            {
                if (_CodDpt3Car != value)
                {
                    _CodDpt3Car = value;
                    RaisePropertyChanged("CodDpt3Car");
                }
            }
        }
        #endregion ===== CodDpt3Car =====

        #region ===== LibDpt =====
        /// <summary>
        /// LibDpt
        /// </summary>
        private String _LibDpt;
        public String LibDpt
        {
            get { return _LibDpt; }
            set
            {
                if (_LibDpt != value)
                {
                    _LibDpt = value;
                    RaisePropertyChanged("LibDpt");
                }
            }
        }
        #endregion ===== LibDpt =====

        #region ===== BackgroundColor =====
        /// <summary>
        /// BackgroundColor
        /// </summary>
        public Brush BackgroundColor
        {
            get { return Closed ? Brushes.Green : Brushes.Yellow; }
        }
        #endregion  ===== BackgroundColor =====

        #region ===== ForegroundColor =====
        /// <summary>
        /// ForegroundColor
        /// </summary>
        public Brush ForegroundColor
        {
            get { return Closed ? Brushes.White : Brushes.Black; }
        }
        #endregion  ===== ForegroundColor =====

        #region ===== Horodatage =====
        /// <summary>
        /// 
        /// </summary>
        public String Horodatage
        {
            get { return Closed ? Timestamp.ToString("dddd d MMMM yyyy à HH'h'mm:ss") : String.Empty; }
        }
        #endregion ===== Horodatage =====

        #region ===== Private Method =====

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        private void Initialize(XElement el)
        {
            CodReg = el.StringValue("CodReg");
            CodDpt = el.StringValue("CodDpt");
            CodMinDpt = el.StringValue("CodMinDpt");
            CodDpt3Car = el.StringValue("CodDpt3Car");
            LibDpt = el.StringValue("LibDpt");
            Closed = GetStatus(el.StringValue("Clos"));
            Timestamp = Closed ? GetTimestamp(el.StringValue("DateClotureDpt"), el.StringValue("HeureClotureDpt")) : DateTime.MinValue;
            SetWinner(el);
        }
      
        #endregion

        #region ===== Public Method =====

        /// <summary>
        /// 
        /// </summary>
        public void StartBlinking(int nSecondsToBlink = 30, int nMilliseconds = 500)
        {
            IsBlinking = true;
            _dtStopBlink = DateTime.Now.AddSeconds(nSecondsToBlink);
            _timer = new Timer(new TimerCallback(timer_Tick), null, nMilliseconds, nMilliseconds);
        }
        /// <summary>
        /// 
        /// </summary>
        public void StopBlinking()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
                IsBlinking = false;
                DisplayColor = FixedColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brNewColor"></param>
        /// <param name="nSecondsToBlink"></param>
        public void SetColor(Brush brNewColor, int nSecondsToBlink = 0)
        {
            FixedColor = brNewColor;
            if (nSecondsToBlink > 0)
            {
                StartBlinking(nSecondsToBlink);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        public void SetWinner(XElement el)
        {
            Winners.Clear();
            foreach (var elc in el.Descendants("Candidat"))
            {
                Winners.Add(new CandidatViewModel(elc));
            }
            RaisePropertyChanged("Tooltip");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool GetStatus(String status)
        {
            return (status.ToLower() == "clos");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public DateTime GetTimestamp(String date, String hours)
        {
            StringBuilder oStringBuilder = new StringBuilder();
            oStringBuilder.Append(date);
            oStringBuilder.Append('T');
            oStringBuilder.Append(hours);
            DateTime dt;
            if (!DateTime.TryParseExact(oStringBuilder.ToString(), "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
            {
                dt = DateTime.MinValue;
            }
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        public bool Update(XElement el)
        {
            DateTime dtOldTimestamp = Timestamp;
            Closed = GetStatus(el.StringValue("Clos"));
            Timestamp = Closed ? GetTimestamp(el.StringValue("DateClotureDpt"), el.StringValue("HeureClotureDpt")) : DateTime.MinValue;
            return (Closed && (Timestamp > dtOldTimestamp));
        }
        #endregion ===== Public Method =====

        #region ===== Timer =====
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        void timer_Tick(object state)
        {
            if (_dtStopBlink.Subtract(DateTime.Now).TotalMilliseconds < 0)
            {
                StopBlinking();
                DisplayColor = FixedColor;
            }
            else
            {
                if (DisplayColor == FixedColor)
                {
                    DisplayColor = AlternateColor;
                }
                else
                {
                    DisplayColor = FixedColor;
                }
            }
        }
        #endregion
        
        #region ===== Winners =====
        /// <summary>
        /// 
        /// </summary>
        private List<CandidatViewModel> _winners = new List<CandidatViewModel>();
        /// <summary>
        /// 
        /// </summary>
        public List<CandidatViewModel> Winners
        {
            get { return _winners; }
            set
            {
                _winners = value;
                RaisePropertyChanged("Winners");
            }
        }
        #endregion

        #region ===== Tooltip =====
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private ToolTip _tooltip;
        /// <summary>
        /// Objet ToolTip
        /// </summary>
        public ToolTip Tooltip
        {
            get { return _tooltip; }
            set
            {
                _tooltip = value;
                RaisePropertyChanged("Tooltip");
            }
        }
        #endregion

        #region ===== Constructors =====
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        public ZoneViewModel(XElement el)
        {
            Initialize(el);
        }
        /// <summary>
        /// 
        /// </summary>
        public ZoneViewModel()
        {
        }
        #endregion
    }
}
