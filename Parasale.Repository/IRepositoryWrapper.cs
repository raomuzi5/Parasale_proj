namespace Parasale.Repository
{
    public interface IRepositoryWrapper
    {
        IObjectionLogRepository ObjectionLogRepository { get; set; }
        IObjectionRepository ObjectionRepository { get; set; }
        IParasaleRepository ParasaleRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        IObjectionNotificationRepository ObjectionNotificationRepository { get; set; }
        ICollectionRepository CollectionRepository { get; set; }
        IQuickCollectionRepository QuickCollectionRepository { get; set; }
        IQuickObjectionRepository QuickObjectionRepository { get; set; }
        ICCSRepository CCSRepository { get; set; }
        IInvitiesRepository InvitesRepository { get; set; }
        ISessionRepository SessionRepository { get; set; }
        IUserHistory UserHistory { get; set; }
        IVoiceOnboardingData VoiceOnboarding { get; set; }
        //IVoiceOnBoardingSubSteps VoiceOnboardingSubSteps { get; set; }
        //IUserHistory UserHistory { get; set; }
        IAuditLogRepository AuditLogRepository { get; set; }

    }
}