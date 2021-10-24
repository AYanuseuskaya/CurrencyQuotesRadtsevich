using ModelTestTask.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ModelTestTask
{
    public class Model : IModelWPF, INotifyPropertyChanged
    {
        public CurrencyCode currencyCodes { get; set; }

        private List<TreeViewData> _viewData;
        public List<TreeViewData> viewData
        {
            get { return _viewData; }
            set
            {
                _viewData = value;
                OnPropertyChanged("viewData");
            }
        }
        private List<CurrentCourse> _currentCourses;
        public List<CurrentCourse> currentCourses {
            get { return _currentCourses; }
            set
            {
                _currentCourses = value;
                OnPropertyChanged("currentCourses");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public async Task DownloadFile(string loadFromFilePath)
        {
            try
            {
                HttpClient _client = new HttpClient();
                _client.BaseAddress = new Uri("http://www.cbr-xml-daily.ru/");
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await _client.GetAsync(loadFromFilePath);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        currencyCodes = JsonSerializer.Deserialize<CurrencyCode>(jsonString);
                    }
                    catch (SystemException ex)
                    {

                    }

                }
            }
            catch (SystemException ex)
            {

            }
        }
        public async Task GetViewData(string loadFromFilePath)
        {
            await DownloadFile(loadFromFilePath);
            ConvertDictionaryViewData();
            ConvertDictionarycurrentCourses();
        }
        public async Task GetCurrentCourse(string loadFromFilePath)
        {
            await DownloadFile(loadFromFilePath);
            ConvertDictionarycurrentCourses();
        }
        public async Task<List<string>> ComboCurrencyCodes(string loadFromFilePath)
        {
            await DownloadFile(loadFromFilePath);
            List<string> comboData = new List<string>();
            if (currencyCodes != null && currencyCodes.Valute != null)
            {
                comboData = currencyCodes.Valute
                         .Select(e => e.Key)
                         .ToList();
                ConvertDictionarycurrentCourses();
            }
            return comboData;
        }
        public async Task<string> ConvertCurrency(string loadFromFilePath, string currencyFrom, string currencyTo, decimal value)
        {
            decimal result = 0;
            await DownloadFile(loadFromFilePath);
            ConvertDictionarycurrentCourses();
            if (currentCourses != null)
            {
                CurrentCourse codeFrom = currentCourses.FirstOrDefault(s => s.code.CharCode == currencyFrom);
                CurrentCourse codeTo = currentCourses.FirstOrDefault(s => s.code.CharCode == currencyTo);
                result = convertCurrency(codeFrom.code.Value, codeFrom.code.Nominal, codeTo.code.Value, codeTo.code.Nominal, value);
            }
            return result.ToString();
        }

       public decimal  convertCurrency(decimal codeFromValue, decimal codeFromNominal, decimal codeToValue, decimal codeToNominal, decimal value)
        {
            decimal result = 0;
            decimal codeToCours = 0;
            decimal codeFromCourse = 0;
            if (codeFromNominal != 0)
            {
                codeFromCourse = Math.Round(codeFromValue / codeFromNominal, 4);
            }
            if (codeToNominal != 0)
            {
                codeToCours = Math.Round(codeToValue / codeToNominal, 4);
            }
            if (codeToCours != 0)
            {
                result = Math.Round(codeFromCourse * value/ codeToCours, 4);
            }
            return result;
        }
        public async Task<string> SearchResponse(string loadFromFilePath, string currencyCode)
        {
            string resultSeachCode = "";
            decimal rubcourse = 0;
            decimal usdcourse = 0;
            await DownloadFile(loadFromFilePath);
            ConvertDictionarycurrentCourses();
            if (currentCourses != null && currentCourses.Count > 0)
            {
                CurrentCourse current = currentCourses.FirstOrDefault(s => s.Name == currencyCode || s.code.NumCode == currencyCode);
                CurrentCourse codeUsd = currentCourses.FirstOrDefault(s => s.code.CharCode == "USD");
                if (current != null && codeUsd != null)
                {
                    rubcourse = PaymentCourseRUB(current.code.Value, current.code.Nominal);
                    usdcourse = PaymentCourseUSD(codeUsd.code.Value, codeUsd.code.Nominal, rubcourse);
                }
            }
            if (rubcourse != 0 && usdcourse != 0)
            {
                resultSeachCode = "1 " + currencyCode + " = " + rubcourse.ToString() + " рублей и " + usdcourse.ToString() + " y.e";
            }
            return resultSeachCode;
        }
        public decimal PaymentCourseRUB (decimal currentCodeValue, decimal currentCodeNominal)
        {
            decimal rubcourse = 0;
            if (currentCodeNominal != 0)
            {
                rubcourse = Math.Round(currentCodeValue / currentCodeNominal, 4);
            }
            return rubcourse;
        }
        public decimal PaymentCourseUSD(decimal codeUsdValue, decimal codeUsdNominal,decimal rubcourse)
        {
            decimal usdcourse = 0;
            if (codeUsdNominal != 0)
            {
                usdcourse = Math.Round(rubcourse / (codeUsdValue / codeUsdNominal), 4);
            }
            return usdcourse;
        }
        void ConvertDictionaryViewData()
        {
            if (currencyCodes != null && currencyCodes.Valute != null && currencyCodes.Valute.Count > 0)
            {
                viewData = currencyCodes.Valute
                         .Select(e => new TreeViewData { Name = e.Key, CodeData = new List<Code>() { e.Value } })
                         .ToList();
            }
        }
        void ConvertDictionarycurrentCourses()
        {
            if (currencyCodes != null && currencyCodes.Valute != null && currencyCodes.Valute.Count > 0)
            {
                currentCourses = currencyCodes.Valute
                         .Select(e => new CurrentCourse { Name = e.Key, code = e.Value })
                         .ToList();
            }
        }
    }
}
