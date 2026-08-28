using System.Net;
using System.Net.Mail;
using TaskManagerApp.Data.Models;
using TaskManagerApp.DTOS;
using TaskManagerApp.InterfacesRepositories;
using TaskManagerApp.InterfacesServices;

namespace TaskManagerApp.Services
{
    public class EmailService : IEmailService
    {
        readonly IEmailCodeRepository _emailCodeRepository;
        public EmailService(IEmailCodeRepository emailCodeRepository)
        {
            _emailCodeRepository = emailCodeRepository;
        }

        public async Task<bool> VerifyEmail(string email, string code)
        {
            var emailCode = await _emailCodeRepository.GetValidCodeAsync(email, code);

            if (emailCode == null)
                return false;

            await _emailCodeRepository.MarkAsUsedAsync(emailCode);

            return true;
        }

        public async Task SendVerificationCode(string email)
        {
            var code = new EmailCode
            {
                Email = email,
                Code = Random.Shared.Next(100000, 999999).ToString(),
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };


            await _emailCodeRepository.AddAsync(code);


            var message = new MailMessage();

            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{email}");
            message.Subject = "Verification Code";
            message.Body = $"Your verification code is {code.Code}";

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task DeleteCodes()
        {
            await _emailCodeRepository.DeleteExpiredCodesAsync();
        }


        //TODO: BETTER EMAIL DESIGN SHOULD BE IMPLEMENTED
        public async Task SendReminderTaskEmail(string username, string email, TaskItem taskItem)
        {
        
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{email}");
            var html = await File.ReadAllTextAsync("Templates/ReminderTaskEmail.html");

            // Replace placeholders with actual values
            html = html.Replace("{{UserName}}", username);
            html = html.Replace("{{TaskName}}", taskItem.Name);
            html = html.Replace("{{StartTime}}", taskItem.StartDate.ToString("yyyy-MM-dd HH:mm"));

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        //TODO: BETTER EMAIL DESIGN SHOULD BE IMPLEMENTED
        public async Task SendEmailForNewChallenges(string username, string email, List<Challenge> challenges)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{email}");
            var html = await File.ReadAllTextAsync("Templates/NewChallengesEmail.html");
            // Replace placeholders with actual values
            html = html.Replace("{{UserName}}", username);

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task SendReminderForStreakEmail(User user)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.To.Add($"{user.Email}");
            var html = await File.ReadAllTextAsync("Templates/StreakReminderEmail.html");
            // Replace placeholders with actual values
            html = html.Replace("{{UserName}}", user.Username);

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task SendFriendRequestToTheRelatedUserEmail(UserDto initiator, UserDto receiver)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.Subject = "You have a new friend request!";
            message.To.Add($"{receiver.Email}");
            var html = await File.ReadAllTextAsync("Templates/FriendRequestToTheRelatedUserEmail.html");

            // Replace placeholders with actual values
            html = html.Replace("{{UserName}}", receiver.Username);
            html = html.Replace("{{InitiatorName}}", initiator.Username);
            //URL should be changed to the actual login page of the application
            html = html.Replace("{{LoginUrl}}", "https://yourapp.com/login");

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task AnswerFriendRequestInformInitiatorEmail(UsersRelationsDto usersRelationsDto)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.Subject = "Answer received for your friend request!";
            message.To.Add($"{usersRelationsDto.Initiator.Email}");
            var html = await File.ReadAllTextAsync("Templates/AnswerFriendRequestInformInitiatorEmail.html");

            // Replace placeholders with actual values
            html = html.Replace("{{InitiatorName}}", usersRelationsDto.Initiator.Username);
            html = html.Replace("{{ReceiverName}}", usersRelationsDto.RelatedUser.Username);
            string relationStatus;
            if (usersRelationsDto.RelationStatus == RelationStatus.Accepted)
            {
                relationStatus = "accepted";
            }
            else if (usersRelationsDto.RelationStatus == RelationStatus.Rejected)
            {
                relationStatus = "rejected";
            }
            else
            {
                relationStatus = "blocked";
            }
            html = html.Replace("{{Response}}", relationStatus);

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task UpdateRelationShipStatusInformBothEmail(string email, string initiatorName, string reciverName, RelationType relationType)
        {

            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.Subject = "Your relationship status has been updated!";
            message.To.Add($"{email}");

            var html = await File.ReadAllTextAsync("Templates/UpdateRelationShipStatusInformBothEmail.html");

            // Replace placeholders with actual values
            html = html.Replace("{{InitiatorName}}", initiatorName);
            html = html.Replace("{{RecipientName}}", reciverName);
            string relation;
            if (relationType == RelationType.Friend)
            {
                relation = "Friends";
            }
            else if (relationType == RelationType.Unfriend)
            {
                relation = "Unfriended";
            }
            else
            {
                relation = "Blocked";
            }
            html = html.Replace("{{NewStatus}}", relation);

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task InviteFriendToTaskEmail(string friendEmail, string friendName, string ownerName, string taskName, DateTime taskStartDate)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.Subject = "You have been invited to a task!";
            message.To.Add($"{friendEmail}");

            var html = await File.ReadAllTextAsync("Templates/InviteFriendToTaskEmail.html");

            // Replace placeholders with actual values
            html = html.Replace("{{ReceiverName}}", friendName);
            html = html.Replace("{{InitiatorName}}", ownerName);
            html = html.Replace("{{TaskName}}", taskName);
            html = html.Replace("{{TaskStartDate}}", taskStartDate.ToString("MM-dd H:mm"));

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

        public async Task AnswerTaskInviteInforOwner(string ownerEmail, string ownerName, string friendName, string taskName, DateTime taskStartDate, Status status)
        {
            var message = new MailMessage();
            message.From = new MailAddress("oginik7@gmail.com");
            message.Subject = "Your Task Invite Response";
            message.To.Add($"{ownerEmail}");

            var html = await File.ReadAllTextAsync("Templates/AnswerTaskInviteInformOwner.html");

            // Replace placeholders with actual values
            html = html.Replace("{{OwnerName}}", ownerName);
            html = html.Replace("{{ReceiverName}}", friendName);
            html = html.Replace("{{TaskName}}", taskName);
            html = html.Replace("{{TaskStartDate}}", taskStartDate.ToString("MM-dd H:mm"));

            string response;
            if (status == Status.Accepted)
            {
                response = "accepted";
            }
            else
            {
                response = "declined";
            }
            html = html.Replace("{{Response}}", response);

            message.IsBodyHtml = true;
            message.Body = html;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(
                    "oginik7@gmail.com",
                    "dvvx jezo khrw poau"
                );

                smtp.EnableSsl = true;

                smtp.Send(message);
            }
        }

    }
   
}
