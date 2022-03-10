using _0_Framework.Application;
using DiscountManagement.Application.Contract.ColleagueDiscount;
using DiscountManagement.Domain.ColleagueDiscountAgg;
using System;
using System.Collections.Generic;

namespace DiscountManagement.Application
{
    public class ColleagueDiscountApplication : IColleagueDiscountApplication
    {
        private readonly IColleagueDiscountRepository colleagueDiscountRepository;

        public ColleagueDiscountApplication(IColleagueDiscountRepository colleagueDiscountRepository)
        {
            this.colleagueDiscountRepository = colleagueDiscountRepository;
        }

        public OperationResult Define(DefineColleagueDiscount command)
        {
            var operation = new OperationResult();
            if (colleagueDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var colleagueDiscount = new ColleagueDiscount(command.ProductId, command.DiscountRate);

            colleagueDiscountRepository.Create(colleagueDiscount);
            colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditColleagueDiscount command)
        {
            var operation = new OperationResult();
            var colleagueDiscount = colleagueDiscountRepository.Get(command.Id);
            if (colleagueDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (colleagueDiscountRepository.Exists(x => x.ProductId == command.ProductId && x.DiscountRate == command.DiscountRate && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            colleagueDiscount.Edit(command.ProductId, command.DiscountRate);

            colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditColleagueDiscount GetDetails(long id)
        {
            return colleagueDiscountRepository.GetDetails(id);
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();
            var colleagueDiscount = colleagueDiscountRepository.Get(id);
            if (colleagueDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            colleagueDiscount.Remove();

            colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();
            var colleagueDiscount = colleagueDiscountRepository.Get(id);
            if (colleagueDiscount == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            colleagueDiscount.Restore();

            colleagueDiscountRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel)
        {
            return colleagueDiscountRepository.Search(searchModel);
        }
    }
}
