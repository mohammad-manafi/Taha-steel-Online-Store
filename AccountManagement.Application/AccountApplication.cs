using _0_Framework.Application;
using AccountManagement.Application.Contracts.Account;
using AccountManagement.Domain.AccountAgg;
using System.Collections.Generic;
using System.Linq;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IFileUploader fileUploader;
        private readonly IPasswordHasher passwordHasher;
        private readonly IAccountRepository accountRepository;
        private readonly IAuthHelper authHelper;
        private readonly IRoleRepository roleRepository;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher,
            IFileUploader fileUploader, IAuthHelper authHelper, IRoleRepository roleRepository)
        {
            this.authHelper = authHelper;
            this.roleRepository = roleRepository;
            this.fileUploader = fileUploader;
            this.passwordHasher = passwordHasher;
            this.accountRepository = accountRepository;
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (command.Password != command.RePassword)
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);

            var password = passwordHasher.Hash(command.Password);
            account.ChangePassword(password);
            accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public AccountViewModel GetAccountBy(long id)
        {
            var account = accountRepository.Get(id);
            return new AccountViewModel()
            {
                Fullname = account.Fullname,
                Mobile = account.Mobile
            };
        }

        public OperationResult Register(RegisterAccount command)
        {
            var operation = new OperationResult();

            if (accountRepository.Exists(x => x.Username == command.Username || x.Mobile == command.Mobile))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var password = passwordHasher.Hash(command.Password);
            var path = $"profilePhotos";
            var picturePath = fileUploader.Upload(command.ProfilePhoto, path);
            var account = new Account(command.Fullname, command.Username, password, command.Mobile, command.RoleId,
                picturePath);
            accountRepository.Create(account);
            accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = accountRepository.Get(command.Id);
            if (account == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (accountRepository.Exists(x =>
                (x.Username == command.Username || x.Mobile == command.Mobile) && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var path = $"profilePhotos";
            var picturePath = fileUploader.Upload(command.ProfilePhoto, path);
            account.Edit(command.Fullname, command.Username, command.Mobile, command.RoleId, picturePath);
            accountRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditAccount GetDetails(long id)
        {
            return accountRepository.GetDetails(id);
        }

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();
            var account = accountRepository.GetBy(command.Username);
            if (account == null)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var result = passwordHasher.Check(account.Password, command.Password);
            if (!result.Verified)
                return operation.Failed(ApplicationMessages.WrongUserPass);

            var permissions = roleRepository.Get(account.RoleId)
                .Permissions
                .Select(x => x.Code)
                .ToList();

            var authViewModel = new AuthViewModel(account.Id, account.RoleId, account.Fullname
                , account.Username, account.Mobile, permissions);

            authHelper.Signin(authViewModel);
            return operation.Succedded();
        }

        public void Logout()
        {
            authHelper.SignOut();
        }

        public List<AccountViewModel> GetAccounts()
        {
            return accountRepository.GetAccounts();
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return accountRepository.Search(searchModel);
        }
    }
}