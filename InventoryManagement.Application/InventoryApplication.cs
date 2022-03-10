using _0_Framework.Application;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using System.Collections.Generic;

namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        private readonly IAuthHelper authHelper;
        private readonly IInventoryRepository inventoryRepository;

        public InventoryApplication(IInventoryRepository inventoryRepository, IAuthHelper authHelper)
        {
            this.inventoryRepository = inventoryRepository;
            this.authHelper = authHelper;
        }

        public OperationResult Create(CreateInventory command)
        {
            var operation = new OperationResult();
            if (inventoryRepository.Exists(x => x.ProductId == command.ProductId))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var inventory = new Inventory(command.ProductId, command.UnitPrice);
            inventoryRepository.Create(inventory);
            inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditInventory command)
        {
            var operation = new OperationResult();
            var inventory = inventoryRepository.Get(command.Id);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            inventory.Edit(command.ProductId, command.UnitPrice);
            inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditInventory GetDetails(long id)
        {
            return inventoryRepository.GetDetails(id);
        }

        public List<InventoryOperationViewModel> GetOperationLog(long inventoryId)
        {
            return inventoryRepository.GetOperationLog(inventoryId);
        }

        public OperationResult Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();
            var inventory = inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            const long operatorId = 1;
            inventory.Increase(command.Count, operatorId, command.Description);
            inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Reduce(ReduceInventory command)
        {
            var operation = new OperationResult();
            var inventory = inventoryRepository.Get(command.InventoryId);
            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            var operatorId = authHelper.CurrentAccountId();
            inventory.Reduce(command.Count, operatorId, command.Description, 0);
            inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Reduce(List<ReduceInventory> command)
        {
            var operation = new OperationResult();
            var operatorId = authHelper.CurrentAccountId();
            foreach (var item in command)
            {
                var inventory = inventoryRepository.GetBy(item.ProductId);
                inventory.Reduce(item.Count, operatorId, item.Description, item.OrderId);
            }

            inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return inventoryRepository.Search(searchModel);
        }
    }
}