using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WeatherApp.Commands;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Membres
        private BaseViewModel currentViewModel;
        private List<BaseViewModel> viewModels;
        private OpenWeatherService ows;

        #endregion

        #region Propriétés
        /// <summary>
        /// Model actuellement affiché
        /// </summary>
        public BaseViewModel CurrentViewModel
        {
            get { return currentViewModel; }
            set { 
                currentViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Commande pour changer la page à afficher
        /// </summary>
        public DelegateCommand<string> ChangePageCommand { get; set; }

        public List<BaseViewModel> ViewModels
        {
            get {
                if (viewModels == null)
                    viewModels = new List<BaseViewModel>();
                return viewModels; 
            }
        }
        #endregion

        public ApplicationViewModel()
        {
            ChangePageCommand = new DelegateCommand<string>(ChangePage);
           
            /// TODO 11 : Commenter cette ligne lorsque la configuration utilisateur fonctionne
           // var apiKey = AppConfiguration.GetValue("OWApiKey");
            ows = new OpenWeatherService(Properties.Settings.Default.apiKey);

            initViewModels();
        }
        #region Méthodes
        void initViewModels()
        {

            /// TemperatureViewModel setup
            var tvm = new TemperatureViewModel();
            var cvm = new ConfigurationViewModel();
            /// TODO 09 : Indiquer qu'il n'y a aucune clé si le Settings apiKey est vide.
            /// S'il y a une valeur, instancié OpenWeatherService avec la clé
            if (cvm.ApiKey == null || cvm.ApiKey == String.Empty)
            {
                tvm.RawText = "ApiKey manquante, vous devez aller la configurer";
            }
            else
            {   
                tvm.RawText="";
                ows = new OpenWeatherService(cvm.ApiKey);
            }
            tvm.SetTemperatureService(ows);
            ViewModels.Add(tvm);
            ViewModels.Add(cvm);
            /// TODO 01 : ConfigurationViewModel Add Configuration ViewModel

            CurrentViewModel = ViewModels[0];
        }

        private void ChangePage(string pageName)
        {
            /// TODO 10 : Si on a changé la clé, il faudra la mettre dans le service.
            var tvm = (TemperatureViewModel)ViewModels.FirstOrDefault(x => x.Name == "TemperatureViewModel");
            var cvm = (ConfigurationViewModel)ViewModels.FirstOrDefault(x => x.Name == "ConfigurationViewModel");

            /// Algo
            /// Si la vue actuelle est ConfigurationViewModel
            ///   Mettre la nouvelle clé dans le OpenWeatherService
            ///   Rechercher le TemperatureViewModel dans la liste des ViewModels
            ///   Si le service de temperature est null
            ///     Assigner le service de température
            /// 
            if (pageName=="ConfigurationViewModel")
            {
                ows.SetApiKey(Properties.Settings.Default.apiKey);
                
                if(tvm.TemperatureService==null)
                {
                    tvm.SetTemperatureService(ows);
                }

                if(cvm.ApiKey==null||cvm.ApiKey==String.Empty)
                {
                    tvm.RawText = "ApiKey manquante, vous devez aller la configurer";
                }
                else
                {
                    tvm.RawText = "";
                }
            }
            if (pageName == "TemperatureViewModel")
            {
                ows.SetApiKey(Properties.Settings.Default.apiKey);
                if (cvm.ApiKey == null || cvm.ApiKey == String.Empty)
                {
                    tvm.RawText = "ApiKey manquante, vous devez aller la configurer";
                }
                else
                {
                    tvm.RawText = "";
                }
            }
                /// Permet de retrouver le ViewModel avec le nom indiqé
                CurrentViewModel = ViewModels.FirstOrDefault(x => x.Name == pageName);  
        }

        #endregion
    }
}
