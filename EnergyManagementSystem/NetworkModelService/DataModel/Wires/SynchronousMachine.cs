﻿//-----------------------------------------------------------------------
// <copyright file="SynchronousMachine.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;

    /// <summary>
    /// SynchronousMachine class
    /// </summary>
    public class SynchronousMachine : RotatingMachine
    {
        private float maxQ;
        private float minQ;
        private EmsFuelType fuelType;
        private SynchronousMachineOperatingMode operatingMode;

        

        public SynchronousMachine(long globalId) : base(globalId)
        {

        }

        public override bool Equals(object obj)
        {
            if( base.Equals(obj))
            {
                SynchronousMachine s = (SynchronousMachine)obj;
                return (s.MaxQ == this.MaxQ
                    && s.MinQ == this.MinQ
                    && s.FuelType == this.FuelType
                    && s.OperatingMode == this.OperatingMode);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public float MaxQ
        {
            get
            {
                return maxQ;
            }

            set
            {
                maxQ = value;
            }
        }

        public float MinQ
        {
            get
            {
                return minQ;
            }

            set
            {
                minQ = value;
            }
        }

        public EmsFuelType FuelType
        {
            get
            {
                return fuelType;
            }

            set
            {
                fuelType = value;
            }
        }

        public SynchronousMachineOperatingMode OperatingMode
        {
            get
            {
                return operatingMode;
            }

            set
            {
                operatingMode = value;
            }
        }
       

        #region IAccess implementation

        public override bool HasProperty(ModelCode t)
        {
            switch(t)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    return true;
                default:
                    return base.HasProperty(t);
            }
            
        }

        public override void GetProperty(Property prop)
        {
            switch(prop.Id)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                    prop.SetValue((short)FuelType);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    prop.SetValue(MaxQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    prop.SetValue(MinQ);
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    prop.SetValue((short)OperatingMode);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
            
        }

        public override void SetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.SYNCHRONOUSMACHINE_FUELTYPE:
                    FuelType = (EmsFuelType)property.AsEnum();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MAXQ:
                    MaxQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_MINQ:
                    MinQ = property.AsFloat();
                    break;

                case ModelCode.SYNCHRONOUSMACHINE_OPERATINGMODE:
                    OperatingMode = (SynchronousMachineOperatingMode)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
            
        }

        #endregion IAccess implementation

        #region IReference implementation

        //koliko znam ovde ne treba nista, ali neka cr proveri

        #endregion IReference implementation
    }
}
