using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class VoiceOnBoardingRepository : IVoiceOnboardingData
    {
        private ParasaleDbContext _context;
        public VoiceOnBoardingRepository(ParasaleDbContext context)
        {
            _context = context;
        }
        public VoiceOnBoarding FirstOrDefaultVB()
        {
            return _context.VoiceOnBoardings.FirstOrDefault();
        }
        public VoiceOnBoarding GetVoiceOnBoardingByUserId(string userid)
        {
            return _context.VoiceOnBoardings.Where(a => 1== 1).FirstOrDefault();
        }

        public IEnumerable<VoiceOnBoarding> GetVoiceOnBoardings()
        {
            return _context.VoiceOnBoardings.Where(a => 1 == 1);
        }
    }
}
