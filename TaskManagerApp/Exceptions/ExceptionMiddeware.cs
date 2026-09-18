namespace TaskManagerApp.Exceptions
{
    public class ExceptionMiddeware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddeware(RequestDelegate next)
        {
            _next = next;
        }

        private static Task HandleError(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new { error = message });
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UserNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch (IncorectEmailFormatException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (IncorectPasswordFormatException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (EmailorPasswordNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch (UserCodeNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch (IncorectTaskItemNameException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (TaskItemNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(UserNameAlreadyExistsException ex)
            {
                await HandleError(context, 409, ex.Message);
            }
            catch (UserEmailAlreadyExistsException ex)
            {
                await HandleError(context, 409, ex.Message);
            }
            catch (InvalidVerificationCodeException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (EmailNotVerifiedException ex)
            {
                await HandleError(context, 403, ex.Message);
            }
            catch (EmptyTaskNameException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(IncorrectTaskEndDateException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (NoTaskCategoryException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(NoTaskEndDateException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (NoTaskStartDateException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(TaskAlreadyCompletedException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(TaskAlreadyOverdueException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(UnnecessaryUpdateOperationException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(UserAlreadyHasThisTaskException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (UserDoesntHaveTasksException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (UserNotAssignedToTaskException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (ExceededNumberOfInvitesForOneDayException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (YouAreAlreadyFriendsException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (InviteIsStillPendingException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (TheUserHasBlockedYouException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(CantJoinTaskHasEndedException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(CantSendTaskHasEndedException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(FriendHasAlreadyAcceptedThisTaskException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(FriendHasDeclinedTheInvitationException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(IdenticalUserIdException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(RelationAlreadyExistsException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(RelationNotFoundException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(TaskDoesntHaveSharedUsersException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(TooManyFailedLoginAttemptsException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(UserDoesntHaveSharedTasksException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(YouAreNotInvitedForThisTaskException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(YouHaveAlreadyAnsweredThisInviteException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (YouAreNotFriendsWithThisUserException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(ChallengeNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(ChallengeAlreadyCompletedException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (UserHasNotJoinedTheChallengeException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(ChallengeHasNotStartedException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch (CategoryNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(NoChallengesFoundForThisCategoryException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(ThereAreNoChallengesForThisCategoryException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(UserHasntJoinedAnyChallengesException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(UserHasntCompletedAnyChallengesException ex)
            {
                await HandleError(context, 400, ex.Message);
            }
            catch(RankNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch(StateNotFoundException ex)
            {
                await HandleError(context, 404, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleError(context, 500, ex.Message);
            }
           
        }
    }
}
