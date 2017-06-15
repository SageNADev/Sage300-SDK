// The MIT License (MIT) 
// Copyright (c) 1994-2017 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#region Namespace

using System;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers
{
    /// <summary>
    /// Class for SegmentCodes mapping
    /// </summary>
    /// <typeparam name="T">SegmentCodes</typeparam>
    public class SegmentCodesMapper<T> : ModelMapper<T> where T : SegmentCodes, new ()
    {
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public SegmentCodesMapper(Context context)
            : base(context)
        {
        }

        #endregion

        #region ModelMapper methods

        /// <summary>
        /// Get Mapper
        /// </summary>
        /// <param name="entity">Business Entity</param>
        /// <returns>Mapped Model</returns>
        public override T Map(IBusinessEntity entity)
        {
            var model = base.Map(entity);

            //model.SegmentNumber = EnumUtility.GetEnum<SegmentNumber>(entity.GetValue<string>(SegmentCodes.Index.SegmentNumber));
            model.SegmentNumber = entity.GetValue<string>(SegmentCodes.Index.SegmentNumber);
            model.SegmentCode = entity.GetValue<string>(SegmentCodes.Index.SegmentCode);
            model.Description = entity.GetValue<string>(SegmentCodes.Index.Description);
            return model;
        }

        /// <summary>
        /// Set Mapper
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void Map(T model, IBusinessEntity entity)
        {
            if (model == null)
            {
                return;
            }

            entity.SetValue(SegmentCodes.Index.SegmentNumber, model.SegmentNumber);
            entity.SetValue(SegmentCodes.Index.SegmentCode, model.SegmentCode);
            entity.SetValue(SegmentCodes.Index.Description, model.Description);
        }

        /// <summary>
        /// Map Key
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        public override void MapKey(T model, IBusinessEntity entity)
        {
            entity.SetValue(SegmentCodes.Index.SegmentNumber, model.SegmentNumber);
            entity.SetValue(SegmentCodes.Index.SegmentCode, model.SegmentCode);
        }

        #endregion
    }
}