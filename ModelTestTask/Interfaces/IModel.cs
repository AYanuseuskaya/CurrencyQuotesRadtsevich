using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTestTask.Interfaces
{
    public interface IModel
    {
        CurrencyCode currencyCodes { get; set; }
        Task GetViewData(string loadFromFilePath);
        Task DownloadFile(string loadFromFilePath);
        Task GetCurrentCourse(string loadFromFilePath);
        Task<string> ConvertCurrency(string loadFromFilePath, string currencyFrom, string currencyTo, decimal value);
        Task<string> SearchResponse(string loadFromFilePath, string currencyCode);
    }
}
