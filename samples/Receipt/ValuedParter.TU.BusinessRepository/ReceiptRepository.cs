// The MIT License (MIT) 
// Copyright (c) 1994-2016 Sage Software, Inc.  All rights reserved.
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

#region namespaces

using ACCPAC.Advantage;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base.Statefull;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using ValuedParter.TU.BusinessRepository.Mappers;
using ValuedParter.TU.Interfaces.BusinessRepository;
using ValuedParter.TU.Models;
using ValuedParter.TU.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ICModel = Sage.CA.SBS.ERP.Sage300.IC.Models;

#endregion

namespace ValuedParter.TU.BusinessRepository
{
    /// <summary>
    /// Repository for Receipt
    /// </summary>
    /// <typeparam name="T">ReceiptHeader</typeparam>
    /// <typeparam name="TU">ReceiptDetail</typeparam>
    /// <typeparam name="TDetail2">ReceiptOptionalField</typeparam>
    /// <typeparam name="TDetail3">ReceiptDetailOptionalField</typeparam>
    /// <typeparam name="TDetail4">ReceiptDetailLotNumber</typeparam>
    /// <typeparam name="TDetail5">ReceiptDetailSerialNumber</typeparam>
    public class ReceiptRepository<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> : SequencedHeaderDetailFiveRepository<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>,
        IReceiptEntity<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>
        where T : ReceiptHeader, new()
        where TU : ReceiptDetail, new()
        where TDetail2 : ReceiptOptionalField, new()
        where TDetail3 : ReceiptDetailOptionalField, new()
        where TDetail4 : ReceiptDetailLotNumber, new()
        where TDetail5 : ReceiptDetailSerialNumber, new()
    {
        #region Business Entity Variables

        /// <summary>
        /// IC0590 - Inventory Control, Receipt
        /// </summary>
        private IBusinessEntity _receiptEntity;

        /// <summary>
        /// IC0580 - Inventory Control, Receipt Details
        /// </summary>
        private IBusinessEntity _receiptDetailEntity;

        /// <summary>
        /// IC0645 - Inventory Control, Receipt Optional Field
        /// </summary>
        private IBusinessEntity _receiptOptionalFieldEntity;

        /// <summary>
        /// IC0585 - Inventory Control, Receipt Details Optional Field
        /// </summary>
        private IBusinessEntity _receiptDetailOptionalFieldEntity;

        /// <summary>
        /// IC0582 - Inventory Control, Receipt Details Lot Number
        /// </summary>
        private IBusinessEntity _receiptDetailLotNumberEntity;

        /// <summary>
        /// IC0587 - Inventory Control, Receipt Details Serial Number
        /// </summary>
        private IBusinessEntity _receiptDetailSerialNumberEntity;

        /// <summary>
        /// IC0210 - Inventory Control, Category
        /// </summary>
        private IBusinessEntity _categoryEntity;

        /// <summary>
        /// IC0290 - Inventory Control, LocationDetail
        /// </summary>
        private IBusinessEntity _locationDetailEntity;

        /// <summary>
        /// IC0310 - Inventory Control, Item
        /// </summary>
        private IBusinessEntity _itemEntity;

        /// <summary>
        /// IC0370 - Inventory Control, Locations
        /// </summary>
        private IBusinessEntity _locationsEntity;

        /// <summary>
        /// IC0750 - Inventory Control, IitemUnitOfMeasure
        /// </summary>
        private IBusinessEntity _itemUnitOfMeasureEntity;

        /// <summary>
        /// Gets or sets the detail filter.
        /// </summary>
        /// <value>The detail filter.</value>
        public Expression<Func<TU, bool>> DetailFilter { get; set; }

        #endregion

        #region Private Variables

        /// <summary>
        /// Receipt Mapper
        /// </summary>
        private readonly ReceiptHeaderMapper<T> _receiptMapper;

        /// <summary>
        /// Receipt Detail Mapper
        /// </summary>
        private readonly ReceiptDetailMapper<TU> _receiptDetailMapper;

        /// <summary>
        /// Receipt Optional Field Mapper
        /// </summary>
        private readonly ReceiptOptionalFieldMapper<TDetail2> _receiptOptionalFieldMapper;

        /// <summary>
        /// Receipt Detail Optional Field Mapper
        /// </summary>
        private readonly ReceiptDetailOptionalFieldMapper<TDetail3> _receiptDetailOptionalFieldMapper;

        /// <summary>
        /// Constant to detail
        /// </summary>
        private const int VerifyDetail = 1001;

        #endregion

        #region Constructors

        /// <summary>
        /// Sets Context and DBLink
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptRepository(Context context)
            : base(context, new ReceiptMapper<T>(context), null, BusinessEntitySessionParams.Get(context))
        {
            SetValidRecordFilter();
            _receiptMapper = new ReceiptHeaderMapper<T>(context);
            _receiptDetailMapper = new ReceiptDetailMapper<TU>(context);
            _receiptOptionalFieldMapper = new ReceiptOptionalFieldMapper<TDetail2>(context);
            _receiptDetailOptionalFieldMapper = new ReceiptDetailOptionalFieldMapper<TDetail3>(context);
        }

        /// <summary>
        /// Sets Context and Session.
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="session">Session</param>
        public ReceiptRepository(Context context, IBusinessEntitySession session)
            : base(context, new ReceiptMapper<T>(context), session)
        {
            SetValidRecordFilter();
            _receiptMapper = new ReceiptHeaderMapper<T>(context);
            _receiptDetailMapper = new ReceiptDetailMapper<TU>(context);
            _receiptOptionalFieldMapper = new ReceiptOptionalFieldMapper<TDetail2>(context);
            _receiptDetailOptionalFieldMapper = new ReceiptDetailOptionalFieldMapper<TDetail3>(context);
        }

        /// <summary>
        /// Creates the business entities.
        /// </summary>
        /// <returns>SequencedHeaderDetailFiveBusinessEntitySet.</returns>
        protected override SequencedHeaderDetailFiveBusinessEntitySet<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> CreateBusinessEntities()
        {
            return CreateBusinessEntitiesInternal();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Get receipt
        /// </summary>
        /// <param name="id">Receipt Id</param>
        /// <returns>Receipt</returns>
        public override T GetById<TKey>(TKey id)
        {
            CheckRights(_receiptEntity, SecurityType.Inquire);
            var receiptMapper = new ReceiptMapper<T>(Context);
            var receiptNumber = id.ToString();

            if (!string.IsNullOrEmpty(receiptNumber))
            {
                _receiptEntity.Order = 2;
                _receiptEntity.SetValue(ReceiptHeader.Fields.ReceiptNumber, receiptNumber, true);
            }

            if (!string.IsNullOrEmpty(receiptNumber))
            {
                if (!_receiptEntity.Read(false))
                {
                    _receiptEntity.ClearRecord();
                    return
                        receiptMapper.Map(
                            new List<IBusinessEntity>
                            {
                                _receiptEntity,
                                _receiptDetailEntity,
                                _receiptOptionalFieldEntity,
                                _receiptDetailOptionalFieldEntity,
                                _receiptDetailSerialNumberEntity,
                                _receiptDetailLotNumberEntity
                            }, 0, 10);
                }
            }
            //Setting Receipt Mode 
            var receiptMode = ReceiptType.Receipt;
            if (_receiptEntity.Exists)
            {
                var completed = (Complete)(_receiptEntity.GetValue<int>(ReceiptHeader.Index.Complete));
                var status = (RecordStatus)(_receiptEntity.GetValue<int>(ReceiptHeader.Index.RecordStatus));

                if (completed == Complete.Yes)
                {
                    receiptMode = ReceiptType.Complete;
                }
                else if (status == RecordStatus.Entered)
                {
                    receiptMode = (ReceiptType)(_receiptEntity.GetValue<int>(ReceiptHeader.Index.ReceiptType));
                }
                else
                {
                    receiptMode = ReceiptType.Return;
                }
                _receiptEntity.SetValue(ReceiptHeader.Fields.ReceiptType, receiptMode);
            }
            else
            {
                _receiptEntity.SetValue(ReceiptHeader.Fields.ReceiptType, receiptMode);
            }
            // End - Setting Receipt Mode 

            var headerModel =
                receiptMapper.Map(
                    new List<IBusinessEntity>
                    {
                        _receiptEntity,
                        _receiptDetailEntity,
                        _receiptOptionalFieldEntity,
                        _receiptDetailOptionalFieldEntity,
                        _receiptDetailSerialNumberEntity,
                        _receiptDetailLotNumberEntity
                    }, 0, 10);
            //Setting Home Currency 
            headerModel.HomeCurrency = Session.HomeCurrency;
            return headerModel;
        }

        /// <summary>
        ///Posts the Receipt based on primary key,Sequence Number
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="filter">filter</param>
        /// <param name="yesNo">yesNo</param>
        /// <returns>Model</returns>
        public virtual T Post(T model, Expression<Func<T, bool>> filter, bool yesNo)
        {
            CheckRights(GetAccessRights(), SecurityType.Delete);
            model.RecordStatus = RecordStatus.Posted;
            if (yesNo)
            {
                model.Complete = Complete.Yes;
                model.RecordDeleted = RecordDeleted.Yes;
            }
            if (model.ReceiptType == ReceiptType.Complete)
            {
                model.Complete = Complete.Yes;
            }

            var header = Save(model);
            var receiptMapper = new ReceiptMapper<T>(Context);
            var headerModel = receiptMapper.Map(new List<IBusinessEntity> { _receiptEntity, _receiptDetailEntity });
            headerModel.Warnings = header.Warnings;
            headerModel.HomeCurrency = Session.HomeCurrency;
            return headerModel;
        }

        /// <summary>
        /// Gets the Details
        /// </summary>
        /// <param name="pageNumber">Current Page Number</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="filter">Filters, if any</param>
        /// <param name="orderBy">Order By, if any</param>
        /// <returns>List of details</returns>
        public override EnumerableResponse<TU> GetDetail(int pageNumber, int pageSize,
            Expression<Func<TU, bool>> filter = null, OrderBy orderBy = null)
        {
            var resultsCount = SetFilter(_receiptDetailEntity, filter, null, orderBy);

            if (_receiptDetailEntity.Fetch(false))
            {
                return new EnumerableResponse<TU>
                {
                    Items = MapDataToModel(_receiptDetailEntity, _receiptDetailMapper, pageNumber, pageSize, resultsCount),
                    TotalResultsCount = GetTotalRecords(_receiptDetailEntity)
                };
            }
            return new EnumerableResponse<TU> { Items = new List<TU>(), TotalResultsCount = 0 };
        }

        /// <summary>
        /// Creates a new Receipt Header.
        /// </summary>
        /// <returns>Receipt Header Model</returns>
        public override T NewHeader()
        {
            CheckRights(_receiptEntity, SecurityType.Inquire);
            CreateBusinessEntities();
            var receiptMapper = new ReceiptMapper<T>(Context);

            _receiptEntity.Order = 0;
            _receiptEntity.SetValue(ReceiptHeader.Fields.SequenceNumber, 0);
            _receiptEntity.Init();
            _receiptDetailEntity.ClearRecord();
            _receiptEntity.Order = 2;

            _receiptDetailOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
            _receiptEntity.Read(false);

            //Pull the default optional fields 
            _receiptEntity.SetValue(ReceiptHeader.Fields.ProcessCommand, ProcessCommand.InsertOptionalFields);
            _receiptEntity.Process();
            var headerModel = receiptMapper.Map(new List<IBusinessEntity> { _receiptEntity, _receiptDetailEntity, _receiptOptionalFieldEntity, _receiptDetailOptionalFieldEntity, _receiptDetailSerialNumberEntity, _receiptDetailLotNumberEntity }, 0, 10);

            headerModel.RecordStatus = RecordStatus.Entered;
            headerModel.Complete = Complete.No;
            headerModel.RecordPrinted = RecordPrinted.No;

            //Setting Home Currency 
            headerModel.HomeCurrency = Session.HomeCurrency;
            headerModel.Warnings = Helper.GetExceptions(Session);
            return headerModel;
        }

        /// <summary>
        /// Method to Add Receipts.
        /// </summary>
        /// <param name="model">Receipt Model</param>
        /// <returns>Receipt Header Model</returns>
        public override T Add(T model)
        {
            CheckRights(_receiptEntity, SecurityType.Add);
            CreateBusinessEntities();

            var receiptMapper = new ReceiptMapper<T>(Context);
            _receiptMapper.Map(model, _receiptEntity);

            var details = GetDetailModel(model);
            var details2 = GetDetail2Model(model).ToList();
            var details3 = GetDetail3Model(model);

            if (!_receiptEntity.Exists)
            {
                SyncDetails3(details3);
                SyncDetails2(details2);
                SyncDetails(details);

                if (!details.Any() && model.ReceiptDetail.TotalResultsCount == 0)
                {
                    _receiptDetailEntity.ClearRecord();
                    _receiptDetailEntity.Verify();
                }

                _receiptEntity.Verify();
                _receiptEntity.Insert();
            }
            else
            {
                var entityErrors = new List<EntityError>
                {
                    new EntityError
                    {
                        Message = string.Format(CommonResx.UpdateFailedMessage, CommonResx.HeaderEntity),
                        Priority = Priority.Error
                    }
                };
                throw ExceptionHelper.BusinessException(entityErrors);
            }

            //If more than one detail items are retrieved then move the pointer to Top of the detail entity 
            model = receiptMapper.Map(new List<IBusinessEntity> { _receiptEntity, _receiptDetailEntity });
            model.Warnings = Helper.GetExceptions(Session);
            return model;
        }

        /// <summary>
        /// Method to Save Receipts.
        /// </summary>
        /// <param name="model">Receipts Model</param>
        /// <returns>Receipt Header Model</returns>
        public override T Save(T model)
        {
            IsSessionAvailable();
            CheckRights(GetAccessRights(), SecurityType.Modify);
            var receiptMapper = new ReceiptMapper<T>(Context);
            var header = model;
            if (header.RecordStatus != RecordStatus.Posted)
            {
                header.RecordStatus = RecordStatus.Entered;
            }
            _receiptMapper.Map(header, _receiptEntity);

            var details = GetDetailModel(header).ToList();
            var details2 = GetDetail2Model(model).ToList();
            var details3 = GetDetail3Model(model);

            SyncDetails3(details3);
            SyncDetails2(details2);
            SyncDetails(details);

            if (!details.Any() && header.ReceiptDetail.TotalResultsCount == 0)
            {
                _receiptDetailEntity.ClearRecord();
                _receiptDetailEntity.Verify();
            }
            _receiptEntity.Verify();

            if (_receiptEntity.Exists)
            {
                _receiptEntity.Update();
            }
            else
            {
                _receiptEntity.Insert();
            }
            var headerModel = receiptMapper.Map(new List<IBusinessEntity> { _receiptEntity, _receiptDetailEntity });
            headerModel.Warnings = Helper.GetExceptions(Session);
            //Setting Home Currency 
            headerModel.HomeCurrency = Session.HomeCurrency;
            return headerModel;
        }

        /// <summary>
        ///Delete the Receipt based on primary key.
        /// </summary>
        /// <param name="filter">filter</param>
        /// <returns>Model</returns>
        public override T Delete(Expression<Func<T, bool>> filter)
        {
            CheckRights(_receiptEntity, SecurityType.Delete);
            CreateBusinessEntities();
            if (_receiptEntity.Read(false)) // Check if the record exists and then delete.
            {
                if (Search(_receiptEntity, filter))
                {
                    _receiptEntity.Delete();
                }
            }
            return _receiptMapper.Map(_receiptEntity);
        }

        /// <summary>
        /// Get the Detail Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt Detail Model</returns>
        protected override IEnumerable<TU> GetDetailModel(T header)
        {
            var receiptDetail = header.ReceiptDetail;
            if (receiptDetail != null && receiptDetail.Items != null && receiptDetail.Items.Any())
                return receiptDetail.Items.Count() != 0 ? header.ReceiptDetail.Items.Cast<TU>().ToList() : null;
            return new List<TU>();
        }

        /// <summary>
        /// Get the Detail 2 Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt Detail 2</returns>
        protected override IEnumerable<TDetail2> GetDetail2Model(T header)
        {
            var receiptOptionalField = header.ReceiptOptionalField;
            if (receiptOptionalField != null && receiptOptionalField.Items != null && receiptOptionalField.Items.Any())
                return receiptOptionalField.Items.Count() != 0 ? header.ReceiptOptionalField.Items.Cast<TDetail2>().ToList() : null;
            return new List<TDetail2>();
        }

        /// <summary>
        /// Get the Detail 3 Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt Detail 3</returns>
        protected override IEnumerable<TDetail3> GetDetail3Model(T header)
        {
            var receiptDetailOptionalField = header.ReceiptDetailOptionalField;
            if (receiptDetailOptionalField != null && receiptDetailOptionalField.Items != null && receiptDetailOptionalField.Items.Any())
                return receiptDetailOptionalField.Items.Count() != 0 ? header.ReceiptDetailOptionalField.Items.Cast<TDetail3>().ToList() : null;
            return new List<TDetail3>();
        }

        /// <summary>
        /// Get the Detail 4 Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt Detail 4</returns>
        protected override IEnumerable<TDetail4> GetDetail4Model(T header)
        {
            var receiptDetailLotNumber = header.ReceiptDetailLotNumber;
            if (receiptDetailLotNumber != null && receiptDetailLotNumber.Items != null && receiptDetailLotNumber.Items.Any())
                return receiptDetailLotNumber.Items.Count() != 0 ? header.ReceiptDetailLotNumber.Items.Cast<TDetail4>().ToList() : null;
            return new List<TDetail4>();
        }

        /// <summary>
        /// Get the Detail 5 Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt detail 5</returns>
        protected override IEnumerable<TDetail5> GetDetail5Model(T header)
        {
            var receiptDetailSerialNumber = header.ReceiptDetailSerialNumber;
            if (receiptDetailSerialNumber != null && receiptDetailSerialNumber.Items != null && receiptDetailSerialNumber.Items.Any())
                return receiptDetailSerialNumber.Items.Count() != 0
                    ? header.ReceiptDetailSerialNumber.Items.Cast<TDetail5>().ToList()
                    : null;
            return new List<TDetail5>();
        }

        /// <summary>
        /// Map for process detail
        /// </summary>
        /// <param name="detail">Receipt Detail</param>
        /// <param name="detailEntity">Business Entity</param>
        protected override void ProcessMap(TU detail, IBusinessEntity detailEntity)
        {
            detailEntity.SetValue(ReceiptDetail.Index.SequenceNumber, detail.SequenceNumber, true);
            detailEntity.SetValue(ReceiptDetail.Index.LineNumber, detail.LineNumber, true);
        }

        /// <summary>
        /// Map for process detail 2
        /// </summary>
        /// <param name="detail2">Receipt Detail2</param>
        /// <param name="detail2Entity">Business Entity</param>
        protected override void ProcessMap2(TDetail2 detail2, IBusinessEntity detail2Entity)
        {
            detail2Entity.SetValue(ReceiptOptionalField.Index.SequenceNumber, detail2.SequenceNumber, true);
            detail2Entity.SetValue(ReceiptOptionalField.Index.OptionalField, detail2.OptionalField, true);
        }

        /// <summary>
        /// Map for process detail 3
        /// </summary>
        /// <param name="detail3">Receipt Detail3</param>
        /// <param name="detail3Entity">Business Entity</param>
        protected override void ProcessMap3(TDetail3 detail3, IBusinessEntity detail3Entity)
        {
            detail3Entity.SetValue(ReceiptDetailOptionalField.Index.SequenceNumber, detail3.SequenceNumber, true);
            detail3Entity.SetValue(ReceiptDetailOptionalField.Index.LineNumber, detail3.LineNumber, true);
            detail3Entity.SetValue(ReceiptDetailOptionalField.Index.OptionalField, detail3.OptionalField, true);
        }

        /// <summary>
        /// Map for process detail 4
        /// </summary>
        /// <param name="detail4">Receipt Detail4</param>
        /// <param name="detail4Entity">Business Entity</param>
        protected override void ProcessMap4(TDetail4 detail4, IBusinessEntity detail4Entity)
        {
            detail4Entity.SetValue(ReceiptDetailLotNumber.Index.SequenceNumber, detail4.SequenceNumber, true);
            detail4Entity.SetValue(ReceiptDetailLotNumber.Index.LineNumber, detail4.LineNumber, true);
            detail4Entity.SetValue(ReceiptDetailLotNumber.Index.LotNumber, detail4.LotNumber, true);
        }

        /// <summary>
        /// Map for process detail 5
        /// </summary>
        /// <param name="detail5">Receipt Detail5</param>
        /// <param name="detail5Entity">Business Entity</param>
        protected override void ProcessMap5(TDetail5 detail5, IBusinessEntity detail5Entity)
        {
            detail5Entity.SetValue(ReceiptDetailSerialNumber.Index.SequenceNumber, detail5.SequenceNumber, true);
            detail5Entity.SetValue(ReceiptDetailSerialNumber.Index.LineNumber, detail5.LineNumber, true);
            detail5Entity.SetValue(ReceiptDetailSerialNumber.Index.SerialNumber, detail5.SerialNumber, true);
        }

        /// <summary>
        /// Get Details count from Header
        /// </summary>
        /// <param name="headerEntity">Header Entity</param>
        /// <returns>Number of details</returns>
        protected override int GetDetailCount(IBusinessEntity headerEntity)
        {
            return 0;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets ValidRecord Filter
        /// </summary>
        private void SetValidRecordFilter()
        {
            ValidRecordFilter = seq => seq.SequenceNumber != 0;
        }

        /// <summary>
        /// Method to Detail Process
        /// </summary>
        private void DetailProcess()
        {
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.ProcessCommand, ProcessCommand.InsertOptionalFields, true);
            _receiptDetailEntity.Process();
        }


        /// <summary>
        /// Method to Location Process
        /// </summary>
        /// <param name="model"></param>
        private void LocationProcess(TU model)
        {
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.Location, model.Location, true);
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.CheckBelowZero, true);
            _receiptDetailEntity.Process();
            _receiptDetailOptionalFieldEntity.ClearRecord();
            _receiptDetailOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
            _receiptEntity.SetValue(ReceiptHeader.Index.RecordStatus, RecordStatus.Entered, true);
        }

        /// <summary>
        /// Method to Item Process
        /// </summary>
        /// <param name="model"></param>
        private void ItemProcess(TU model)
        {
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.ItemNumber, model.ItemNumber, true);
            _receiptDetailEntity.Process();
        }

        /// <summary>
        /// Synchronizes the detail.
        /// </summary>
        /// <param name="details">The details.</param>
        private void SyncDetails(IEnumerable<TU> details)
        {
            if (details == null) return;

            var allDetails = details as IList<TU> ?? details.ToList();
            var newLine = allDetails.FirstOrDefault(detail => detail.IsNewLine);

            if (newLine != null )
            {
                InsertDetail(newLine);
            }
            foreach ( var detail in allDetails.Where(detail => detail.HasChanged || detail.IsDeleted).Where(detail => detail != newLine))
            {
                SyncDetail(detail);
            }
        }

        /// <summary>
        /// Synchronizes the detail 2.
        /// </summary>
        /// <param name="details2">The details.</param>
        private void SyncDetails2(IEnumerable<TDetail2> details2)
        {
            if (details2 == null) return;

            var allDetails = details2 as IList<TDetail2> ?? details2.ToList();
            var newLine2 = allDetails.FirstOrDefault(detail => detail.IsNewLine);

            if (newLine2 != null)
            {
                InsertDetail2(newLine2);
            }
            foreach (var detail2 in allDetails.Where(detail2 => detail2.HasChanged || detail2.IsDeleted).Where(detail2 => detail2 != newLine2))
            {
                SyncDetail2(detail2);
            }
        }

        /// <summary>
        /// Synchronizes the detail 3.
        /// </summary>
        /// <param name="details3">The details.</param>
        private void SyncDetails3(IEnumerable<TDetail3> details3)
        {
            if (details3 == null) return;

            var allDetails = details3 as IList<TDetail3> ?? details3.ToList();
            var newLine3 = allDetails.FirstOrDefault(detail => detail.IsNewLine);

            if (newLine3 != null)
            {
                InsertDetail3(newLine3);
            }
            foreach (var detail3 in allDetails.Where(detail3 => detail3.HasChanged || detail3.IsDeleted).Where(detail3 => detail3 != newLine3))
            {
                SyncDetail3(detail3);
            }
        }

        /// <summary>
        /// Sync Detail Records
        /// </summary>
        /// <param name="detail">Receipt Detail Model</param>
        public override bool SyncDetail(TU detail)
        {
            _receiptDetailMapper.MapKey(detail, _receiptDetailEntity);
            _receiptDetailEntity.Fetch(false);
            var recordExist = _receiptDetailEntity.Read(false);

            if (!recordExist && !detail.IsDeleted)
            {
                _receiptDetailEntity.Insert();
                detail.IsNewLine = false;
            }
            if (detail.IsDeleted && recordExist)
            {
                _receiptDetailEntity.Delete();
                _receiptDetailEntity.ClearRecord();
            }
            else if (detail.IsDeleted && !recordExist)
            {
                _receiptDetailEntity.ClearRecord();
            }
            else if (recordExist)
            {
                _receiptDetailMapper.Map(detail, _receiptDetailEntity);
                _receiptDetailEntity.Update();
            }

            return true;
        }

        /// <summary>
        /// Sync Detail 2 Records
        /// </summary>
        /// <param name="detail2">Receipt Optional Field Model</param>
        private void SyncDetail2(TDetail2 detail2)
        {
            _receiptOptionalFieldMapper.MapKey(detail2, _receiptOptionalFieldEntity);

            var recordExist = _receiptOptionalFieldEntity.Read(false);

            if (!recordExist && !detail2.IsDeleted)
            {
                _receiptOptionalFieldEntity.Insert();
                detail2.IsNewLine = false;
            }
            if (detail2.IsDeleted && recordExist)
            {
                _receiptOptionalFieldEntity.Delete();
            }
            else if (detail2.IsDeleted && !recordExist)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }
            else if (recordExist)
            {
                _receiptOptionalFieldMapper.Map(detail2, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Update();
                _receiptOptionalFieldMapper.MapKey(detail2, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Read(false);
            }
        }

        /// <summary>
        /// Sync Detail 3 Records
        /// </summary>
        /// <param name="detail3">Receipt Detail Optional Field</param>
        private void SyncDetail3(TDetail3 detail3)
        {
            _receiptDetailOptionalFieldMapper.MapKey(detail3, _receiptDetailOptionalFieldEntity);

            var recordExist = _receiptDetailOptionalFieldEntity.Read(false);

            if (!recordExist && !detail3.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Insert();

                detail3.IsNewLine = false;
            }

            if (detail3.IsDeleted && recordExist)
            {
                _receiptDetailOptionalFieldEntity.Delete();
            }
            else if (detail3.IsDeleted && !recordExist)
            {
                _receiptDetailOptionalFieldEntity.ClearRecord();
            }
            else if (recordExist)
            {
                _receiptDetailOptionalFieldMapper.Map(detail3, _receiptDetailOptionalFieldEntity);
                _receiptDetailOptionalFieldEntity.Update();
                _receiptDetailOptionalFieldMapper.MapKey(detail3, _receiptDetailOptionalFieldEntity);
                _receiptDetailOptionalFieldEntity.Read(false);
            }
        }


        /// <summary>
        /// Save Details
        /// </summary>
        /// <param name="details">Details</param>
        /// <returns>true</returns>
        public override bool SaveDetails(IEnumerable<TU> details)
        {
            IsSessionAvailable();
            SyncDetails(details);
            return true;
        }

        /// <summary>
        /// Insert Detail records
        /// </summary>
        /// <param name="newLine">Receipt Detail Model</param>
        private void InsertDetail(TU newLine)
        {
            _receiptDetailMapper.MapKey(newLine, _receiptDetailEntity);

            var recordExists = _receiptDetailEntity.Exists;

            if (!recordExists && newLine.IsDeleted)
            {
                _receiptDetailEntity.ClearRecord();
            }

            _receiptDetailMapper.Map(newLine, _receiptDetailEntity);

            if (!recordExists && !newLine.IsDeleted)
            {
                _receiptDetailEntity.Insert();
                newLine.IsNewLine = false;
            }
            else if (recordExists && !newLine.IsDeleted)
            {
                _receiptDetailEntity.Read(false);
                _receiptDetailEntity.Update();
            }
            else if (recordExists && newLine.IsDeleted)
            {
                _receiptDetailEntity.Read(false);
                _receiptDetailEntity.Delete();
            }
        }

        /// <summary>
        ///  Insert Detail 2 records
        /// </summary>
        /// <param name="newLine2">Receipt Optional Field</param>
        private void InsertDetail2(TDetail2 newLine2)
        {
            _receiptOptionalFieldMapper.Map(newLine2, _receiptOptionalFieldEntity);

            var recordExists = _receiptOptionalFieldEntity.Exists;

            if (!recordExists && newLine2.IsDeleted)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }

            if (!recordExists && !newLine2.IsDeleted)
            {
                _receiptOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
                _receiptOptionalFieldEntity.Insert();
                newLine2.IsNewLine = false;
            }
            else if (recordExists && !newLine2.IsDeleted)
            {
                _receiptOptionalFieldEntity.Read(false);
                _receiptOptionalFieldEntity.Update();
            }
            else if (recordExists && newLine2.IsDeleted)
            {
                _receiptOptionalFieldEntity.Read(false);
                _receiptOptionalFieldEntity.Delete();
            }
        }

        /// <summary>
        ///  Insert Detail 3 records
        /// </summary>
        /// <param name="newLine3">Receipt Detail Optional Field</param>
        private void InsertDetail3(TDetail3 newLine3)
        {
            _receiptDetailOptionalFieldMapper.Map(newLine3, _receiptDetailOptionalFieldEntity);
            var recordExists = _receiptDetailOptionalFieldEntity.Exists;

            if (!recordExists && newLine3.IsDeleted)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }

            if (!recordExists && !newLine3.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Insert();
                newLine3.IsNewLine = false;
            }
            else if (recordExists && !newLine3.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Read(false);
                _receiptDetailOptionalFieldEntity.Update();
            }
            else if (recordExists && newLine3.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Read(false);
                _receiptDetailOptionalFieldEntity.Delete();
            }
            _receiptDetailEntity.Update();
        }

        /// <summary>
        /// Synchronizes the detail.
        /// </summary>
        /// <param name="detailOptionalFields">Detail optional Fields</param>
        private void SyncOptionalFields(IEnumerable<TDetail2> detailOptionalFields)
        {
            if (detailOptionalFields == null)
            {
                //This is to update when there are no records and to cover AutoInsert error, if exists
                if (_receiptEntity.Exists)
                {
                    _receiptEntity.Update();
                }
                return;
            }

            var allDetails = detailOptionalFields as IList<TDetail2> ?? detailOptionalFields.ToList();

            foreach (var detail in allDetails.Where(detail => (detail.HasChanged || detail.IsDeleted) && !detail.IsNewLine))
            {
                SyncOptionalField(detail);
                if (_receiptEntity.Exists)
                {
                    _receiptEntity.Update();
                }

            }
            foreach (var newLine in allDetails.Where(detail => detail.IsNewLine && detail.OptionalField != null))
            {
                InsertOptionalField(newLine);
                if (_receiptEntity.Exists)
                {
                    _receiptEntity.Update();
                }
            }
        }

        /// <summary>
        /// Open and compose business entities
        /// </summary>
        /// <returns>Sequenced Header Detail Five Business Entity Set</returns>
        private SequencedHeaderDetailFiveBusinessEntitySet<T, TU, TDetail2, TDetail3, TDetail4, TDetail5> CreateBusinessEntitiesInternal()
        {
            _receiptEntity = OpenEntity(ReceiptHeader.EntityName, true);
            _receiptDetailEntity = OpenEntity(ReceiptDetail.EntityName, true);
            _receiptOptionalFieldEntity = OpenEntity(ReceiptOptionalField.EntityName, true);
            _receiptDetailOptionalFieldEntity = OpenEntity(ReceiptDetailOptionalField.EntityName, true);
            _receiptDetailLotNumberEntity = OpenEntity(ReceiptDetailLotNumber.EntityName, true);
            _receiptDetailSerialNumberEntity = OpenEntity(ReceiptDetailSerialNumber.EntityName, true);
            _locationsEntity = OpenEntity(ICModel.Locations.EntityName, true);
            _locationDetailEntity = OpenEntity(ICModel.LocationDetail.EntityName, true);
            _itemUnitOfMeasureEntity = OpenEntity(ICModel.ItemUnitOfMeasure.EntityName, true);
            _itemEntity = OpenEntity(ICModel.Item.EntityName, true);
            _categoryEntity = OpenEntity(ICModel.Category.EntityName, true);

            _receiptEntity.Compose(new[] { _receiptDetailEntity.View, _receiptOptionalFieldEntity.View });

            _receiptDetailEntity.Compose(new[] { 
                _receiptEntity.View, 
                _itemEntity.View, 
                _itemUnitOfMeasureEntity.View, 
                _categoryEntity.View, 
                _locationsEntity.View,
                _locationDetailEntity.View,
                _receiptDetailOptionalFieldEntity.View,
                _receiptDetailSerialNumberEntity.View,
                _receiptDetailLotNumberEntity.View
            });

            _receiptOptionalFieldEntity.Compose(new[] { _receiptEntity.View });
            _receiptDetailOptionalFieldEntity.Compose(new[] { _receiptDetailEntity.View });
            _receiptDetailLotNumberEntity.Compose(new[] { _receiptDetailEntity.View });
            _receiptDetailSerialNumberEntity.Compose(new[] { _receiptDetailEntity.View });

            var businessEntities = new SequencedHeaderDetailFiveBusinessEntitySet<T, TU, TDetail2, TDetail3, TDetail4, TDetail5>
            {
                HeaderBusinessEntity = _receiptEntity,
                HeaderMapper = new ReceiptHeaderMapper<T>(Context),
                DetailBusinessEntity = _receiptDetailEntity,
                DetailMapper = new ReceiptDetailMapper<TU>(Context),
                Detail2BusinessEntity = _receiptOptionalFieldEntity,
                Detail2Mapper = new ReceiptOptionalFieldMapper<TDetail2>(Context),
                Detail3BusinessEntity = _receiptDetailOptionalFieldEntity,
                Detail3Mapper = new ReceiptDetailOptionalFieldMapper<TDetail3>(Context),
                Detail4BusinessEntity = _receiptDetailLotNumberEntity,
                Detail4Mapper = new ReceiptDetailLotNumberMapper<TDetail4>(Context),
                Detail5BusinessEntity = _receiptDetailSerialNumberEntity,
                Detail5Mapper = new ReceiptDetailSerialNumberMapper<TDetail5>(Context),
            };
            return businessEntities;
        }

        /// <summary>
        /// To retrieve the List of Optional Fields
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="filterCount">The filter count.</param>
        /// <returns>List of receipt optional fields</returns>
        private EnumerableResponse<TDetail2> RetrieveOptionalField(int pageNumber,
            int pageSize, int filterCount)
        {
            var optionalFieldList = new List<TDetail2>();
            var optionalFieldItems = new EnumerableResponse<TDetail2>();
            var startIndex = CommonUtil.ComputeStartIndex(pageNumber, pageSize);
            var endIndex = CommonUtil.ComputeEndIndex(pageNumber, pageSize, filterCount);
            var loopCounter = 1;
            var lineNumber = 0;

            //Map detail entities based on the page number and page size
            _receiptOptionalFieldEntity.SetValue(ReceiptOptionalField.Index.OptionalField, string.Empty);
            _receiptOptionalFieldEntity.Browse(String.Empty, true);

            if (_receiptOptionalFieldEntity.Fetch(false))
            {
                do
                {
                    if (loopCounter >= startIndex)
                    {
                        var optionalFieldDetail = _receiptOptionalFieldMapper.Map(_receiptOptionalFieldEntity);
                        optionalFieldDetail.DisplayIndex = lineNumber++;
                        FormatOptionalFields(optionalFieldDetail);
                        optionalFieldList.Add(optionalFieldDetail);
                    }

                    loopCounter++;
                } while (loopCounter <= endIndex && _receiptOptionalFieldEntity.Next());
            }

            var totalRecords = filterCount != 0 ? filterCount : GetTotalRecords(_receiptOptionalFieldEntity);

            if (optionalFieldList.Any())
            {
                optionalFieldItems.Items = optionalFieldList;
                optionalFieldItems.TotalResultsCount = totalRecords;
            }

            return optionalFieldItems;
        }

        /// <summary>
        /// Format Optional Field
        /// </summary>
        /// <param name="optionalField">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        private TDetail2 FormatOptionalFields(TDetail2 optionalField)
        {
            if (optionalField == null) return null;
            if (optionalField.Type == Models.Enums.Type.Date)
            {
                optionalField.Value = DateUtil.GetShortDate(optionalField.Value, string.Empty, true);
            }

            if (optionalField.Type != Models.Enums.Type.Time) return optionalField;
            if (optionalField.Value != null)
            {
                optionalField.Value = Regex.Replace(optionalField.Value, @"(\w{2})(\w{2})(\w{2})", @"$1:$2:$3");
            }
            return optionalField;
        }

        /// <summary>
        /// Format Optional Field
        /// </summary>
        /// <param name="optionalField">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        private TDetail3 FormatDetailOptFields(TDetail3 optionalField)
        {
            if (optionalField.Type == Models.Enums.Type.Date)
            {
                optionalField.Value = DateUtil.GetShortDate(optionalField.Value, string.Empty, true);
            }

            if (optionalField.Type != Models.Enums.Type.Time) return optionalField;
            if (optionalField.Value != null)
            {
                optionalField.Value = Regex.Replace(optionalField.Value, @"(\w{2})(\w{2})(\w{2})", @"$1:$2:$3");
            }
            return optionalField;
        }

        /// <summary>
        /// To set fields for Detail Optional Fields
        /// </summary>
        /// <param name="optionalField">Optional Fields</param>
        private void SyncOptionalField(TDetail2 optionalField)
        {
            _receiptOptionalFieldMapper.MapKey(optionalField, _receiptOptionalFieldEntity);
            _receiptOptionalFieldEntity.Read(false);

            if (optionalField.IsDeleted && _receiptOptionalFieldEntity.Exists)
            {
                _receiptOptionalFieldEntity.Delete();
            }
            else if (optionalField.IsDeleted)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }
            else if (_receiptOptionalFieldEntity.Exists)
            {
                _receiptOptionalFieldMapper.MapKey(optionalField, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Read(false);
                _receiptOptionalFieldMapper.Map(optionalField, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Update();
            }
        }

        /// <summary>
        /// Insert new optional field
        /// </summary>
        /// <param name="newLine"></param>
        /// <returns></returns>
        private void InsertOptionalField(ReceiptOptionalField newLine)
        {
            //The reason for the exists check is that when an exception is thrown, new line will still be true.
            if (!newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
                _receiptOptionalFieldEntity.Insert();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to Get Item values data.
        /// </summary>
        /// <param name="detail">Receipt Detail Model</param>
        /// <param name="eventType">Event Type</param>
        /// <returns>Receipt Detail Model</returns>
        public virtual TU GetRowValues(TU detail, int eventType)
        {
            IsSessionAvailable();
            var detailsMapper = new ReceiptDetailMapper<TU>(Context);
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.SequenceNumber, detail.SequenceNumber);
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.LineNumber, detail.LineNumber);

            if (_receiptDetailEntity.Exists)
            {
                _receiptDetailEntity.Read(false);
            }

            switch (eventType)
            {
                case VerifyDetail:
                    _receiptDetailEntity.Verify();
                    _receiptDetailEntity.Process();
                    break;

                case ReceiptDetail.Index.ItemNumber:

                    ItemProcess(detail);

                    if (detail.Location != null)
                    {
                        LocationProcess(detail);
                    }
                    if (detail.QuantityReceived != 0)
                    {
                        _receiptDetailEntity.SetValue(ReceiptDetail.Index.QuantityReceived, detail.QuantityReceived, true);
                    }

                    break;

                case ReceiptDetail.Index.Location:

                    if (detail.ItemNumber != null)
                    {
                        ItemProcess(detail);
                    }
                    LocationProcess(detail);

                    if (detail.QuantityReceived != 0)
                    {
                        _receiptDetailEntity.SetValue(ReceiptDetail.Index.QuantityReceived, detail.QuantityReceived, true);
                    }
                    break;

                case ReceiptDetail.Index.QuantityReceived:

                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.QuantityReceived, detail.QuantityReceived, true);
                    break;

                case ReceiptDetail.Index.UnitCost:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.UnitCost, detail.UnitCost, true);
                    break;

                case ReceiptDetail.Index.ExtendedCost:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.ExtendedCost, detail.ExtendedCost, true);
                    break;

                case ReceiptDetail.Index.UnitOfMeasure:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.UnitOfMeasure, detail.UnitOfMeasure, true);
                    break;

                case ReceiptDetail.Index.QuantityReturned:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.QuantityReturned, detail.QuantityReturned, true);
                    break;

                case ReceiptDetail.Index.AdjustedUnitCost:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.AdjustedUnitCost, detail.AdjustedUnitCost, true);
                    break;

                case ReceiptDetail.Index.AdjustedCost:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.AdjustedCost, detail.AdjustedCost, true);
                    break;

                case ReceiptDetail.Index.Comments:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.Comments, detail.Comments, true);
                    break;

                case ReceiptDetail.Index.Labels:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.Labels, detail.Labels, true);
                    break;

                case ReceiptDetail.Index.ManufacturersItemNumber:
                    _receiptDetailEntity.SetValue(ReceiptDetail.Index.ManufacturersItemNumber, detail.ManufacturersItemNumber, true);
                    break;
            }

            if (_receiptDetailEntity.Exists)
            {
                _receiptDetailEntity.Update();
            }

            return detailsMapper.Map(_receiptDetailEntity);
        }

        /// <summary>
        /// Get Header Values
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="eventType">eventType</param>
        /// <returns>Receipt Header view Model</returns>
        public virtual T GetHeaderValues(T model, int eventType)
        {
            switch (eventType)
            {
                case ReceiptHeader.Index.ReceiptType:
                    _receiptEntity.SetValue(ReceiptHeader.Index.ReceiptType, model.ReceiptType, true);
                    break;

                case ReceiptHeader.Index.AdditionalCost:
                    _receiptEntity.SetValue(ReceiptHeader.Index.AdditionalCost, model.AdditionalCost, true);
                    break;

                case ReceiptHeader.Index.RequireLabels:
                    _receiptEntity.SetValue(ReceiptHeader.Index.RequireLabels, model.RequireLabels, true);
                    break;

                case ReceiptHeader.Index.ReceiptCurrency:
                    _receiptEntity.SetValue(ReceiptHeader.Index.ReceiptCurrency, model.ReceiptCurrency, true);
                    break;

                case ReceiptHeader.Index.AdditionalCostCurrency:
                    _receiptEntity.SetValue(ReceiptHeader.Index.AdditionalCostCurrency, model.AdditionalCostCurrency,
                        true);
                    break;

                case ReceiptHeader.Index.RateType:
                    _receiptEntity.SetValue(ReceiptHeader.Index.RateType, model.RateType, true);
                    break;

                case ReceiptHeader.Index.ExchangeRate:
                    _receiptEntity.SetValue(ReceiptHeader.Index.ExchangeRate, model.ExchangeRate, true);
                    break;

                case ReceiptHeader.Index.VendorNumber:
                    _receiptEntity.SetValue(ReceiptHeader.Index.VendorNumber, model.VendorNumber, true);
                    break;

            }
            var receiptHeaderMapper = new ReceiptHeaderMapper<T>(Context);
            var header = receiptHeaderMapper.Map(_receiptEntity);
            header.HomeCurrency = Session.HomeCurrency;
            header.Warnings = Helper.GetExceptions(Session);
            return header;

        }


        /// <summary>
        /// Gets Receipt Optional Field Details
        /// </summary>
        /// <param name="pageNumber">pageNumber</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="filter">filter</param>
        /// <returns>List of Optional Field Details</returns>
        public virtual EnumerableResponse<TDetail2> GetOptionalField(int pageNumber, int pageSize,
            Expression<Func<TDetail2, bool>> filter = null)
        {
            IsSessionAvailable();
            return RetrieveOptionalField(pageNumber, pageSize, 0);
        }

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">Optional Fields details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        public virtual EnumerableResponse<TDetail2> SaveOptionalFields(IEnumerable<TDetail2> optionalFieldDetails, string receiptNumber)
        {
            IsSessionAvailable();
            _receiptEntity.SetValue(ReceiptHeader.Index.ReceiptNumber, receiptNumber);
            _receiptEntity.Read(false);
            SyncOptionalFields(optionalFieldDetails);
            _receiptEntity.Update();
            return GetOptionalField(0, 10);
        }

        /// <summary>
        /// Refreshes the Detail
        /// </summary>
        /// <param name="detail">TU model</param>
        /// <param name="eventType">Property that changed</param>
        /// <returns>TU model</returns>
        public virtual T RefreshDetail(TU detail, string eventType)
        {
            IsSessionAvailable();
            _receiptDetailMapper.MapKey(detail, _receiptDetailEntity);
            if (_receiptDetailEntity.Exists)
            {
                _receiptDetailEntity.Process();
                _receiptDetailEntity.Update();
            }
            else
            {
                _receiptDetailEntity.Process();
            }
            var detailData = _receiptDetailMapper.Map(_receiptDetailEntity);
            detailData.DisplayIndex = detail.DisplayIndex;
            var headerData = _receiptMapper.Map(_receiptEntity);
            headerData.ReceiptDetail = new EnumerableResponse<ReceiptDetail>
            {
                Items = new List<TU> { detailData }
            };
            return headerData;
        }

        /// <summary>
        /// Refreshes the header when the optional field
        /// </summary>
        /// <returns>returns optional field value</returns>
        public virtual string RefreshOptField()
        {
            var optCount = _receiptEntity.GetValue<string>(ReceiptHeader.Fields.OptionalFields);
            return optCount;
        }

        #region OptionalFields

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        public virtual TDetail2 GetOptionalFieldFinderData(string optionalField)
        {
            IsSessionAvailable();
            _receiptOptionalFieldEntity.SetValue(ReceiptOptionalField.Index.OptionalField, optionalField.ToUpper(), true);
            var updatedOptionalField = _receiptOptionalFieldMapper.Map(_receiptOptionalFieldEntity);
            return FormatOptionalFields(updatedOptionalField);
        }

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Detail Optional Field model</returns>
        public virtual TDetail3 GetDetailOptFieldFinderData(string optionalField)
        {
            IsSessionAvailable();
            _receiptDetailOptionalFieldEntity.SetValue(ReceiptDetailOptionalField.Index.OptionalField, optionalField.ToUpper(), true);
            var updatedOptionalField = _receiptDetailOptionalFieldMapper.Map(_receiptDetailOptionalFieldEntity);
            return FormatDetailOptFields(updatedOptionalField);
        }

        /// <summary>
        /// Set Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Optional Field model</param>
        /// <returns>Receipt Optional Field model</returns>
        public virtual TDetail2 SetOptionalFieldValue(TDetail2 optionalField)
        {
            IsSessionAvailable();
            _receiptOptionalFieldMapper.MapKey(optionalField, _receiptOptionalFieldEntity);
            _receiptOptionalFieldEntity.Read(false);
            _receiptOptionalFieldMapper.Map(optionalField, _receiptOptionalFieldEntity);

            var updatedOptionalField = _receiptOptionalFieldMapper.Map(_receiptOptionalFieldEntity);
            if (updatedOptionalField != null)
            {
                updatedOptionalField.HasChanged = false;
            }
            return FormatOptionalFields(updatedOptionalField);

        }

        /// <summary>
        /// Set Detail Optional Field Value
        /// </summary>
        /// <param name="optionalField">Receipt Detail Optional Field model</param>
        /// <returns>Receipt Detail Optional Field model</returns>
        public virtual TDetail3 SetOptionalFieldValue(TDetail3 optionalField)
        {
            IsSessionAvailable();
            _receiptDetailOptionalFieldMapper.MapKey(optionalField, _receiptDetailOptionalFieldEntity);
            _receiptDetailOptionalFieldEntity.Read(false);
            _receiptDetailOptionalFieldMapper.Map(optionalField, _receiptDetailOptionalFieldEntity);

            var updatedOptionalField = _receiptDetailOptionalFieldMapper.Map(_receiptDetailOptionalFieldEntity);
            if (updatedOptionalField != null)
            {
                updatedOptionalField.HasChanged = false;
            }
            return FormatDetailOptFields(updatedOptionalField);

        }

        /// <summary>
        /// Gets Optional Fields
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param> 
        /// <returns>Enumerable response of Receipt Optional Field</returns>
        public virtual EnumerableResponse<TDetail2> GetOptFields(int pageNumber, int pageSize)
        {
            IsSessionAvailable();
            _receiptOptionalFieldMapper.Map(null, _receiptOptionalFieldEntity);
            _receiptOptionalFieldEntity.Browse("", true);
            _receiptOptionalFieldEntity.Fetch(false);
            return RetrieveOptField(pageNumber, pageSize, _receiptOptionalFieldEntity, _receiptOptionalFieldMapper, 0);
        }

        /// <summary>
        /// Save Detail Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">The details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        /// <param name="isDetail">Is Detail Model</param>
        public virtual bool SaveDetailOptFields(IEnumerable<TDetail3> optionalFieldDetails, string receiptNumber, bool isDetail)
        {
            IsSessionAvailable();
            return SyncDetailOptFields(optionalFieldDetails, isDetail);
        }

        /// <summary>
        /// Gets Optional Fields
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <param name="pageSize">Page Size</param> 
        /// <returns>Enumerable response of Receipt Detail Optional Field</returns>
        public virtual EnumerableResponse<TDetail3> GetDetailOptFields(int pageNumber, int pageSize)
        {
            IsSessionAvailable();
            _receiptDetailOptionalFieldMapper.Map(null, _receiptDetailOptionalFieldEntity);
            _receiptDetailOptionalFieldEntity.Browse("", true);
            _receiptDetailOptionalFieldEntity.Fetch(false);
            return RetrieveDetailOptField(pageNumber, pageSize, _receiptDetailOptionalFieldEntity, _receiptDetailOptionalFieldMapper, 0);
        }

        /// <summary>
        /// To retrieve the List of Optional Fields
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="entity">Entity</param>
        /// <param name="mapper">OptionalFieldMapper</param>
        /// <param name="filterCount">The filter count.</param>
        /// <returns>List of Receipt detail optional fields</returns>
        private EnumerableResponse<TDetail3> RetrieveDetailOptField(int currentPageNumber, int pageSize,
            IBusinessEntity entity, ModelMapper<TDetail3> mapper, int filterCount)
        {
            CheckRights(GetAccessRights(), SecurityType.Inquire);

            var optionalFieldList = new List<TDetail3>();
            var startIndex = CommonUtil.ComputeStartIndex(currentPageNumber, pageSize);
            var endIndex = CommonUtil.ComputeEndIndex(currentPageNumber, pageSize, filterCount);
            var loopCounter = 1;
            if (!entity.Top())
                return new EnumerableResponse<TDetail3>
                {
                    Items = optionalFieldList,
                    TotalResultsCount = filterCount != 0 ? filterCount : GetOptionalFieldsCount(entity)
                };
            do
            {
                if (loopCounter >= startIndex)
                {
                    var optionalField = mapper.Map(entity);
                    optionalField.DisplayIndex = loopCounter;
                    switch (optionalField.Type)
                    {
                        case Models.Enums.Type.Date:
                            optionalField.Value = DateUtil.GetShortDate(optionalField.Value, string.Empty, true);
                            break;
                        case Models.Enums.Type.Time:
                            var value = optionalField.Value ?? "000000";
                            optionalField.Value = Regex.Replace(value, @"(\w{2})(\w{2})(\w{2})", @"$1:$2:$3");
                            break;
                    }

                    //Added to return an empty list if there are no optional fields
                    optionalFieldList.Add(optionalField);
                }

                loopCounter++;
            } while (loopCounter <= endIndex && entity.Next());
            return new EnumerableResponse<TDetail3>
            {
                Items = optionalFieldList,
                TotalResultsCount = filterCount != 0 ? filterCount : GetOptionalFieldsCount(entity)
            };
        }

        /// <summary>
        /// Gets the optional fields count.
        /// </summary>
        /// <returns>Integer value</returns>
        private int GetOptionalFieldsCount(IBusinessEntity entity)
        {
            entity.Top();
            var total = 0;
            do
            {
                total++;
            } while (entity.Next());
            if (!entity.Exists && total == 1)
            {
                return 0;
            }
            return total;
        }

        /// <summary>
        /// Method to synchronize the detail optional fields.
        /// </summary>
        /// <param name="detailOptionalFields">Detail Optional Fields.</param>
        /// <param name="isDetail">Is Detail</param>
        /// <returns></returns>
        private bool SyncDetailOptFields(IEnumerable<TDetail3> detailOptionalFields, bool isDetail)
        {
            var isUpdated = false;
            if (detailOptionalFields == null)
            {
                //This is to update when there are no records and to cover AutoInsert error, if exists
                UpdateHeader(isDetail);
                return false;
            }
            var allDetails = detailOptionalFields as IList<TDetail3> ?? detailOptionalFields.ToList();

            foreach (var detail in allDetails.Where(detail => (detail.HasChanged || detail.IsDeleted) && !detail.IsNewLine))
            {
                isUpdated = isDetail ? SyncDetailOptField(detail) : SyncOptField(detail as TDetail2);
                //UpdateHeader(isDetail);
            }
            foreach (var newLine in allDetails.Where(detail => detail.IsNewLine && detail.OptionalField != null))
            {
                isUpdated = isDetail ? InsertDetailOptField(newLine) : InsertOptField(newLine as TDetail2);
                //UpdateHeader(isDetail);
            }
            return isUpdated;
        }

        /// <summary>
        /// To update the Detail or Header entity
        /// </summary>
        /// <param name="isDetail">Boolean isDetail</param>
        private void UpdateHeader(bool isDetail)
        {
            if (isDetail)
            {
                if (_receiptDetailEntity.Exists)
                {
                    _receiptDetailEntity.Update();
                }
            }
            else
            {
                if (_receiptEntity.Exists)
                {
                    _receiptEntity.Update();
                    Helper.GetExceptions(Session);
                }
            }
        }

        /// <summary>
        /// To set fields for Detail Optional Fields
        /// </summary>
        /// <param name="optionalField">Optional Fields</param>
        private bool SyncDetailOptField(TDetail3 optionalField)
        {
            var isUpdated = false;
            _receiptDetailOptionalFieldMapper.MapKey(optionalField, _receiptDetailOptionalFieldEntity);
            _receiptDetailOptionalFieldEntity.Read(false);

            if (optionalField.IsDeleted && _receiptDetailOptionalFieldEntity.Exists)
            {
                _receiptDetailOptionalFieldEntity.Delete();
                isUpdated = true;
            }
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            // If Details not exist and its deleted then clear the record
            else if (optionalField.IsDeleted && !_receiptDetailOptionalFieldEntity.Exists)
            {
                _receiptDetailOptionalFieldEntity.ClearRecord();
                isUpdated = true;
            }
            else if (_receiptDetailOptionalFieldEntity.Exists)
            {
                _receiptDetailOptionalFieldMapper.MapKey(optionalField, _receiptDetailOptionalFieldEntity);
                _receiptDetailOptionalFieldEntity.Read(false);
                _receiptDetailOptionalFieldMapper.Map(optionalField, _receiptDetailOptionalFieldEntity);
                _receiptDetailOptionalFieldEntity.Update();
                isUpdated = true;
            }
            if (!_receiptDetailOptionalFieldEntity.Exists) return isUpdated;
            _receiptDetailOptionalFieldEntity.Update();
            return true;
        }

        /// <summary>
        /// Insert new detail optional field
        /// </summary>
        /// <param name="newLine">Receipt Detail Optional Field</param>
        /// <returns>True/False</returns>
        private bool InsertDetailOptField(TDetail3 newLine)
        {
            var optionalFieldMapper = new ReceiptDetailOptionalFieldMapper<TDetail3>(Context);
            optionalFieldMapper.Map(newLine, _receiptDetailOptionalFieldEntity);

            //The reason for the exists check is that when an exception is thrown, new line will still be true.
            if (_receiptDetailOptionalFieldEntity.Exists)
            {
                _receiptDetailOptionalFieldEntity.Update();
            }
            if (!newLine.IsDeleted && !_receiptDetailOptionalFieldEntity.Exists)
            {
                _receiptDetailOptionalFieldEntity.Insert();
            }
            return true;
        }

        /// <summary>
        /// To retrieve the List of Optional Fields
        /// </summary>
        /// <param name="currentPageNumber">The current page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="entity">Entity</param>
        /// <param name="mapper">OptionalFieldMapper</param>
        /// <param name="filterCount">The filter count.</param>
        /// <returns>List of Receipt optional fields</returns>
        private EnumerableResponse<TDetail2> RetrieveOptField(int currentPageNumber, int pageSize,
            IBusinessEntity entity, ModelMapper<TDetail2> mapper, int filterCount)
        {
            CheckRights(GetAccessRights(), SecurityType.Inquire);

            var optionalFieldList = new List<TDetail2>();
            var startIndex = CommonUtil.ComputeStartIndex(currentPageNumber, pageSize);
            var endIndex = CommonUtil.ComputeEndIndex(currentPageNumber, pageSize, filterCount);
            var loopCounter = 1;
            var lineNumber = 0;
            if (!entity.Top())
                return new EnumerableResponse<TDetail2>
                {
                    Items = optionalFieldList,
                    TotalResultsCount = filterCount != 0 ? filterCount : GetOptionalFieldsCount(entity)
                };
            do
            {
                if (loopCounter >= startIndex)
                {
                    var optionalField = mapper.Map(entity);
                    optionalField.DisplayIndex = lineNumber++;
                    switch (optionalField.Type)
                    {
                        case Models.Enums.Type.Date:
                            optionalField.Value = DateUtil.GetShortDate(optionalField.Value, string.Empty, true);
                            break;
                        case Models.Enums.Type.Time:
                            var value = optionalField.Value ?? "000000";
                            optionalField.Value = Regex.Replace(value, @"(\w{2})(\w{2})(\w{2})", @"$1:$2:$3");
                            break;
                    }

                    //Added to return an empty list if there are no optional fields
                    optionalFieldList.Add(optionalField);
                }

                loopCounter++;
            } while (loopCounter <= endIndex && entity.Next());
            return new EnumerableResponse<TDetail2>
            {
                Items = optionalFieldList,
                TotalResultsCount = filterCount != 0 ? filterCount : GetOptionalFieldsCount(entity)
            };
        }

        /// <summary>
        /// To set fields for Optional Fields
        /// </summary>
        /// <param name="optionalField">Optional Fields</param>
        private bool SyncOptField(TDetail2 optionalField)
        {
            var optionalFieldMapper = new ReceiptOptionalFieldMapper<TDetail2>(Context);
            var isUpdated = false;
            optionalFieldMapper.MapKey(optionalField, _receiptOptionalFieldEntity);
            _receiptOptionalFieldEntity.Read(false);

            if (optionalField.IsDeleted && _receiptOptionalFieldEntity.Exists)
            {
                _receiptOptionalFieldEntity.Delete();
                isUpdated = true;
            }
            else if (optionalField.IsDeleted && !_receiptOptionalFieldEntity.Exists)
            {
                _receiptOptionalFieldEntity.ClearRecord();
                isUpdated = true;
            }
            else if (_receiptOptionalFieldEntity.Exists)
            {
                optionalFieldMapper.MapKey(optionalField, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Read(false);
                optionalFieldMapper.Map(optionalField, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Update();
                isUpdated = true;
            }
            if (!_receiptOptionalFieldEntity.Exists) return isUpdated;
            _receiptOptionalFieldEntity.Update();
            return true;
        }

        /// <summary>
        /// Insert new optional field
        /// </summary>
        /// <param name="newLine">Receipt Detail Model</param>
        /// <returns>True/False</returns>
        private bool InsertOptField(TDetail2 newLine)
        {
            var optionalFieldMapper = new ReceiptOptionalFieldMapper<TDetail2>(Context);
            optionalFieldMapper.Map(newLine, _receiptOptionalFieldEntity);

            if (!newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.Insert();
            }
            //The reason for the exists check is that when an exception is thrown, new line will still be true.
            if (_receiptOptionalFieldEntity.Exists)
            {
                _receiptOptionalFieldEntity.Update();
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Refreshes the specified header.
        /// </summary>
        /// <param name="model">header model.</param>
        /// <returns>returns refreshed header</returns>
        public new T Refresh(T model)
        {
            var receiptMapper = new ReceiptMapper<T>(Context);
            IsSessionAvailable();
            receiptMapper.Map(model, new List<IBusinessEntity> { _receiptEntity });

            var headerModel =
                receiptMapper.Map(new List<IBusinessEntity>
                {
                    _receiptEntity,
                    _receiptDetailEntity,
                    _receiptOptionalFieldEntity,
                    _receiptDetailOptionalFieldEntity,
                    _receiptDetailSerialNumberEntity,
                    _receiptDetailLotNumberEntity
                });

            headerModel.HomeCurrency = Session.HomeCurrency;
            headerModel.Warnings = Helper.GetExceptions(Session);
            return headerModel;
        }

        /// <summary>
        /// Read the header 
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns></returns>
        public virtual T ReadHeader(T model)
        {
            if (model != null)
            {
                _receiptMapper.Map(model, _receiptEntity);
            }
            var headerModel = _receiptMapper.Map(_receiptEntity);
            headerModel.HomeCurrency = Session.HomeCurrency;
            headerModel.Warnings = Helper.GetExceptions(Session);
            return headerModel;
        }

        /// <summary>
        /// Additional Access Check for Export and Import
        /// </summary>
        /// <returns>User Access</returns>
        public override UserAccess GetAccessRights()
        {
            var userAccess = base.GetAccessRights();
            if (SecurityCheck(Security.TUImport))
            {
                userAccess.SecurityType |= SecurityType.Import;

            }
            if (SecurityCheck(Security.TUExport))
            {
                userAccess.SecurityType |= SecurityType.Export;
            }
            if (userAccess.SecurityType.HasFlag(SecurityType.Modify) || userAccess.SecurityType.HasFlag(SecurityType.Add) || userAccess.SecurityType.HasFlag(SecurityType.Delete))
            {
                AddSecurityType(userAccess, SecurityType.Inquire);
            }

            UserAccess access = GetAccessRightsOptionalFields();
            userAccess.ResourceSecurity = new Dictionary<string, UserAccess>
            {
                {ReceiptOptionalField.EntityName, access},
                {ReceiptDetailOptionalField.EntityName, access}                
            };

            return userAccess;
        }

        /// <summary>
        /// Get the Default Optional Field
        /// </summary>
        /// <returns>Header with default optional field</returns>
        public virtual T GetDefaultDetailOptField()
        {
            DetailProcess();
            var detailData = _receiptDetailMapper.Map(_receiptDetailEntity);

            var headerData = _receiptMapper.Map(_receiptEntity);
            headerData.ReceiptDetail = new EnumerableResponse<ReceiptDetail>
            {
                Items = new List<TU> { detailData }
            };
            return headerData;
        }

        /// <summary>
        /// Find whether Internal Usage record with Internal Usage Number passed exists
        /// </summary>
        /// <param name="id">An Internal Usage Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns True if exists, False otherwise</returns>
        public virtual bool Exists(string id, T model)
        {
            _receiptEntity.SetValue(ReceiptHeader.Fields.ReceiptNumber, id);
            var exists = _receiptEntity.Exists;
            return exists;
        }

        /// <summary>
        /// Retrieves the composite rate between the given source and home currencies. 
        /// If the currencies are non-block currencies, the call functions the same as GetCurrencyRate.
        /// </summary>
        /// <param name="rateType">String param for Rate type</param>
        /// <param name="sourceCurrencyCode">String param for Source Currency Code </param>
        /// <param name="date">DateTime param for Date</param>
        /// <returns>Returns the corresponding Currency Rate object.</returns>
        public virtual CompositeCurrencyRate GetCurrencyRateComposite(string rateType,
            string sourceCurrencyCode, DateTime date)
        {
            return Session.GetCurrencyRateComposite(Session.HomeCurrency, rateType, sourceCurrencyCode, date);
        }



        /// <summary>
        /// Get security access rights for optional fields
        /// </summary>
        /// <returns>UserAccess</returns>
        private UserAccess GetAccessRightsOptionalFields()
        {
            var optionalFieldUserAccess = new UserAccess();
            if (SecurityCheck(Security.TUOptionalFieldTransaction))
            {
                optionalFieldUserAccess.SecurityType = SecurityType.Add | SecurityType.Delete;
            }
            if (SecurityCheck(Security.TUReceipt))
            {
                optionalFieldUserAccess.SecurityType = optionalFieldUserAccess.SecurityType | SecurityType.Modify;
            }
            return optionalFieldUserAccess;
        }

        #endregion
    }
}