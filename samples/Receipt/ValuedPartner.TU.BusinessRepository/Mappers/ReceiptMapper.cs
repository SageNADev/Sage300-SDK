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

#region Namespaces

using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository;
using Sage.CA.SBS.ERP.Sage300.Common.BusinessRepository.Utilities;
using Sage.CA.SBS.ERP.Sage300.Common.Interfaces.Entity;
using Sage.CA.SBS.ERP.Sage300.Common.Models;
using Sage.CA.SBS.ERP.Sage300.Common.Utilities;
using ValuedPartner.TU.Models;
using System.Collections.Generic;

#endregion

namespace ValuedPartner.TU.BusinessRepository.Mappers
{
    /// <summary>
    /// Class for mapping Receipt header and Receipt details.
    /// </summary>
    public class ReceiptMapper: ModelHierarchyMapper<ReceiptHeader> 
    {
        #region Private Properties

        /// <summary>
        /// Gets or sets Context.
        /// </summary>
        private Context Context { get; set; }

        /// <summary>
        /// The _header mapper
        /// </summary>
        private readonly ReceiptHeaderMapper _headerMapper;

        /// <summary>
        /// The _detail mapper
        /// </summary>
        private readonly ReceiptDetailMapper _detailMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize the mappers.
        /// </summary>
        /// <param name="context">Context</param>
        public ReceiptMapper(Context context)
        {
            Context = context;
            _headerMapper = new ReceiptHeaderMapper(Context);
            _detailMapper = new ReceiptDetailMapper(Context);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps header.
        /// </summary>
        /// <param name="model">Receipt Header Model</param>
        /// <param name="entity">Business Entity</param>
        public override void Map(ReceiptHeader model, List<IBusinessEntity> entity)
        {
            _headerMapper.Map(model, entity[0]);
        }

        /// <summary>
        /// Map Data to Header and Details entity
        /// </summary>
        /// <param name="entities">Business Entities</param>
        /// <param name="detailPageNumber">Page Number</param>
        /// <param name="detailPageSize">Page Size</param>
        /// <param name="filterCount">Filter Count</param>
        /// <returns>Receipt Header Model</returns> 
        public override ReceiptHeader Map(List<IBusinessEntity> entities, int? detailPageNumber = null, int? detailPageSize = null, int? filterCount = null)
        { 
            var model = _headerMapper.Map(entities[0]);
            model.ReceiptDetail = new EnumerableResponse<ReceiptDetail>();
            var receiptDetail = new List<ReceiptDetail>();
            var startIndex = CommonUtil.ComputeStartIndex(detailPageNumber, detailPageSize);
            var endIndex = CommonUtil.ComputeEndIndex(detailPageNumber, detailPageSize, filterCount);
            var loopCounterEntity1 = 1;
            var lineNumber = startIndex != 0 ? startIndex : 1;
            int totalDetailRecords;

            // Map detail entities based on the page number and page size
            if (detailPageNumber.HasValue && detailPageSize.HasValue)
            {
                entities[1].Top();

                do
                {
                    if (loopCounterEntity1 >= startIndex)
                    {
                        lineNumber = AddReceiptDetail(entities[1], lineNumber, receiptDetail);
                    }
                    loopCounterEntity1++;
                } while (loopCounterEntity1 <= endIndex && entities[1].Next());

                totalDetailRecords = filterCount != null && filterCount.Value != 0 ? filterCount.Value: Helper.GetRecordCount(entities[1]);
            }
            else
            {
                receiptDetail.Add(_detailMapper.Map(entities[1]));
                totalDetailRecords = filterCount != null && filterCount.Value != 0 ? filterCount.Value : 0;
            }

            model.ReceiptDetail.Items = receiptDetail;
            model.ReceiptDetail.TotalResultsCount = totalDetailRecords;
            return (ReceiptHeader)model; 
        }

        #endregion

        #region Private Variables

        /// <summary>
        /// Method to Add Receipt detail.
        /// </summary>
        /// <param name="detailEntity">Detail Entity</param>
        /// <param name="lineNumber">Line Number</param>
        /// <param name="receiptDetail">Receipt Detail</param>
        /// <returns>Line Number</returns>
        private int AddReceiptDetail(IBusinessEntity detailEntity, int lineNumber, List<ReceiptDetail> receiptDetail)
        {
            var firstItem = _detailMapper.Map(detailEntity);
            // Set the pointer to the current row
            _detailMapper.MapKey(firstItem, detailEntity);
            var isSuccess = detailEntity.Read(false);

            firstItem.LineNumber = lineNumber;
            if (isSuccess)
            {
                receiptDetail.Add(firstItem);
            }

            lineNumber++;
            return lineNumber;
        }

        #endregion
    }
}
