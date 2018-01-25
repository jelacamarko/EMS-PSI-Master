﻿using EMS.Common;
using EMS.CommonMeasurement;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UIClient.PubSub;

namespace UIClient.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        #region Fields
        private CeSubscribeProxy ceSubscribeProxy;

        private const int MAX_DISPLAY_NUMBER = 10;
        private const int NUMBER_OF_ALLOWED_ATTEMPTS = 5; // number of allowed attepts to subscribe to the CE
        private int attemptsCount = 0;
        private float currentConsumption;
        private float currentProduction;
        private bool isOptionsExpanded = false;

        private ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> generatorsContainer = new ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>>();
        private ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> energyConsumersContainer = new ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>>();
        private Dictionary<long, bool> gidToBoolMap = new Dictionary<long, bool>();
        private ICommand expandCommand;
        private ICommand visibilityCheckedCommand;
        private ICommand visibilityUncheckedCommand;
        private ICommand changeAlgorithmCommand;
        private OptimizationType selectedOptimizationType = OptimizationType.Linear;


        #endregion

        #region Properties

        public ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> GeneratorsContainer
        {
            get
            {
                return generatorsContainer;
            }

            set
            {
                generatorsContainer = value;
            }
        }

        public ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> EnergyConsumersContainer
        {
            get
            {
                return energyConsumersContainer;
            }

            set
            {
                energyConsumersContainer = value;
            }
        }

        public float CurrentConsumption
        {
            get
            {
                return currentConsumption;
            }

            set
            {
                currentConsumption = value;
                OnPropertyChanged();
            }
        }

        public float CurrentProduction
        {
            get
            {
                return currentProduction;
            }

            set
            {
                currentProduction = value;
                OnPropertyChanged();
            }
        }

        public bool IsOptionsExpanded
        {
            get
            {
                return isOptionsExpanded;
            }

            set
            {
                isOptionsExpanded = value;
                OnPropertyChanged();
            }
        }

        public Dictionary<long, bool> GidToBoolMap
        {
            get
            {
                return gidToBoolMap;
            }

            set
            {
                gidToBoolMap = value;
            }
        }

        public OptimizationType SelectedOptimizationType
        {
            get
            {
                return selectedOptimizationType;
            }

            set
            {
                selectedOptimizationType = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public DashboardViewModel()
        {
            SubsrcibeToCE();
            ceSubscribeProxy.ChooseOptimization(selectedOptimizationType);
        }

        #region Commands
        public ICommand ExpandCommand => expandCommand ?? (expandCommand = new RelayCommand(ExpandCommandExecute));

        public ICommand VisibilityCheckedCommand => visibilityCheckedCommand ?? (visibilityCheckedCommand = new RelayCommand<long>(VisibilityCheckedCommandExecute));

        public ICommand VisibilityUncheckedCommand => visibilityUncheckedCommand ?? (visibilityUncheckedCommand = new RelayCommand<long>(VisibilityUncheckedCommandExecute));

        public ICommand ChangeAlgorithmCommand => changeAlgorithmCommand ?? (changeAlgorithmCommand = new RelayCommand(ChangeAlgorithmCommandExecute));

        #endregion

        #region CommandsExecutions

        private void ExpandCommandExecute(object obj)
        {
            if (IsOptionsExpanded)
            {
                IsOptionsExpanded = false;
            }
            else
            {
                IsOptionsExpanded = true;
            }
        }

        private void VisibilityCheckedCommandExecute(long gid)
        {
            GidToBoolMap[gid] = true;
            OnPropertyChanged(nameof(GidToBoolMap));
        }

        private void VisibilityUncheckedCommandExecute(long gid)
        {
            GidToBoolMap[gid] = false;
            OnPropertyChanged(nameof(GidToBoolMap));
        }

        private void ChangeAlgorithmCommandExecute(object obj)
        {
            ceSubscribeProxy.ChooseOptimization(SelectedOptimizationType);
        }
        #endregion

        private void SubsrcibeToCE()
        {
            try
            {
                ceSubscribeProxy = new CeSubscribeProxy(CallbackAction);
                ceSubscribeProxy.Subscribe();
            }
            catch (Exception e)
            {
                CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Could not connect to Publisher Service! \n ");
                Thread.Sleep(1000);
                if (attemptsCount++ < NUMBER_OF_ALLOWED_ATTEMPTS)
                {
                    SubsrcibeToCE();
                }
                else
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceError, "Could not connect to Publisher Service!  \n {0}", e.Message);
                }
            }
        }

        private void CallbackAction(object obj)
        {
            List<MeasurementUI> measUIs = obj as List<MeasurementUI>;

            if (obj == null)
            {
                throw new Exception("CallbackAction receive wrong param");
            }
            if (measUIs.Count == 0)
            {
                return;
            }

            if ((EMSType)ModelCodeHelper.ExtractTypeFromGlobalId(measUIs[0].Gid) == EMSType.ENERGYCONSUMER)
            {
                AddMeasurmentTo(EnergyConsumersContainer, measUIs);
                CurrentConsumption = measUIs.Sum(x => x.CurrentValue);
            }
            else
            {
                AddMeasurmentTo(GeneratorsContainer, measUIs);
                CurrentProduction = measUIs.Sum(x => x.CurrentValue);
            }
        }

        private void AddMeasurmentTo(ObservableCollection<KeyValuePair<long, ObservableCollection<MeasurementUI>>> container, List<MeasurementUI> measUIs)
        {
            foreach (var measUI in measUIs)
            {
                var keyPair = container.FirstOrDefault(x => x.Key == measUI.Gid);

                if (keyPair.Value == null)
                {
                    var tempQueue = new ObservableCollection<MeasurementUI>();
                    tempQueue.Add(measUI);
                    container.Add(new KeyValuePair<long, ObservableCollection<MeasurementUI>>(measUI.Gid, tempQueue));
                    GidToBoolMap.Add(measUI.Gid, true);
                }
                else
                {
                    keyPair.Value.Add(measUI);
                    if (keyPair.Value.Count > MAX_DISPLAY_NUMBER)
                    {
                        keyPair.Value.RemoveAt(0);
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            ceSubscribeProxy.Unsubscribe();
            base.OnDispose();
        }
    }
}
