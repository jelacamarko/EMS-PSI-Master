﻿//-----------------------------------------------------------------------
// <copyright file="RegulatingCondEq.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.NetworkModelService.DataModel.Wires
{
    using System;
    using System.Collections.Generic;
    using EMS.Common;
    using EMS.Services.NetworkModelService.DataModel.Core;

    /// <summary>
    /// RegulatingCondEq class
    /// </summary>
    public class RegulatingCondEq : ConductingEquipment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegulatingCondEq" /> class
        /// </summary>
        /// <param name="globalId">globalId of the entity</param>
        public RegulatingCondEq(long globalId) : base(globalId)
        {
        }

        /// <summary>
        /// Chechs are the entities equals
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <returns>indicator of equality</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns hash code of the entity
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IAccess implementation

        /// <summary>
        /// Checks if the entity has a property
        /// </summary>
        /// <param name="property">model code of property</param>
        /// <returns>indicator of has property</returns>
        public override bool HasProperty(ModelCode property)
        {
            return base.HasProperty(property);
        }

        /// <summary>
        /// Gets the property
        /// </summary>
        /// <param name="property">property to get</param>
        public override void GetProperty(Property property)
        {
            base.GetProperty(property);
        }

        /// <summary>
        /// Sets the property
        /// </summary>
        /// <param name="property">property to set</param>
        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }

        #endregion IAccess implementation

        #region IReference implementation

        #endregion IReference implementation
    }
}
