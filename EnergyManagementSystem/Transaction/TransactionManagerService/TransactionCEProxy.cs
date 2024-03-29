﻿using EMS.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;
using System.ServiceModel;

namespace EMS.Services.TransactionManagerService
{
    public class TransactionCEProxy : ITransactionContract
    {
        private static ITransactionContract proxy;
        private static DuplexChannelFactory<ITransactionContract> factory;

        public static ITransactionContract Instance
        {
            get
            {
                if (proxy == null)
                {
                    InstanceContext context = new InstanceContext(new TransactionManager());
                    factory = new DuplexChannelFactory<ITransactionContract>(context, "CETransactionEndpoint");
                    proxy = factory.CreateChannel();
                }

                return proxy;
            }

            set
            {
                if (proxy == null)
                {
                    proxy = value;
                }
            }
        }

        public bool Commit()
        {
            return proxy.Commit();
        }

        public UpdateResult Prepare(ref Delta delta)
        {
            return proxy.Prepare(ref delta);
        }

        public bool Rollback()
        {
            return proxy.Rollback();
        }
    }
}