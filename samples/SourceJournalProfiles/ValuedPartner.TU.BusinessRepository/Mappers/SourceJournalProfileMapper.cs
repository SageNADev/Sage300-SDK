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

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers
{
    /// <summary>
    /// Class for SourceJournalProfile mapping
    /// </summary>
    /// <typeparam name="T">SourceJournalProfile</typeparam>
    public class SourceJournalProfileMapper<T> : ModelMapper<T> where T : SourceJournalProfile, new ()
    {
        #region Constructor

        /// <summary>
        /// Constructor to set the Context
        /// </summary>
        /// <param name="context">Context</param>
        public SourceJournalProfileMapper(Context context)
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

            model.SourceJournalName = entity.GetValue<string>(SourceJournalProfile.Index.SourceJournalName);
            model.SourceCodeID01 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID01);
            model.SourceType01 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType01);
            model.SourceCodeID02 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID02);
            model.SourceType02 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType02);
            model.SourceCodeID03 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID03);
            model.SourceType03 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType03);
            model.SourceCodeID04 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID04);
            model.SourceType04 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType04);
            model.SourceCodeID05 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID05);
            model.SourceType05 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType05);
            model.SourceCodeID06 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID06);
            model.SourceType06 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType06);
            model.SourceCodeID07 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID07);
            model.SourceType07 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType07);
            model.SourceCodeID08 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID08);
            model.SourceType08 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType08);
            model.SourceCodeID09 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID09);
            model.SourceType09 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType09);
            model.SourceCodeID10 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID10);
            model.SourceType10 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType10);
            model.SourceCodeID11 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID11);
            model.SourceType11 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType11);
            model.SourceCodeID12 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID12);
            model.SourceType12 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType12);
            model.SourceCodeID13 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID13);
            model.SourceType13 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType13);
            model.SourceCodeID14 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID14);
            model.SourceType14 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType14);
            model.SourceCodeID15 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID15);
            model.SourceType15 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType15);
            model.SourceCodeID16 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID16);
            model.SourceType16 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType16);
            model.SourceCodeID17 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID17);
            model.SourceType17 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType17);
            model.SourceCodeID18 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID18);
            model.SourceType18 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType18);
            model.SourceCodeID19 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID19);
            model.SourceType19 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType19);
            model.SourceCodeID20 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID20);
            model.SourceType20 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType20);
            model.SourceCodeID21 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID21);
            model.SourceType21 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType21);
            model.SourceCodeID22 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID22);
            model.SourceType22 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType22);
            model.SourceCodeID23 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID23);
            model.SourceType23 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType23);
            model.SourceCodeID24 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID24);
            model.SourceType24 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType24);
            model.SourceCodeID25 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID25);
            model.SourceType25 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType25);
            model.SourceCodeID26 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID26);
            model.SourceType26 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType26);
            model.SourceCodeID27 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID27);
            model.SourceType27 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType27);
            model.SourceCodeID28 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID28);
            model.SourceType28 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType28);
            model.SourceCodeID29 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID29);
            model.SourceType29 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType29);
            model.SourceCodeID30 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID30);
            model.SourceType30 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType30);
            model.SourceCodeID31 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID31);
            model.SourceType31 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType31);
            model.SourceCodeID32 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID32);
            model.SourceType32 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType32);
            model.SourceCodeID33 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID33);
            model.SourceType33 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType33);
            model.SourceCodeID34 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID34);
            model.SourceType34 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType34);
            model.SourceCodeID35 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID35);
            model.SourceType35 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType35);
            model.SourceCodeID36 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID36);
            model.SourceType36 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType36);
            model.SourceCodeID37 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID37);
            model.SourceType37 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType37);
            model.SourceCodeID38 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID38);
            model.SourceType38 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType38);
            model.SourceCodeID39 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID39);
            model.SourceType39 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType39);
            model.SourceCodeID40 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID40);
            model.SourceType40 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType40);
            model.SourceCodeID41 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID41);
            model.SourceType41 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType41);
            model.SourceCodeID42 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID42);
            model.SourceType42 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType42);
            model.SourceCodeID43 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID43);
            model.SourceType43 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType43);
            model.SourceCodeID44 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID44);
            model.SourceType44 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType44);
            model.SourceCodeID45 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID45);
            model.SourceType45 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType45);
            model.SourceCodeID46 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID46);
            model.SourceType46 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType46);
            model.SourceCodeID47 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID47);
            model.SourceType47 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType47);
            model.SourceCodeID48 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID48);
            model.SourceType48 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType48);
            model.SourceCodeID49 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID49);
            model.SourceType49 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType49);
            model.SourceCodeID50 = entity.GetValue<string>(SourceJournalProfile.Index.SourceCodeID50);
            model.SourceType50 = entity.GetValue<string>(SourceJournalProfile.Index.SourceType50);
            return model;
        }

        /// <summary>
        /// Set Mapper
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Map(T model, IBusinessEntity entity)
        {
            if (model == null)
            {
                return;
            }

            entity.SetValue(SourceJournalProfile.Index.SourceJournalName, model.SourceJournalName);
            entity.SetValue(SourceJournalProfile.Index.SourceCodeID01, model.SourceCodeID01);
            entity.SetValue(SourceJournalProfile.Index.SourceType01, model.SourceType01);
            entity.SetValue(SourceJournalProfile.Index.SourceCodeID02, model.SourceCodeID02);
            entity.SetValue(SourceJournalProfile.Index.SourceType02, model.SourceType02);
            entity.SetValue(SourceJournalProfile.Index.SourceCodeID03, model.SourceCodeID03);
            entity.SetValue(SourceJournalProfile.Index.SourceType03, model.SourceType03);
            entity.SetValue(SourceJournalProfile.Index.SourceCodeID04, model.SourceCodeID04);
            entity.SetValue(SourceJournalProfile.Index.SourceType04, model.SourceType04);
        }

        /// <summary>
        /// Map Key
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="entity">Business Entity</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapKey(T model, IBusinessEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}