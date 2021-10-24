using ModelTestTask;
using ModelTestTask.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModelTestTask
{
    public class ViewModel :  INotifyPropertyChanged
    {
        string filePath;
        IModelWPF Model { get; set; }
        public List<TreeViewData> Valute
        {
            get { return Model.viewData; }
        }
        public List<CurrentCourse> currentCourse
        {
            get { return Model.currentCourses; }
        }
   
        private List<string> _comboCurrencyCodes;
        public List<string> ComboCurrencyCodes
        {
            get { return _comboCurrencyCodes; }
            set
            {
                _comboCurrencyCodes = value;
                OnPropertyChanged("ComboCurrencyCodes");
            }
        }
        private string seachCode;
        public string SeachCode
        {
            get { return seachCode; }
            set
            {
                seachCode = value;
                OnPropertyChanged("SeachCode");
            }
        }
        private string resultSeachCode;
        public string ResultSeachCode
        {
            get { return resultSeachCode; }
            set
            {
                resultSeachCode = value;
                OnPropertyChanged("ResultSeachCode");
            }
        }
        private string labelSeachError;
        public string LabelSeachError
        {
            get { return labelSeachError; }
            set
            {
                labelSeachError = value;
                OnPropertyChanged("LabelSeachError");
            }
        }
        private string convertToText;
        public string ConvertToText
        {
            get { return convertToText; }
            set
            {
                convertToText = value;
                OnPropertyChanged("ConvertToText");
            }
        }
        private string convertToComboSelect;
        public string ConvertToComboSelect
        {
            get { return convertToComboSelect; }
            set
            {
                convertToComboSelect = value;
                OnPropertyChanged("ConvertToComboSelect");
            }
        }
        private string convertFromText;
        public string ConvertFromText
        {
            get { return convertFromText; }
            set
            {
                convertFromText = value;
                OnPropertyChanged("ConvertFromText");
            }
        }
        private string convertFromComboSelect;
        public string ConvertFromComboSelect
        {
            get { return convertFromComboSelect; }
            set
            {
                convertFromComboSelect = value;
                OnPropertyChanged("ConvertFromComboSelect");
            }
        }

        public ViewModel(IModelWPF model)
        {
            this.Model = model;
            filePath = "http://www.cbr-xml-daily.ru/daily_json.js";
            labelSeachError = "";
            Model.PropertyChanged += OnModelPropertyChanged;
            GetCurrensyCodeAsync();
        }
        private RelayCommand getDataCommand;
        public RelayCommand GetDataCommand
        {
            get
            {
                return getDataCommand ??
                  (getDataCommand = new RelayCommand
                  (obj =>
                  {
                      Model.GetViewData(filePath);
                  }));
            }
        }
        private RelayCommand getCurrentCourse;
        public RelayCommand GetCurrentCourse
        {
            get
            {
                return getCurrentCourse ??
                  (getCurrentCourse = new RelayCommand
                  (obj =>
                  {
                      Model.GetCurrentCourse(filePath);
                  }));
            }
        }
        private RelayCommand convertCurrencyCommandFrom;
        public RelayCommand ConvertCurrencyCommandFrom
        {
            get
            {
                return convertCurrencyCommandFrom ??
                  (convertCurrencyCommandFrom = new RelayCommand
                  (obj =>
                  {
                      ConvertData("From");
                  }));
            }
        }
        private RelayCommand convertCurrencyCommandTo;
        public RelayCommand ConvertCurrencyCommandTo
        {
            get
            {
                return convertCurrencyCommandTo ??
                  (convertCurrencyCommandTo = new RelayCommand
                  (obj =>
                  {
                      ConvertData("To");
                  }));
            }
        }
        private RelayCommand seachDataCommand;
        public RelayCommand SeachDataCommand
        {
            get
            {
                return seachDataCommand ??
                  (seachDataCommand = new RelayCommand
                  (obj =>
                  {
                      SearchResponse();
                  }));
            }
        }
        private async void ConvertData (string from)
       {
           switch (from)
            {
                case "From":
                    if (ConvertFromText != null && ConvertFromText != "")
                    {
                        ConvertToText = await Model.ConvertCurrency(filePath, ConvertFromComboSelect, ConvertToComboSelect, Convert.ToDecimal(ConvertFromText));
                    }
                    else if (ConvertToText != null && ConvertToText != "")
                    {
                        ConvertToText = "";
                    }
                    break;
                case "To":
                    if (ConvertToText != null && ConvertToText != "")
                    {
                        ConvertFromText =await Model.ConvertCurrency(filePath, ConvertToComboSelect, ConvertFromComboSelect, Convert.ToDecimal(ConvertToText));
                    }
                    else if (ConvertFromText != null && ConvertFromText != "")
                    {
                        ConvertFromText = "";
                    }
                    break;
            }
       }
        private async void GetCurrensyCodeAsync()
        {
            ComboCurrencyCodes = await Model.ComboCurrencyCodes(filePath);
        }
        private async void SearchResponse()
        {
            ResultSeachCode = await Model.SearchResponse(filePath, SeachCode);
            if (ResultSeachCode == "")
            {
                LabelSeachError = "Введите правильно числовой код валюты либо код страны";
            }
            else
            {
                LabelSeachError = "";
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.currentCourses))
            {
                RaisePropertyChanged("currentCourse");
            }
            if (e.PropertyName == nameof(Model.viewData))
            {
                RaisePropertyChanged("Valute");
            }
        }
        private void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
