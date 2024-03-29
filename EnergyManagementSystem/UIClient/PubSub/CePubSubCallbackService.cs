﻿using EMS.Common;
using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace UIClient.PubSub
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class CePubSubCallbackService : ICePubSubCallbackContract
    {

        private Action<object> callbackAction;

        public Action<object> CallbackAction
        {
            get
            {
                return callbackAction;
            }

            set
            {
                callbackAction = value;
            }
        }

        /// <summary>
        /// This method will receive optimization result form CalculationEngine service.
        /// </summary>
        /// <param name="result"></param>
        public void OptimizationResults(List<MeasurementUI> result)
        {
            Console.WriteLine("OperationContext id: {0}", OperationContext.Current.SessionId);
            Console.WriteLine(string.Format("OPTIMIZATION RESULT: {0}", result.ToString()));

            CommonTrace.WriteTrace(CommonTrace.TraceInfo, "OPTIMIZATION RESULT: {0} | SessionID: {1}",
                                   result.ToString(), OperationContext.Current.SessionId);
            CallbackAction(result);
        }
    }
}