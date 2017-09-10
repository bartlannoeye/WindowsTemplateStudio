using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;
using WTSPrism.Models;

namespace WTSPrism.ViewModels
{
    public class MasterDetailDetailPageViewModel : ViewModelBase
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";
        private readonly INavigationService _navigationService;

        public ICommand StateChangedCommand { get; }

        private Order _item;
        public Order Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public MasterDetailDetailPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Item = e.Parameter as Order;
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                _navigationService.GoBack();
            }
        }
    }
}
