using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using WTSPrism.Models;
using WTSPrism.Services;

namespace WTSPrism.ViewModels
{
    public class GridPageViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;

        public GridPageViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public ObservableCollection<Order> Source => _sampleDataService.GetGridSampleData();
    }
}
