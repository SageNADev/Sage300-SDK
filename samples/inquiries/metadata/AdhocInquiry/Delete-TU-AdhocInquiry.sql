-- Copyright (c) 1994-2018 Sage Software, Inc.  All rights reserved.
-- Delete-TU-AdhocInquiry.sql

BEGIN
DELETE InquiryTemplate WHERE [TemplateId] = '94c77ad4-8629-488b-9298-d93984552179';

DELETE InquiryTemplate WHERE [TemplateId] = '94c77ad4-8629-488b-9298-d93984552178';

END
GO


BEGIN
DELETE FROM InquiryDataSource WHERE [DataSourceId] = '35bc6e80-52a1-4704-a0d9-2b6d6a7d05a9';

DELETE FROM InquiryDataSource WHERE [DataSourceId] = '35bc6e80-52a1-4704-a0d9-2b6d6a7d05a8';

END
GO
