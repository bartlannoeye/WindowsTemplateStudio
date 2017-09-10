using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using WTSPrism.Models;
using WTSPrism.Services;

namespace WTSPrism.ViewModels
{
    public class ChartPageViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;

        public ChartPageViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return _sampleDataService.GetChartSampleData();
            }
        }
    }
}
