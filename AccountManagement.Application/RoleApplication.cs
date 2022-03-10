﻿using _0_Framework.Application;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;
using System.Collections.Generic;

namespace AccountManagement.Application
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IRoleRepository roleRepository;

        public RoleApplication(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public OperationResult Create(CreateRole command)
        {
            var operation = new OperationResult();
            if (roleRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var role = new Role(command.Name, new List<Permission>());
            roleRepository.Create(role);
            roleRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditRole command)
        {
            var operation = new OperationResult();
            var role = roleRepository.Get(command.Id);
            if (role == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (roleRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var permissions = new List<Permission>();
            command.Permissions.ForEach(code => permissions.Add(new Permission(code)));

            role.Edit(command.Name, permissions);
            roleRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditRole GetDetails(long id)
        {
            return roleRepository.GetDetails(id);
        }

        public List<RoleViewModel> List()
        {
            return roleRepository.List();
        }
    }
}