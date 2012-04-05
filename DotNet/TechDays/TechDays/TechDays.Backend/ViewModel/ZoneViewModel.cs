using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Media;
using System.Xml.Linq;
using TechDays.Extensions;
using System.Globalization;

namespace TechDays.Backend.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoneViewModel : ViewModelBase
    {
        #region Propriétés publiques

        #region Etat du département
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private bool _clos;
        /// <summary>
        /// Etat du département
        /// </summary>
        public bool Clos
        {
            get { return _clos; }
            set
            {
                if (_clos != value)
                {
                    _clos = value;
                    RaisePropertyChanged("Clos");
                    RaisePropertyChanged("BackgroundColor");
                    RaisePropertyChanged("ForegroundColor");
                }
            }
        }
        #endregion

        #region Horodatage de cloture du département
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private DateTime _timestamp;
        /// <summary>
        /// Horodatage de cloture du département
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
        #endregion

        #region Code ministère de la région
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private string _CodReg;
        /// <summary>
        /// Code ministère du département
        /// </summary>
        public string CodReg
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
        #endregion

        #region Code ministère du département
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private String _CodDpt;
        /// <summary>
        /// Code ministère du département
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
        #endregion

        #region Code du département
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private String _CodMinDpt;
        /// <summary>
        /// Code du département
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
        #endregion

        #region Code du département sur 3 caractères
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private String _CodDpt3Car;
        /// <summary>
        /// Code du département sur 3 caractères
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
        #endregion

        #region Nom du département
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private string _LibDpt;
        /// <summary>
        /// Nom du département
        /// </summary>
        public string LibDpt
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
        #endregion

        #region Couleur de fond
        /// <summary>
        /// Couleur de fond
        /// </summary>
        public Brush BackgroundColor
        {
            get { return _clos ? Brushes.Green : Brushes.Yellow; }
        }
        #endregion

        #region Couleur du texte
        /// <summary>
        /// Couleur du texte
        /// </summary>
        public Brush ForegroundColor
        {
            get { return _clos ? Brushes.White : Brushes.Black; }
        }
        #endregion

        #region Heure de cloture
        /// <summary>
        /// Heure de cloture
        /// </summary>
        public String Horodatage
        {
            get { return Clos ? Timestamp.ToString("dddd d MMMM yyyy à HH'h'mm:ss") : String.Empty; }
        }
        #endregion

        #endregion

        #region Constructeurs
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

        #region Méthodes privées
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
            Clos = GetStatus(el.StringValue("Clos"));
            Timestamp = Clos ? GetTimestamp(el.StringValue("DateClotureDpt"), el.StringValue("HeureClotureDpt")) : DateTime.MinValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sStatus"></param>
        /// <returns></returns>
        private bool GetStatus(string sStatus)
        {
            return (sStatus.ToLower() == "clos");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="sHeure"></param>
        /// <returns></returns>
        private DateTime GetTimestamp(string sDate, string sHeure)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sDate);
            sb.Append('T');
            sb.Append(sHeure);
            DateTime dt;
            if (!DateTime.TryParseExact(sb.ToString(), "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
            {
                dt = DateTime.MinValue;
            }
            return dt;
        }
        #endregion

        #region Méthodes publiques
        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        /// <returns>Indique si une nouvelle version est disponible</returns>
        public bool Update(XElement el)
        {
            DateTime dtOldTimestamp = Timestamp;
            Clos = GetStatus(el.StringValue("Clos"));
            Timestamp = Clos ? GetTimestamp(el.StringValue("DateClotureDpt"), el.StringValue("HeureClotureDpt")) : DateTime.MinValue;
            return (Clos && (Timestamp > dtOldTimestamp));
        }



        #endregion
    }
}
