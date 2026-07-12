namespace TaskManagerApp.InterfacesServices
{
    public interface IEmailCodeService
    {
        public Task<bool> VerifyEmail(string email, string code);
        public Task SendVerificationCode(string email);
        public Task DeleteCodes();
    }
}
