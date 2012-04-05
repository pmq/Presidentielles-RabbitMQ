using System;
using System.Globalization;
using System.Xml.Linq;
using GalaSoft.MvvmLight;
using TechDays.Extensions;

namespace TechDays.UI.Map.ViewModel
{
    public class CandidatViewModel : ViewModelBase
    {
        const String GENDER_H = "M.";

        #region ===== Gender =====
        /// <summary>
        /// Variable privée associée à la propriété
        /// </summary>
        private String _gender;
        /// <summary>
        /// Gender
        /// </summary>
        public String Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged("Gender");
            }
        }
        #endregion  ===== Gender =====

        #region ===== Name =====
        /// <summary>
        /// 
        /// </summary>
        private String _name;
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }
        #endregion

        #region ===== FirstName =====
        /// <summary>
        /// 
        /// </summary>
        private String _firstname;
        /// <summary>
        /// 
        /// </summary>
        public String FirstName
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                RaisePropertyChanged("FirstName");
            }
        }
        #endregion

         #region ===== Result =====
        /// <summary>
        /// 
        /// </summary>
        private int _result;
        /// <summary>
        /// 
        /// </summary>
        public int Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }
        #endregion

        #region ===== Pct =====
        /// <summary>
        /// 
        /// </summary>
        private double _pct;
        /// <summary>
        /// 
        /// </summary>
        public double Pct
        {
            get { return _pct; }
            set
            {
                _pct = value;
                RaisePropertyChanged("Pct");
            }
        }
        #endregion

        #region ===== Portrait =====
        private String _portrait;
        public String Portrait
        {
            get { return _portrait; }
            set
            {
                _portrait = value;
                RaisePropertyChanged("Portrait");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Uri UrlPortrait
        {
            get
            {
                if (Gender == GENDER_H)
                    return new Uri("pack://application:,,,/TechDays.UI.Map;component/Images/silhouette_homme.jpg");
                else
                    return new Uri("pack://application:,,,/TechDays.UI.Map;component/Images/silhouette_femme.jpg");
            }
        }
        #endregion

        #region ===== Constructors =====

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        public CandidatViewModel(XElement el)
        {
            Name = el.StringValue("Nom");
            FirstName = el.StringValue("Prenom");
            Gender = el.StringValue("Civilite");

            NumberFormatInfo nbi = new NumberFormatInfo();
            nbi.NumberGroupSeparator = " ";

            Result = el.IntValue("Voix", nbi, 0);
            nbi.NumberGroupSeparator = " ";
            Pct = el.DoubleValue("RapVoixExp", 0);

            Portrait = UrlPortrait.ToString();

        }
        #endregion  ===== Constructors =====
    }
}
