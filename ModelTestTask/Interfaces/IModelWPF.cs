using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTestTask.Interfaces
{
    public interface IModelWPF: IModel, INotifyPropertyChanged
    {
        List<TreeViewData> viewData { get; set; }
        List<CurrentCourse> currentCourses { get; set; }
        Task<List<string>> ComboCurrencyCodes(string loadFromFilePath);
    }
}
