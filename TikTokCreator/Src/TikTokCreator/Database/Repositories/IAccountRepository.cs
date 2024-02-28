using TikTokCreator.Entities;

namespace TikTokCreator.Repositories
{
    public interface IAccountRepository
    {
        void Add(Account account);
        List<Account> GetAll();
        void UpdateEmail(Account account);
        Account GetById(Guid id);
        void Delete(Guid id);
        bool DeleteRange(List<string> deleteList);
        void DeleteAll();
        bool UpdateStatusByIdAccount(Guid idAccount, string status);
        bool UpdateFullnameByIdAccount(Guid idAccount, string? fullname);
        bool UpdateUsernameByIdAccount(Guid idAccount, string? username);
        bool UpdatePasswordByIdAccount(Guid idAccount, string? password);
        bool UpdateProxyByIdAccount(Guid idAccount, string? proxy);
        bool UpdateTowFAByIdAccount(Guid idAccount, string? towFA);
        bool UpdateAvatarByIdAccount(Guid idAccount, string avatar);
        bool UpdateBackupByIdAccount(Guid idAccount, string? fileBackup);
    }
}
