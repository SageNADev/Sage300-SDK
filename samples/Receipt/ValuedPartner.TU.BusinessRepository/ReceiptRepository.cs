// The MIT License (MIT) 
// Copyright (c) 1994-2019 Sage Software, Inc.  All rights reserved.
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
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Base;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Exceptions;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums;
using Sage.CA.SBS.ERP.Sage300.Common.Models.Enums.ExportImport;
using Sage.CA.SBS.ERP.Sage300.Common.Models.ExportImport;
using Sage.CA.SBS.ERP.Sage300.Common.Resources;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ValuedPartner.TU.BusinessRepository.Mappers;
using ValuedPartner.TU.Interfaces.BusinessRepository;
using ValuedPartner.TU.Models;
using ValuedPartner.TU.Models.Enums;
using ICModel = Sage.CA.SBS.ERP.Sage300.IC.Models;

#endregion

namespace ValuedPartner.TU.BusinessRepository
{
    /// <summary>
    /// Repository for Receipt
    /// </summary>
    public class ReceiptRepository : BaseHeaderDetailRepository, IReceiptRepository
    {
        #region Business Entity Variables

        /// <summary>
        /// IC0590 - Inventory Control, Receipt
        /// </summary>
        private IBusinessEntity _receiptHeaderEntity;

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
        /// Validator Filter - Reserved
        /// </summary>
        /// <value>The valid record filter.</value>
        protected Func<ReceiptHeader, Boolean> ValidRecordFilter { get; set; }


        /// <summary>
        /// Gets or sets the detail filter.
        /// </summary>
        /// <value>The detail filter.</value>
        public Expression<Func<ReceiptDetail, bool>> DetailFilter { get; set; }

        #endregion

        #region Private Variables

        private readonly ModelHierarchyMapper<ReceiptHeader> _mapper;

        /// <summary>
        /// Receipt Mapper
        /// </summary>
        private readonly ReceiptHeaderMapper _receiptMapper;

        /// <summary>
        /// Receipt Detail Mapper
        /// </summary>
        private readonly ReceiptDetailMapper _receiptDetailMapper;

        /// <summary>
        /// Receipt Optional Field Mapper
        /// </summary>
        private readonly ReceiptOptionalFieldMapper _receiptOptionalFieldMapper;

        /// <summary>
        /// Receipt Detail Optional Field Mapper
        /// </summary>
        private readonly ReceiptDetailOptionalFieldMapper _receiptDetailOptionalFieldMapper;

        /// <summary>
        /// Constant to detail
        /// </summary>
        private const int VERIFY_DETAIL = 1001;

        #endregion

        #region Constructors

        /// <summary>
        /// Sets Context and DBLink
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptRepository(Context context) : base(context)
        {
            SetValidRecordFilter();

            _mapper = new ReceiptMapper(context);
            _receiptMapper = new ReceiptHeaderMapper(context);
            _receiptDetailMapper = new ReceiptDetailMapper(context);
            _receiptOptionalFieldMapper = new ReceiptOptionalFieldMapper(context);
            _receiptDetailOptionalFieldMapper = new ReceiptDetailOptionalFieldMapper(context);

            CreateBusinessEntities();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Get receipt
        /// </summary>
        /// <param name="id">Receipt Id</param>
        /// <returns>Receipt</returns>
        public ReceiptHeader GetById<TKey>(TKey id)
        {
            CheckRights(_receiptHeaderEntity, SecurityType.Inquire);
            var receiptMapper = new ReceiptMapper(Context);
            var receiptNumber = id.ToString();

            if (!string.IsNullOrEmpty(receiptNumber))
            {
                _receiptHeaderEntity.Order = 2;
                _receiptHeaderEntity.SetValue(ReceiptHeader.Fields.ReceiptNumber, receiptNumber, true);
            }

            if (!string.IsNullOrEmpty(receiptNumber))
            {
                if (!_receiptHeaderEntity.Read(false))
                {
                    _receiptHeaderEntity.ClearRecord();
                    return
                        receiptMapper.Map(
                            new List<IBusinessEntity>
                            {
                                _receiptHeaderEntity,
                                _receiptOptionalFieldEntity,
                                _receiptDetailOptionalFieldEntity,
                                _receiptDetailSerialNumberEntity,
                                _receiptDetailLotNumberEntity
                            }, 0, 10);
                }
            }

            //Setting Receipt Mode 
            var receiptMode = ReceiptType.Receipt;
            if (_receiptHeaderEntity.Exists)
            {
                var completed = (Complete)(_receiptHeaderEntity.GetValue<int>(ReceiptHeader.Index.Complete));
                var status = (RecordStatus)(_receiptHeaderEntity.GetValue<int>(ReceiptHeader.Index.RecordStatus));

                if (completed == Complete.Yes)
                {
                    receiptMode = ReceiptType.Complete;
                }
                else if (status == RecordStatus.Entered)
                {
                    receiptMode = (ReceiptType)(_receiptHeaderEntity.GetValue<int>(ReceiptHeader.Index.ReceiptType));
                }
                else
                {
                    receiptMode = ReceiptType.Return;
                }
                _receiptHeaderEntity.SetValue(ReceiptHeader.Fields.ReceiptType, receiptMode);
            }
            else
            {
                _receiptHeaderEntity.SetValue(ReceiptHeader.Fields.ReceiptType, receiptMode);
            }
            // End - Setting Receipt Mode 

            var headerModel =
                receiptMapper.Map(
                    new List<IBusinessEntity>
                    {
                        _receiptHeaderEntity,
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
        public virtual ReceiptHeader Post(ReceiptHeader model, Expression<Func<ReceiptHeader, bool>> filter, bool yesNo)
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
            var receiptMapper = new ReceiptMapper(Context);
            var headerModel = receiptMapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity, _receiptDetailEntity });
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
        public EnumerableResponse<ReceiptDetail> GetDetail(int pageNumber, int pageSize,
            Expression<Func<ReceiptDetail, bool>> filter = null, OrderBy orderBy = null)
        {
            var resultsCount = SetFilter(_receiptDetailEntity, filter, null, orderBy);

            if (_receiptDetailEntity.Fetch(false))
            {
                return new EnumerableResponse<ReceiptDetail>
                {
                    Items = MapDataToModel(_receiptDetailEntity, _receiptDetailMapper, pageNumber, pageSize, resultsCount),
                    TotalResultsCount = GetTotalRecords(_receiptDetailEntity)
                };
            }
            return new EnumerableResponse<ReceiptDetail> { Items = new List<ReceiptDetail>(), TotalResultsCount = 0 };
        }

        /// <summary>
        /// Creates a new Receipt Header.
        /// </summary>
        /// <returns>Receipt Header Model</returns>
        public ReceiptHeader NewHeader()
        {
            CheckRights(_receiptHeaderEntity, SecurityType.Inquire);

            var receiptMapper = new ReceiptMapper(Context);

            _receiptHeaderEntity.Order = 0;
            _receiptHeaderEntity.SetValue(ReceiptHeader.Fields.SequenceNumber, 0);
            _receiptHeaderEntity.Init();
            _receiptDetailEntity.ClearRecord();
            _receiptHeaderEntity.Order = 2;

            _receiptDetailOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
            _receiptHeaderEntity.Read(false);

            //Pull the default optional fields 
            _receiptHeaderEntity.SetValue(ReceiptHeader.Fields.ProcessCommand, ProcessCommand.InsertOptionalFields);
            _receiptHeaderEntity.Process();
            var headerModel = receiptMapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity, _receiptOptionalFieldEntity, _receiptDetailOptionalFieldEntity, _receiptDetailSerialNumberEntity, _receiptDetailLotNumberEntity }, 0, 10);

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
        public ReceiptHeader Add(ReceiptHeader model)
        {
            CheckRights(_receiptHeaderEntity, SecurityType.Add);
            CreateBusinessEntities();

            var receiptMapper = new ReceiptMapper(Context);
            _receiptMapper.Map(model, _receiptHeaderEntity);

            var details2 = GetReceiptOptionalFieldModel(model).ToList();
            var details3 = GetReceiptDetailOptionalFieldModel(model);

            if (!_receiptHeaderEntity.Exists)
            {
                SyncReceiptDetailOptionalField(details3);
                SyncReceiptOptionalField(details2);

                _receiptHeaderEntity.Verify();
                _receiptHeaderEntity.Insert();
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
            model = receiptMapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity, _receiptDetailEntity });
            model.Warnings = Helper.GetExceptions(Session);
            return model;
        }

        /// <summary>
        /// Method to Save Receipts.
        /// </summary>
        /// <param name="model">Receipts Model</param>
        /// <returns>Receipt Header Model</returns>
        public ReceiptHeader Save(ReceiptHeader model)
        {
            IsSessionAvailable();
            CheckRights(GetAccessRights(), SecurityType.Modify);
            var receiptMapper = new ReceiptMapper(Context);
            var header = model;
            if (header.RecordStatus != RecordStatus.Posted)
            {
                header.RecordStatus = RecordStatus.Entered;
            }
            _receiptMapper.Map(header, _receiptHeaderEntity);

            var details2 = GetReceiptOptionalFieldModel(model).ToList();
            var details3 = GetReceiptDetailOptionalFieldModel(model);

            SyncReceiptDetailOptionalField(details3);
            SyncReceiptOptionalField(details2);
            _receiptHeaderEntity.Verify();

            if (_receiptHeaderEntity.Exists)
            {
                _receiptHeaderEntity.Update();
            }
            else
            {
                _receiptHeaderEntity.Insert();
            }
            var headerModel = receiptMapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity });
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
        public ReceiptHeader Delete(Expression<Func<ReceiptHeader, bool>> filter)
        {
            CheckRights(_receiptHeaderEntity, SecurityType.Delete);
            CreateBusinessEntities();
            if (_receiptHeaderEntity.Read(false)) // Check if the record exists and then delete.
            {
                if (Search(_receiptHeaderEntity, filter))
                {
                    _receiptHeaderEntity.Delete();
                }
            }
            return _receiptMapper.Map(_receiptHeaderEntity);
        }

        /// <summary>
        /// Get the Detail Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt Detail Model</returns>
        protected IEnumerable<ReceiptDetail> GetDetailModel(ReceiptHeader header)
        {
            var receiptDetail = header.ReceiptDetail;
            if (receiptDetail != null && receiptDetail.Items != null && receiptDetail.Items.Any())
                return receiptDetail.Items.Count() != 0 ? header.ReceiptDetail.Items.Cast<ReceiptDetail>().ToList() : null;
            return new List<ReceiptDetail>();
        }

        /// <summary>
        /// Get the ReceiptOptionalField Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt ReceiptOptionalField</returns>
        protected IEnumerable<ReceiptOptionalField> GetReceiptOptionalFieldModel(ReceiptHeader header)
        {
            var receiptOptionalField = header.ReceiptOptionalField;
            if (receiptOptionalField != null && receiptOptionalField.Items != null && receiptOptionalField.Items.Any())
                return receiptOptionalField.Items.Count() != 0 ? header.ReceiptOptionalField.Items.Cast<ReceiptOptionalField>().ToList() : null;
            return new List<ReceiptOptionalField>();
        }

        /// <summary>
        /// Get the ReceiptDetailOptionalField Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt ReceiptDetailOptionalField</returns>
        protected IEnumerable<ReceiptDetailOptionalField> GetReceiptDetailOptionalFieldModel(ReceiptHeader header)
        {
            var receiptDetailOptionalField = header.ReceiptDetailOptionalField;
            if (receiptDetailOptionalField != null && receiptDetailOptionalField.Items != null && receiptDetailOptionalField.Items.Any())
                return receiptDetailOptionalField.Items.Count() != 0 ? header.ReceiptDetailOptionalField.Items.Cast<ReceiptDetailOptionalField>().ToList() : null;
            return new List<ReceiptDetailOptionalField>();
        }

        /// <summary>
        /// Get the ReceiptDetailLotNumber Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt ReceiptDetailLotNumber</returns>
        protected IEnumerable<ReceiptDetailLotNumber> GetDetail4Model(ReceiptHeader header)
        {
            var receiptDetailLotNumber = header.ReceiptDetailLotNumber;
            if (receiptDetailLotNumber != null && receiptDetailLotNumber.Items != null && receiptDetailLotNumber.Items.Any())
                return receiptDetailLotNumber.Items.Count() != 0 ? header.ReceiptDetailLotNumber.Items.Cast<ReceiptDetailLotNumber>().ToList() : null;
            return new List<ReceiptDetailLotNumber>();
        }

        /// <summary>
        /// Get the ReceiptDetailSerialNumber Model
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>Receipt detail 5</returns>
        protected IEnumerable<ReceiptDetailSerialNumber> GetDetail5Model(ReceiptHeader header)
        {
            var receiptDetailSerialNumber = header.ReceiptDetailSerialNumber;
            if (receiptDetailSerialNumber != null && receiptDetailSerialNumber.Items != null && receiptDetailSerialNumber.Items.Any())
                return receiptDetailSerialNumber.Items.Count() != 0
                    ? header.ReceiptDetailSerialNumber.Items.Cast<ReceiptDetailSerialNumber>().ToList()
                    : null;
            return new List<ReceiptDetailSerialNumber>();
        }

        /// <summary>
        /// Map for process detail
        /// </summary>
        /// <param name="detail">Receipt Detail</param>
        /// <param name="detailEntity">Business Entity</param>
        protected void ProcessMap(ReceiptDetail detail, IBusinessEntity detailEntity)
        {
            detailEntity.SetValue(ReceiptDetail.Index.SequenceNumber, detail.SequenceNumber, true);
            detailEntity.SetValue(ReceiptDetail.Index.LineNumber, detail.LineNumber, true);
        }

        /// <summary>
        /// Map for process ReceiptOptionalField
        /// </summary>
        /// <param name="detail2">Receipt ReceiptOptionalField</param>
        /// <param name="detail2Entity">Business Entity</param>
        protected void ProcessMap2(ReceiptOptionalField detail2, IBusinessEntity detail2Entity)
        {
            detail2Entity.SetValue(ReceiptOptionalField.Index.SequenceNumber, detail2.SequenceNumber, true);
            detail2Entity.SetValue(ReceiptOptionalField.Index.OptionalField, detail2.OptionalField, true);
        }

        /// <summary>
        /// Map for process ReceiptDetailOptionalField
        /// </summary>
        /// <param name="detail3">Receipt ReceiptDetailOptionalField</param>
        /// <param name="detail3Entity">Business Entity</param>
        protected void ProcessMap3(ReceiptDetailOptionalField detail3, IBusinessEntity detail3Entity)
        {
            detail3Entity.SetValue(ReceiptDetailOptionalField.Index.SequenceNumber, detail3.SequenceNumber, true);
            detail3Entity.SetValue(ReceiptDetailOptionalField.Index.LineNumber, detail3.LineNumber, true);
            detail3Entity.SetValue(ReceiptDetailOptionalField.Index.OptionalField, detail3.OptionalField, true);
        }

        /// <summary>
        /// Map for process ReceiptDetailLotNumber
        /// </summary>
        /// <param name="detail4">Receipt Detail4</param>
        /// <param name="detail4Entity">Business Entity</param>
        protected void ProcessMap4(ReceiptDetailLotNumber detail4, IBusinessEntity detail4Entity)
        {
            detail4Entity.SetValue(ReceiptDetailLotNumber.Index.SequenceNumber, detail4.SequenceNumber, true);
            detail4Entity.SetValue(ReceiptDetailLotNumber.Index.LineNumber, detail4.LineNumber, true);
            detail4Entity.SetValue(ReceiptDetailLotNumber.Index.LotNumber, detail4.LotNumber, true);
        }

        /// <summary>
        /// Map for process ReceiptDetailSerialNumber
        /// </summary>
        /// <param name="detail5">ReceiptDetailSerialNumber</param>
        /// <param name="detail5Entity">Business Entity</param>
        protected void ProcessMap5(ReceiptDetailSerialNumber detail5, IBusinessEntity detail5Entity)
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
        protected int GetDetailCount(IBusinessEntity headerEntity)
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
        public void DetailProcess()
        {
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.ProcessCommand, ProcessCommand.InsertOptionalFields, true);
            _receiptDetailEntity.Process();
        }


        /// <summary>
        /// Method to Location Process
        /// </summary>
        /// <param name="model"></param>
        private void LocationProcess(ReceiptDetail model)
        {
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.Location, model.Location, true);
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.CheckBelowZero, true);
            _receiptDetailEntity.Process();
            _receiptDetailOptionalFieldEntity.ClearRecord();
            _receiptDetailOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
            _receiptHeaderEntity.SetValue(ReceiptHeader.Index.RecordStatus, RecordStatus.Entered, true);
        }

        /// <summary>
        /// Method to Item Process
        /// </summary>
        /// <param name="model"></param>
        private void ItemProcess(ReceiptDetail model)
        {
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.ItemNumber, model.ItemNumber, true);
            _receiptDetailEntity.Process();
        }


        /// <summary>
        /// Synchronizes the ReceiptOptionalField.
        /// </summary>
        /// <param name="details">The details.</param>
        private void SyncReceiptOptionalField(IEnumerable<ReceiptOptionalField> details)
        {
            if (details == null) return;

            var allDetails = details as IList<ReceiptOptionalField> ?? details.ToList();
            var newLine = allDetails.FirstOrDefault(detail => detail.IsNewLine);

            if (newLine != null)
            {
                InsertReceiptOptionalField(newLine);
            }
            foreach (var detail in allDetails.Where(detail => detail.HasChanged || detail.IsDeleted).Where(detail => detail != newLine))
            {
                SyncReceiptOptionalField(detail);
            }
        }

        /// <summary>
        /// Synchronizes the ReceiptDetailOptionalField.
        /// </summary>
        /// <param name="details3">The details.</param>
        private void SyncReceiptDetailOptionalField(IEnumerable<ReceiptDetailOptionalField> details)
        {
            if (details == null) return;

            var allDetails = details as IList<ReceiptDetailOptionalField> ?? details.ToList();
            var newLine = allDetails.FirstOrDefault(detail => detail.IsNewLine);

            if (newLine != null)
            {
                InsertReceiptDetailOptionalField(newLine);
            }
            foreach (var detail in allDetails.Where(detail => detail.HasChanged || detail.IsDeleted).Where(detail => detail != newLine))
            {
                SyncReceiptDetailOptionalField(detail);
            }
        }

        /// <summary>
        /// Sync ReceiptOptionalField Records
        /// </summary>
        /// <param name="detail2">Receipt Optional Field Model</param>
        private void SyncReceiptOptionalField(ReceiptOptionalField detail)
        {
            _receiptOptionalFieldMapper.MapKey(detail, _receiptOptionalFieldEntity);

            var recordExist = _receiptOptionalFieldEntity.Read(false);

            if (!recordExist && !detail.IsDeleted)
            {
                _receiptOptionalFieldEntity.Insert();
                detail.IsNewLine = false;
            }
            if (detail.IsDeleted && recordExist)
            {
                _receiptOptionalFieldEntity.Delete();
            }
            else if (detail.IsDeleted && !recordExist)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }
            else if (recordExist)
            {
                _receiptOptionalFieldMapper.Map(detail, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Update();
                _receiptOptionalFieldMapper.MapKey(detail, _receiptOptionalFieldEntity);
                _receiptOptionalFieldEntity.Read(false);
            }
        }

        /// <summary>
        /// Sync ReceiptDetailOptionalField Records
        /// </summary>
        /// <param name="detail3">Receipt Detail Optional Field</param>
        private void SyncReceiptDetailOptionalField(ReceiptDetailOptionalField detail)
        {
            _receiptDetailOptionalFieldMapper.MapKey(detail, _receiptDetailOptionalFieldEntity);

            var recordExist = _receiptDetailOptionalFieldEntity.Read(false);

            if (!recordExist && !detail.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Insert();

                detail.IsNewLine = false;
            }

            if (detail.IsDeleted && recordExist)
            {
                _receiptDetailOptionalFieldEntity.Delete();
            }
            else if (detail.IsDeleted && !recordExist)
            {
                _receiptDetailOptionalFieldEntity.ClearRecord();
            }
            else if (recordExist)
            {
                _receiptDetailOptionalFieldMapper.Map(detail, _receiptDetailOptionalFieldEntity);
                _receiptDetailOptionalFieldEntity.Update();
                _receiptDetailOptionalFieldMapper.MapKey(detail, _receiptDetailOptionalFieldEntity);
                _receiptDetailOptionalFieldEntity.Read(false);
            }
        }

        /// <summary>
        /// Insert Detail records
        /// </summary>
        /// <param name="newLine">Receipt Detail Model</param>
        private void InsertDetail(ReceiptDetail newLine)
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
        ///  Insert ReceiptOptionalField records
        /// </summary>
        /// <param name="newLine">Receipt Optional Field</param>
        private void InsertReceiptOptionalField(ReceiptOptionalField newLine)
        {
            _receiptOptionalFieldMapper.Map(newLine, _receiptOptionalFieldEntity);

            var recordExists = _receiptOptionalFieldEntity.Exists;

            if (!recordExists && newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }

            if (!recordExists && !newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.RecordCreate(ViewRecordCreate.NoInsert);
                _receiptOptionalFieldEntity.Insert();
                newLine.IsNewLine = false;
            }
            else if (recordExists && !newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.Read(false);
                _receiptOptionalFieldEntity.Update();
            }
            else if (recordExists && newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.Read(false);
                _receiptOptionalFieldEntity.Delete();
            }
        }

        /// <summary>
        ///  Insert ReceiptDetailOptionalField records
        /// </summary>
        /// <param name="newLine3">Receipt Detail Optional Field</param>
        private void InsertReceiptDetailOptionalField(ReceiptDetailOptionalField newLine)
        {
            _receiptDetailOptionalFieldMapper.Map(newLine, _receiptDetailOptionalFieldEntity);
            var recordExists = _receiptDetailOptionalFieldEntity.Exists;

            if (!recordExists && newLine.IsDeleted)
            {
                _receiptOptionalFieldEntity.ClearRecord();
            }

            if (!recordExists && !newLine.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Insert();
                newLine.IsNewLine = false;
            }
            else if (recordExists && !newLine.IsDeleted)
            {
                _receiptDetailOptionalFieldEntity.Read(false);
                _receiptDetailOptionalFieldEntity.Update();
            }
            else if (recordExists && newLine.IsDeleted)
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
        private void SyncOptionalFields(IEnumerable<ReceiptOptionalField> detailOptionalFields)
        {
            if (detailOptionalFields == null)
            {
                //This is to update when there are no records and to cover AutoInsert error, if exists
                if (_receiptHeaderEntity.Exists)
                {
                    _receiptHeaderEntity.Update();
                }
                return;
            }

            var allDetails = detailOptionalFields as IList<ReceiptOptionalField> ?? detailOptionalFields.ToList();

            foreach (var detail in allDetails.Where(detail => (detail.HasChanged || detail.IsDeleted) && !detail.IsNewLine))
            {
                SyncOptionalField(detail);
                if (_receiptHeaderEntity.Exists)
                {
                    _receiptHeaderEntity.Update();
                }

            }
            foreach (var newLine in allDetails.Where(detail => detail.IsNewLine && detail.OptionalField != null))
            {
                InsertOptionalField(newLine);
                if (_receiptHeaderEntity.Exists)
                {
                    _receiptHeaderEntity.Update();
                }
            }
        }

        /// <summary>
        /// Open and compose business entities
        /// </summary>
        protected override void CreateBusinessEntities()
        {
            _receiptHeaderEntity = OpenEntity(ReceiptHeader.EntityName, true);
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

            _receiptHeaderEntity.Compose(new[] { _receiptDetailEntity.View, _receiptOptionalFieldEntity.View });

            _receiptDetailEntity.Compose(new[] {
                _receiptHeaderEntity.View,
                _itemEntity.View,
                _itemUnitOfMeasureEntity.View,
                _categoryEntity.View,
                _locationsEntity.View,
                _locationDetailEntity.View,
                _receiptDetailOptionalFieldEntity.View,
                _receiptDetailSerialNumberEntity.View,
                _receiptDetailLotNumberEntity.View
            });

            _receiptOptionalFieldEntity.Compose(new[] { _receiptHeaderEntity.View });
            _receiptDetailOptionalFieldEntity.Compose(new[] { _receiptDetailEntity.View });
            _receiptDetailLotNumberEntity.Compose(new[] { _receiptDetailEntity.View });
            _receiptDetailSerialNumberEntity.Compose(new[] { _receiptDetailEntity.View });
        }

        /// <summary>
        /// To retrieve the List of Optional Fields
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="filterCount">The filter count.</param>
        /// <returns>List of receipt optional fields</returns>
        private EnumerableResponse<ReceiptOptionalField> RetrieveOptionalField(int pageNumber,
            int pageSize, int filterCount)
        {
            var optionalFieldList = new List<ReceiptOptionalField>();
            var optionalFieldItems = new EnumerableResponse<ReceiptOptionalField>();
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
        private ReceiptOptionalField FormatOptionalFields(ReceiptOptionalField optionalField)
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
        private ReceiptDetailOptionalField FormatDetailOptFields(ReceiptDetailOptionalField optionalField)
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
        private void SyncOptionalField(ReceiptOptionalField optionalField)
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

        /// <summary>
        /// Insert Detail records
        /// </summary>
        /// <param name="newLine">New detail</param>
        /// <returns>True if successfully inserted, false otherwise</returns>
        private bool InsertDetailModels(ReceiptDetail newLine)
        {
            _receiptDetailMapper.Map(newLine, _receiptDetailEntity);
            if (!_receiptDetailEntity.Exists && newLine.IsDeleted)
            {
                _receiptDetailEntity.ClearRecord();
            }

            _receiptDetailMapper.Map(newLine, _receiptDetailEntity);

            if (!_receiptDetailEntity.Exists && !newLine.IsDeleted)
            {
                _receiptDetailEntity.Insert();
                newLine.IsNewLine = false;
            }
            else if (_receiptDetailEntity.Exists && !newLine.IsDeleted)
            {
                _receiptDetailEntity.Read(false);
                _receiptDetailEntity.Update();
            }
            else if (_receiptDetailEntity.Exists && newLine.IsDeleted)
            {
                _receiptDetailEntity.Read(false);
                _receiptDetailEntity.Delete();
            }
            return true;
        }

        // <summary>
        /// Synchronizes the detail models.
        /// </summary>
        /// <typeparam name="TU">The type of TU</typeparam>
        /// <param name="businessEntity">The business entity.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="detail">The detail.</param>
        /// <returns>True if successfully synced, false otherwise</returns>
        private bool SyncDetailModels(ReceiptDetail detail)
        {
            var isDetailUpdated = false;
            _receiptDetailMapper.MapKey(detail, _receiptDetailEntity);
            if (!_receiptDetailEntity.Exists && !detail.IsDeleted)
            {
                _receiptDetailEntity.Insert();
                detail.IsNewLine = false;
            }

            _receiptDetailEntity.Read(false);

            if (detail.IsDeleted)
            {
                if (_receiptDetailEntity.Exists)
                {
                    _receiptDetailEntity.Delete();
                }
                else
                {
                    _receiptDetailEntity.ClearRecord();
                }

                isDetailUpdated = true;
            }
            else if (_receiptDetailEntity.Exists)
            {
                _receiptDetailMapper.Map(detail, _receiptDetailEntity);
                _receiptDetailEntity.Update();
                isDetailUpdated = true;
            }
            return isDetailUpdated;
        }

        /// <summary>
        /// New Detail Model
        /// </summary>
        /// <typeparam name="TU">TU model</typeparam>
        /// <param name="businessEntity">Business entity</param>
        /// <param name="mapper">Model mapper</param>
        /// <param name="activeFilter">Filter NOT used</param>
        /// <param name="currentDetail">Detail</param>
        /// <returns>Header model</returns>
        private ReceiptHeader NewDetailModels(ReceiptDetail currentDetail)
        {
            if (currentDetail != null)
            {
                _receiptDetailMapper.MapKey(currentDetail, _receiptDetailEntity);
                if (currentDetail.HasChanged && !currentDetail.IsNewLine)
                {
                    if (_receiptDetailEntity.Exists)
                    {
                        _receiptDetailEntity.Read(false);
                        _receiptDetailMapper.Map(currentDetail, _receiptDetailEntity);
                        _receiptDetailEntity.Update();
                    }
                    else
                    {
                        _receiptDetailMapper.Map(currentDetail, _receiptDetailEntity);
                        _receiptDetailEntity.Insert();
                    }
                }
                else if (currentDetail.IsNewLine)
                {
                    _receiptDetailMapper.Map(currentDetail, _receiptDetailEntity);
                    _receiptDetailEntity.Insert();
                }
            }

            _receiptDetailEntity.RecordCreate(ViewRecordCreate.NoInsert);
            return _mapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity, _receiptDetailEntity });
        }

        /// <summary>
        /// Sync Detail Models
        /// </summary>
        /// <typeparam name="TU">TU model</typeparam>
        /// <param name="businessEntity">Business entity</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="details">Details</param>
        /// <returns>True if successful, false otherwise</returns>
        private bool SyncDetailsModels(IEnumerable<ReceiptDetail> details)
        {
            var isDetailUpdated = false;
            if (details == null) return false;

            var allDetails = details as IList<ReceiptDetail> ?? details.ToList();
            var newLine = allDetails.FirstOrDefault(detail => detail.IsNewLine);

            if (newLine != null)
            {
                isDetailUpdated = InsertDetailModels(newLine);

            }
            foreach (
                var detail in
                    allDetails.Where(detail => detail.HasChanged || detail.IsDeleted).Where(detail => detail != newLine))
            {
                isDetailUpdated = SyncDetailModels(detail);
            }
            return isDetailUpdated;
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

        #region Public Methods

        /// <summary>
        /// Method to Get Item values data.
        /// </summary>
        /// <param name="detail">Receipt Detail Model</param>
        /// <param name="eventType">Event Type</param>
        /// <returns>Receipt Detail Model</returns>
        public virtual ReceiptDetail GetRowValues(ReceiptDetail detail, int eventType)
        {
            IsSessionAvailable();
            var detailsMapper = new ReceiptDetailMapper(Context);
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.SequenceNumber, detail.SequenceNumber);
            _receiptDetailEntity.SetValue(ReceiptDetail.Index.LineNumber, detail.LineNumber);

            if (_receiptDetailEntity.Exists)
            {
                _receiptDetailEntity.Read(false);
            }

            switch (eventType)
            {
                case VERIFY_DETAIL:
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
        public virtual ReceiptHeader GetHeaderValues(ReceiptHeader model, int eventType)
        {
            switch (eventType)
            {
                case ReceiptHeader.Index.ReceiptType:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.ReceiptType, model.ReceiptType, true);
                    break;

                case ReceiptHeader.Index.AdditionalCost:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.AdditionalCost, model.AdditionalCost, true);
                    break;

                case ReceiptHeader.Index.RequireLabels:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.RequireLabels, model.RequireLabels, true);
                    break;

                case ReceiptHeader.Index.ReceiptCurrency:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.ReceiptCurrency, model.ReceiptCurrency, true);
                    break;

                case ReceiptHeader.Index.AdditionalCostCurrency:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.AdditionalCostCurrency, model.AdditionalCostCurrency,
                        true);
                    break;

                case ReceiptHeader.Index.RateType:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.RateType, model.RateType, true);
                    break;

                case ReceiptHeader.Index.ExchangeRate:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.ExchangeRate, model.ExchangeRate, true);
                    break;

                case ReceiptHeader.Index.VendorNumber:
                    _receiptHeaderEntity.SetValue(ReceiptHeader.Index.VendorNumber, model.VendorNumber, true);
                    break;

            }
            var receiptHeaderMapper = new ReceiptHeaderMapper(Context);
            var header = receiptHeaderMapper.Map(_receiptHeaderEntity);
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
        public virtual EnumerableResponse<ReceiptOptionalField> GetOptionalField(int pageNumber, int pageSize,
            Expression<Func<ReceiptOptionalField, bool>> filter = null)
        {
            IsSessionAvailable();
            return RetrieveOptionalField(pageNumber, pageSize, 0);
        }

        /// <summary>
        /// Save Optional Field detail.
        /// </summary>
        /// <param name="optionalFieldDetails">Optional Fields details.</param>
        /// <param name="receiptNumber">Receipt Number</param>
        public virtual EnumerableResponse<ReceiptOptionalField> SaveOptionalFields(IEnumerable<ReceiptOptionalField> optionalFieldDetails, string receiptNumber)
        {
            IsSessionAvailable();
            _receiptHeaderEntity.SetValue(ReceiptHeader.Index.ReceiptNumber, receiptNumber);
            _receiptHeaderEntity.Read(false);
            SyncOptionalFields(optionalFieldDetails);
            _receiptHeaderEntity.Update();
            return GetOptionalField(0, 10);
        }

        /// <summary>
        /// Refreshes the Detail
        /// </summary>
        /// <param name="detail">ReceiptDetail model</param>
        /// <param name="eventType">Property that changed</param>
        /// <returns>ReceiptDetail model</returns>
        public virtual ReceiptHeader RefreshDetail(ReceiptDetail detail, string eventType)
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
            var headerData = _receiptMapper.Map(_receiptHeaderEntity);
            headerData.ReceiptDetail = new EnumerableResponse<ReceiptDetail>
            {
                Items = new List<ReceiptDetail> { detailData }
            };
            return headerData;
        }

        /// <summary>
        /// Refreshes the header when the optional field
        /// </summary>
        /// <returns>returns optional field value</returns>
        public virtual string RefreshOptField()
        {
            var optCount = _receiptHeaderEntity.GetValue<string>(ReceiptHeader.Fields.OptionalFields);
            return optCount;
        }

        #region OptionalFields

        /// <summary>
        /// Gets the optional field finder data.
        /// </summary>
        /// <param name="optionalField">The optional field.</param>
        /// <returns>Header Optional Field model</returns>
        public virtual ReceiptOptionalField GetOptionalFieldFinderData(string optionalField)
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
        public virtual ReceiptDetailOptionalField GetDetailOptFieldFinderData(string optionalField)
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
        public virtual ReceiptOptionalField SetOptionalFieldValue(ReceiptOptionalField optionalField)
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
        public virtual ReceiptDetailOptionalField SetOptionalFieldValue(ReceiptDetailOptionalField optionalField)
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
        public virtual EnumerableResponse<ReceiptOptionalField> GetOptFields(int pageNumber, int pageSize)
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
        public virtual bool SaveDetailOptFields(IEnumerable<ReceiptDetailOptionalField> optionalFieldDetails, string receiptNumber, bool isDetail)
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
        public virtual EnumerableResponse<ReceiptDetailOptionalField> GetDetailOptFields(int pageNumber, int pageSize)
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
        private EnumerableResponse<ReceiptDetailOptionalField> RetrieveDetailOptField(int currentPageNumber, int pageSize,
            IBusinessEntity entity, ModelMapper<ReceiptDetailOptionalField> mapper, int filterCount)
        {
            CheckRights(GetAccessRights(), SecurityType.Inquire);

            var optionalFieldList = new List<ReceiptDetailOptionalField>();
            var startIndex = CommonUtil.ComputeStartIndex(currentPageNumber, pageSize);
            var endIndex = CommonUtil.ComputeEndIndex(currentPageNumber, pageSize, filterCount);
            var loopCounter = 1;
            if (!entity.Top())
                return new EnumerableResponse<ReceiptDetailOptionalField>
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
            return new EnumerableResponse<ReceiptDetailOptionalField>
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
        private bool SyncDetailOptFields(IEnumerable<ReceiptDetailOptionalField> detailOptionalFields, bool isDetail)
        {
            var isUpdated = false;
            if (detailOptionalFields == null)
            {
                //This is to update when there are no records and to cover AutoInsert error, if exists
                UpdateHeader(isDetail);
                return false;
            }
            var allDetails = detailOptionalFields as IList<ReceiptDetailOptionalField> ?? detailOptionalFields.ToList();

            foreach (var detail in allDetails.Where(detail => (detail.HasChanged || detail.IsDeleted) && !detail.IsNewLine))
            {
                isUpdated = isDetail ? SyncDetailOptField(detail) : SyncOptField(detail as ReceiptOptionalField);
                //UpdateHeader(isDetail);
            }
            foreach (var newLine in allDetails.Where(detail => detail.IsNewLine && detail.OptionalField != null))
            {
                isUpdated = isDetail ? InsertDetailOptField(newLine) : InsertOptField(newLine as ReceiptOptionalField);
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
                if (_receiptHeaderEntity.Exists)
                {
                    _receiptHeaderEntity.Update();
                    Helper.GetExceptions(Session);
                }
            }
        }

        /// <summary>
        /// To set fields for Detail Optional Fields
        /// </summary>
        /// <param name="optionalField">Optional Fields</param>
        private bool SyncDetailOptField(ReceiptDetailOptionalField optionalField)
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
        private bool InsertDetailOptField(ReceiptDetailOptionalField newLine)
        {
            var optionalFieldMapper = new ReceiptDetailOptionalFieldMapper(Context);
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
        private EnumerableResponse<ReceiptOptionalField> RetrieveOptField(int currentPageNumber, int pageSize,
            IBusinessEntity entity, ModelMapper<ReceiptOptionalField> mapper, int filterCount)
        {
            CheckRights(GetAccessRights(), SecurityType.Inquire);

            var optionalFieldList = new List<ReceiptOptionalField>();
            var startIndex = CommonUtil.ComputeStartIndex(currentPageNumber, pageSize);
            var endIndex = CommonUtil.ComputeEndIndex(currentPageNumber, pageSize, filterCount);
            var loopCounter = 1;
            var lineNumber = 0;
            if (!entity.Top())
                return new EnumerableResponse<ReceiptOptionalField>
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
            return new EnumerableResponse<ReceiptOptionalField>
            {
                Items = optionalFieldList,
                TotalResultsCount = filterCount != 0 ? filterCount : GetOptionalFieldsCount(entity)
            };
        }

        /// <summary>
        /// To set fields for Optional Fields
        /// </summary>
        /// <param name="optionalField">Optional Fields</param>
        private bool SyncOptField(ReceiptOptionalField optionalField)
        {
            var optionalFieldMapper = new ReceiptOptionalFieldMapper(Context);
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
        private bool InsertOptField(ReceiptOptionalField newLine)
        {
            var optionalFieldMapper = new ReceiptOptionalFieldMapper(Context);
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
        public ReceiptHeader Refresh(ReceiptHeader model)
        {
            var receiptMapper = new ReceiptMapper(Context);
            IsSessionAvailable();
            receiptMapper.Map(model, new List<IBusinessEntity> { _receiptHeaderEntity });

            var headerModel =
                receiptMapper.Map(new List<IBusinessEntity>
                {
                    _receiptHeaderEntity,
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
        public virtual ReceiptHeader ReadHeader(ReceiptHeader model)
        {
            if (model != null)
            {
                _receiptMapper.Map(model, _receiptHeaderEntity);
            }
            var headerModel = _receiptMapper.Map(_receiptHeaderEntity);
            headerModel.HomeCurrency = Session.HomeCurrency;
            headerModel.Warnings = Helper.GetExceptions(Session);
            return headerModel;
        }

        /// <summary>
        /// Additional Access Check for Export and Import
        /// </summary>
        /// <returns>User Access</returns>
        public UserAccess GetAccessRights()
        {
            var userAccess = base.GetAccessRights(_receiptHeaderEntity);

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
        public virtual ReceiptHeader GetDefaultDetailOptField()
        {
            DetailProcess();
            var detailData = _receiptDetailMapper.Map(_receiptDetailEntity);

            var headerData = _receiptMapper.Map(_receiptHeaderEntity);
            headerData.ReceiptDetail = new EnumerableResponse<ReceiptDetail>
            {
                Items = new List<ReceiptDetail> { detailData }
            };
            return headerData;
        }

        /// <summary>
        /// Find whether Internal Usage record with Internal Usage Number passed exists
        /// </summary>
        /// <param name="id">An Internal Usage Number</param>
        /// <param name="model">A model to save the current data</param>
        /// <returns>Returns True if exists, False otherwise</returns>
        public virtual bool Exists(string id, ReceiptHeader model)
        {
            _receiptHeaderEntity.SetValue(ReceiptHeader.Fields.ReceiptNumber, id);
            var exists = _receiptHeaderEntity.Exists;
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
        /// Gets the total records.
        /// </summary>
        /// <param name="businessEntity">The business entity.</param>
        /// <returns>Number of total records</returns>
        public int GetTotalRecords(IBusinessEntity businessEntity)
        {
            var total = 0;
            businessEntity.Top();
            do
            {
                if (businessEntity.Read(false))
                {
                    total++;
                }

            } while (businessEntity.Next());
            if (!businessEntity.Exists && total == 1)
            {
                return 0;
            }
            return total;
        }

        /// <summary>
        /// Sync Detail Records
        /// </summary>
        /// <param name="detail">Receipt Detail Model</param>
        public bool SyncDetail(ReceiptDetail detail)
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
        /// Save Details
        /// </summary>
        /// <param name="details">Details</param>
        /// <returns>True if successfully saved, false otherwise</returns>
        public bool SaveDetails(IEnumerable<ReceiptDetail> details)
        {
            return SyncDetailsModels(details);
        }

        /// <summary>
        /// Creates a new Detail
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Number of records to be retrieved per page</param>
        /// <param name="currentDetail">The current detail.</param>
        /// <returns>New detail</returns>
        public ReceiptHeader NewDetail(int pageNumber, int pageSize, ReceiptDetail currentDetail)
        {
            return NewDetailModels(currentDetail);
        }

        /// <summary>
        /// Sets pointer to the current Detail
        /// </summary>
        /// <param name="currentDetail">The current detail.</param>
        /// <returns>Model with newly set detail</returns>
        public ReceiptHeader SetDetail(ReceiptDetail currentDetail)
        {
            if (currentDetail != null)
            {
                _receiptDetailMapper.MapKey(currentDetail, _receiptDetailEntity);
                if (_receiptDetailEntity.Exists)
                {
                    _receiptDetailEntity.Read(false);
                    _receiptDetailMapper.Map(currentDetail, _receiptDetailEntity);
                }
            }
            return _mapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity, _receiptDetailEntity });
        }

        /// <summary>
        /// Save for detail Entry
        /// </summary>
        /// <param name="detail">Detail model</param>
        /// <returns>Saved detail</returns>
        public ReceiptHeader SaveDetail(ReceiptDetail detail)
        {
            if (detail.IsNewLine)
            {
                InsertDetailModels(detail);
            }
            else
            {
                SyncDetailModels(detail);
            }
            return _mapper.Map(new List<IBusinessEntity> { _receiptHeaderEntity, _receiptDetailEntity });
        }

        #endregion

        #region Export Import

        /// <summary>
        /// Get export or import business entity property
        /// </summary>
        /// <param name="option">export/import option, default to null</param>
        /// <param name="isExport">true if for export, default to false</param>
        /// <returns>business entity property</returns>
        public override BusinessEntityProperty GetExportImportBusinessEntityProperty(string option = null, bool isExport = false)
        {
            var receiptHeader = new BusinessEntityProperty(ReceiptHeader.EntityName, ViewKeyType.SystemGenerated);
            var receiptDetail = new BusinessEntityProperty(ReceiptDetail.EntityName, ViewKeyType.SystemGenerated);
            var receiptHeaderOptionalfield = new BusinessEntityProperty(ReceiptOptionalField.EntityName, ViewKeyType.UserSpecified);
            var receiptDetailOptionalfield = new BusinessEntityProperty(ReceiptDetailOptionalField.EntityName, ViewKeyType.UserSpecified);
            var receiptDetailSerialnumber = new BusinessEntityProperty(ReceiptDetailSerialNumber.EntityName, ViewKeyType.UserSpecified);
            var receiptDetailLotnumber = new BusinessEntityProperty(ReceiptDetailLotNumber.EntityName, ViewKeyType.UserSpecified);

            receiptHeader.AddDetail(receiptDetail);
            receiptHeader.AddDetail(receiptHeaderOptionalfield);
            receiptDetail.AddDetail(receiptDetailOptionalfield);
            receiptDetail.AddDetail(receiptDetailSerialnumber);
            receiptDetail.AddDetail(receiptDetailLotnumber);

            return receiptHeader;
        }

        /// <summary>
        /// Get import types, i.e., insert, update, insert/update, etc.
        /// We only allow insert operation for batch type transactions.
        /// </summary>
        /// <param name="option">import option, default to null</param>
        /// <returns>a list of import types</returns>
        public override IEnumerable<ImportType> GetImportTypes(string option = null)
        {
            return new List<ImportType> { ImportType.Insert };
        }

        /// <summary>
        /// Set any additional properties for import/export
        /// </summary>
        /// <param name="additionalProperties">additional properties to set</param>
        /// <param name="option">import option, default to null</param>
        /// <param name="isExport">true if for export, default to false</param>
        /// <returns>addtional properties</returns>
        public override void SetExportImportAdditionalProperties(dynamic additionalProperties, string option = null, bool isExport = false)
        {
            // Note: Some of the OE/IC/PO views will suppress errors if the verify flag of view.blkput is not set.
            //       In such cases we want to turn on the flag (set VerifyOnPut to true) and get the error messages.
            additionalProperties.VerifyOnPut = true;
        }

        #endregion
    }
}