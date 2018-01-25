﻿using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EMS.CommonMeasurement;

namespace EMS.Services.CalculationEngineService.PubSub
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PublisherService : ICePubSubContract
    {
        public delegate void OptimizationResultEventHandler(object sender, OptimizationEventArgs e);

        public static event OptimizationResultEventHandler OptimizationResultEvent;

        private ICePubSubCallbackContract callback = null;
        private OptimizationResultEventHandler optimizationResultHandler = null;

       // public static OptimizationType OptimizationType = OptimizationType.None;
        public static Action<OptimizationType> ChangeOptimizationTypeAction;

        /// <summary>
        /// This sevent handler runs when a OptimizationChange event is raised.
        /// The client's OptimizationResults operation is invoked to provide notification about the optimization result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OptimizationResultHandler(object sender, OptimizationEventArgs e)
        {
            callback.OptimizationResults(e.OptimizationResult);
        }

        /// <summary>
        /// Clients call this service opeartion to subscribe.
        /// A optimization result event handler is registered for this client instance.
        /// </summary>
        public void Subscribe()
        {
            callback = OperationContext.Current.GetCallbackChannel<ICePubSubCallbackContract>();
            optimizationResultHandler = new OptimizationResultEventHandler(OptimizationResultHandler);
            OptimizationResultEvent += optimizationResultHandler;
        }

        public void Unsubscribe()
        {
            OptimizationResultEvent -= optimizationResultHandler;
        }


        /// <summary>
        /// Information source, in our case it is Calculation Engine, call this service operation to report a optimization result.
        /// A optimization result event is raised. The optimization result event handlers for each subscriber will execute.
        /// </summary>
        /// <param name="result"></param>
        public void PublishOptimizationResults(List<MeasurementUI> result)
        {
            OptimizationEventArgs e = new OptimizationEventArgs
            {
                OptimizationResult = result,
                Message = "Optimization result"
            };

            try
            {
                // Ovakav nacin radi na VS 2017. Prethodne verzije nemaju kompajler za C#6
                // pa ne moze da kompajlira ovakav kod
                //OptimizationResultEvent?.Invoke(this, e);
                OptimizationResultEvent(this, e);
            }
            catch (Exception ex)
            {
                string message = string.Format("CES does not have any subscribed clinet for publishing new optimization result. {0}", ex.Message);
                CommonTrace.WriteTrace(CommonTrace.TraceVerbose, message);
                Console.WriteLine(message);
            }
        }

        public bool ChooseOptimization(OptimizationType optimizationType)
        {
            //OptimizationType = optimizationType;
            ChangeOptimizationTypeAction?.Invoke(optimizationType);
            return true;
        }
    }
}