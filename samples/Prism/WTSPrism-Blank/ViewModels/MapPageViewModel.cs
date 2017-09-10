using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls.Maps;
using WTSPrism.Helpers;
using WTSPrism.Services;

namespace WTSPrism.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        // TODO WTS: Set your preferred default zoom level
        private const double defaultZoomLevel = 17;

        private readonly ILocationService _locationService;

        // TODO WTS: Set your preferred default location if a geolock can't be found.
        private readonly BasicGeoposition defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425,
            Longitude = -122.3417
        };

        private string _mapServiceToken;
        public string MapServiceToken
        {
            get { return _mapServiceToken; }
            set { SetProperty(ref _mapServiceToken, value); }
        }

        private double _zoomLevel;
        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { SetProperty(ref _zoomLevel, value); }
        }

        private Geopoint _center;
        public Geopoint Center
        {
            get { return _center; }
            set { SetProperty(ref _center, value); }
        }

        private ObservableCollection<MapIcon> mapIcons = new ObservableCollection<MapIcon>();
        public ObservableCollection<MapIcon> MapIcons
        {
            get { return mapIcons; }
            set { SetProperty(ref mapIcons, value); }
        }

        public MapPageViewModel(ILocationService locationService)
        {
            _locationService = locationService;
            Center = new Geopoint(defaultPosition);
            ZoomLevel = defaultZoomLevel;

            // TODO WTS: Set your map service token. If you don't have it, request at https://www.bingmapsportal.com/            
            MapServiceToken = "";
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            if (_locationService != null)
            {
                _locationService.PositionChanged += LocationServicePositionChanged;

                var initializationSuccessful = await _locationService.InitializeAsync();

                if (initializationSuccessful)
                {
                    await _locationService.StartListeningAsync();
                }

                if (initializationSuccessful && _locationService.CurrentPosition != null)
                {
                    Center = _locationService.CurrentPosition.Coordinate.Point;
                }
                else
                {
                    Center = new Geopoint(defaultPosition);
                }
            }

            var mapIcon = new MapIcon()
            {
                Location = Center,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = "Map_YourLocation".GetLocalized(),
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };
            MapIcons.Add(mapIcon);
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationServicePositionChanged;
                _locationService.StopListening();
            }
        }

        private void LocationServicePositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
        }
    }
}
