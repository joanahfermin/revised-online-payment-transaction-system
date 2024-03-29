﻿using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.Service
{
    internal interface IBusinessService
    {
        Business Get(object id);
        List<Business> RetrieveBySearchKeyword(string billNumber);
        List<Business> RetrieveBySearchKeywordForPaymentValidationOnly(string billNumber);


        void Insert(List<Business> business);
        void Insert(List<Business> business, bool validate);
        void Update(Business business);

        void RevertSelectedRecordStatus(List<Business> business);
        void UpdateSelectedRecordsStatus(List<Business> businessList, string status);

        void validateBusinessDuplicateRecord(List<Business> listOfBusToSave);
        List<Business> RetrieveBySameRefNum(string refNum);
        void DeleteAll(List<Business> busToDelete);
        List<Business> RetrieveNoName();

    }
}
