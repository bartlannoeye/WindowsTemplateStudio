using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WTSPrism.Models;
using WTSPrism.Services;
using Prism.Windows.Navigation;
using WTSPrism.Constants;

namespace WTSPrism.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;

        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        public MasterDetailPageViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
        {
            _navigationService = navigationService;
            _sampleDataService = sampleDataService;
            ItemClickCommand = new DelegateCommand<ItemClickEventArgs>(OnItemClick);
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
            LoadedCommand = new DelegateCommand<VisualState>(OnLoaded);
        }

        private Order _selected;
        public Order Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICommand ItemClickCommand { get; }
        public ICommand StateChangedCommand { get; }
        public ICommand LoadedCommand { get; }

        public ObservableCollection<Order> SampleItems { get; } = new ObservableCollection<Order>();

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await LoadDataAsync(_currentState);
        }

        public async Task LoadDataAsync(VisualState currentState)
        {
            _currentState = currentState;
            SampleItems.Clear();

            var data = await _sampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
            Selected = SampleItems.First();
        }

        private void OnLoaded(VisualState state)
        {
            _currentState = state;
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            Order item = args?.ClickedItem as Order;
            if (item != null)
            {
                if (_currentState?.Name == NarrowStateName)
                {
                    _navigationService.Navigate(PageTokens.MasterDetailDetailPage, item);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}
