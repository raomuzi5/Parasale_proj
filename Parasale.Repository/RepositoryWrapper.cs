using Parasale.Data;

namespace Parasale.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ParasaleDbContext _context;
        public RepositoryWrapper(ParasaleDbContext context)
        {
            _context = context;
        }
        private IObjectionRepository _objectionRepository;
        private IObjectionLogRepository _objectionLogRepository;
        private IUserRepository _userRepository;
        private IParasaleRepository _parasaleRepository;
        private IObjectionNotificationRepository _objectionNotificationRepository;
        private ICollectionRepository _collectionRepository;
        private IAuditLogRepository _aLogRepository;
        private IQuickCollectionRepository _qcollectionRepository;
        private IInvitiesRepository _invitesRepository;
        private IQuickObjectionRepository _quickObjectionRepository;
        private ICCSRepository _ccsRepository;
        private ISessionRepository _sessionRepository;
        private IUserHistory _userHistory;
        private IVoiceOnboardingData _IVoiceOnboardingData;
        //private IVoiceOnBoardingSubSteps _IVoiceOnBoardingSubSteps;
        public IObjectionNotificationRepository ObjectionNotificationRepository
        {
            get
            {
                if (_objectionNotificationRepository == null)
                    _objectionNotificationRepository = new ObjectionNotificationRepository(_context);
                return _objectionNotificationRepository;
            }
            set
            {
                _objectionNotificationRepository = value;
            }
        }
        public IObjectionRepository ObjectionRepository
        {
            get
            {
                if (_objectionRepository == null)
                    _objectionRepository = new ObjectionRepository(_context);
                return _objectionRepository;
            }
            set
            {
                _objectionRepository = value;
            }
        }
        public ICollectionRepository CollectionRepository
        {
            get
            {
                if (_collectionRepository == null)
                    _collectionRepository = new CollectionRepository(_context);
                return _collectionRepository;
            }
            set
            {
                _collectionRepository = value;
            }
        }
        public IAuditLogRepository AuditLogRepository
        {
            get
            {
                if (_aLogRepository == null)
                    _aLogRepository = new AuditLogRepository(_context);
                return _aLogRepository;
            }
            set
            {
                _aLogRepository = value;
            }
        }
        public IQuickCollectionRepository QuickCollectionRepository
        {
            get
            {
                if (_qcollectionRepository == null)
                    _qcollectionRepository = new QuickCollectionRepository(_context);
                return _qcollectionRepository;
            }
            set
            {
                _qcollectionRepository = value;
            }
        }
        public IQuickObjectionRepository QuickObjectionRepository
        {
            get
            {
                if (_quickObjectionRepository == null)
                    _quickObjectionRepository = new QuickObjectionRepository(_context);
                return _quickObjectionRepository;
            }
            set
            {
                _quickObjectionRepository = value;
            }
        }
        public IInvitiesRepository InvitesRepository
        {
            get
            {
                if (_invitesRepository == null)
                    _invitesRepository = new InvitiesRepository(_context);
                return _invitesRepository;
            }
            set
            {
                _invitesRepository = value;
            }
        }
        public ICCSRepository CCSRepository
        {
            get
            {
                if (_ccsRepository == null)
                    _ccsRepository = new CCSRepository(_context);
                return _ccsRepository;
            }
            set
            {
                _ccsRepository = value;
            }
        }
        public IObjectionLogRepository ObjectionLogRepository
        {
            get
            {
                if (_objectionLogRepository == null)
                    _objectionLogRepository = new ObjectionLogRepository(_context);
                return _objectionLogRepository;
            }
            set
            {
                _objectionLogRepository = value;
            }
        }
        public ISessionRepository SessionRepository
        {
            get
            {
                if (_sessionRepository == null)
                    _sessionRepository = new SessionRepository(_context);
                return _sessionRepository;
            }
            set
            {
                _sessionRepository = value;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
            set
            {
                _userRepository = value;
            }
        }
        public IParasaleRepository ParasaleRepository
        {
            get
            {
                if (_parasaleRepository == null)
                    _parasaleRepository = new ParasaleRepository(_context);
                return _parasaleRepository;
            }
            set
            {
                _parasaleRepository = value;
            }
        }
        public IUserHistory UserHistory
        {
            get
            {
                if (_userHistory == null)
                    _userHistory = new UserHistoryRepository(_context);
                return _userHistory;
            }
            set
            {
                _userHistory = value;
            }
        }
        public IVoiceOnboardingData VoiceOnboarding
        {
            get
            {
                if (_IVoiceOnboardingData == null)
                    _IVoiceOnboardingData = new VoiceOnBoardingRepository(_context);
                return _IVoiceOnboardingData;
            }
            set
            {
                _IVoiceOnboardingData = value;
            }
        }

        //public IVoiceOnBoardingSubSteps VoiceOnboardingSubSteps
        //{
        //    get
        //    {
        //        if (_IVoiceOnBoardingSubSteps == null)
        //            _IVoiceOnBoardingSubSteps = new VoiceOnBoardingRepositorySubSteps(_context);
        //        return _IVoiceOnBoardingSubSteps;
        //    }
        //    set
        //    {
        //        _IVoiceOnBoardingSubSteps = value;
        //    }
        //}
    }
}

