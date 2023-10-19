using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parasale.Models;
using Parasale.Repository;
using Parasale.WebUI.Models;
using Vereyon.Web;

namespace Parasale.WebUI.Controllers
{
    //[Authorize(Roles = "User")]
    public class SpeechController : Controller
    {
        private IRepositoryWrapper _repository;
        private UserManager<AppUser> _userManager;
        private IFlashMessage _flashMessage;

        public SpeechController(IRepositoryWrapper repository, UserManager<AppUser> userManager, IFlashMessage flashMessage)
        {
            _repository = repository;
            _userManager = userManager;
            _flashMessage = flashMessage;
        }

        [HttpPost]
        public async Task<IActionResult> RecordSpeechToText(string speech)
        {
            if (ModelState.IsValid)
            {
                await _repository.ParasaleRepository.Add(new speechToText()
                {
                    Text = speech,
                    AppUser = await _userManager.FindByNameAsync(User.Identity.Name),
                    Recieved = false
                });
                if (await _repository.ParasaleRepository.Save())
                {
                    return Json(true);
                }
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> GetObjections(int colId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var pushedObjections = _repository.ObjectionNotificationRepository.GetObjectionsPushedByManager(user.Id);
            var objections = new List<Objection>();
            //if (string.IsNullOrWhiteSpace(user.AssignedManager))
            // objections = _repository.ObjectionRepository.GetAllbjections();
            objections = _repository.ObjectionRepository.getObjectionsbyCollection(colId);
            //else if (pushedObjections != null && pushedObjections.Count > 0)
            //    objections = pushedObjections.Select(p => new Objection()
            //    {
            //        Id = Convert.ToInt32(p.Objection.Id),
            //        InitialObjection = p.Objection.InitialObjection,
            //        PitchKeyword = p.Objection.PitchKeyword,
            //        BadPitchResponse = p.Objection.BadPitchResponse,
            //        ValidPitchResponse = p.Objection.ValidPitchResponse
            //    }).ToList();
            //else
            //    objections = _repository.ObjectionRepository.GetAllbjectionsFromManager(user.AssignedManager);

            if (objections != null && objections.Count > 0)
                return Json(new { objections });

            return Json(false);
        }

        [HttpGet]
        public IActionResult GetUpdatedObjections(bool IsCompleted)
        {
            return ViewComponent("ObjectionLog", new { name = User.Identity.Name, isCompleted = IsCompleted });
        }

        [HttpPost]
        public async Task<IActionResult> LogObjections(int objectionId, bool isCompleted, CCS arrayTobeSaved, int sessionId, int? CollectionId, int? DialectDataId, string DialectDataName, string Dialect)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            int objectionLog = 0;
            ObjectionLog log = new ObjectionLog();
            //if (!(user.IsManager || user.IsCompanyAdmin))
            //{
            if (ModelState.IsValid)
            {
                try
                {
                    await _repository.ParasaleRepository.Add(new UserHistory()
                    {
                        CollectionId = CollectionId ?? 0,
                        LanguageDialect = Dialect,
                        UserId = user.Id,
                        DialectDataName = DialectDataName,
                        DialectDataId = DialectDataId,
                    });
                    try
                    {
                        await _repository.ParasaleRepository.Save();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                log.IsCompleted = isCompleted;
                log.AppUser = user;//await _userManager.FindByNameAsync(User.Identity.Name);
                log.ObjectionTime = DateTime.Now;
                log.AssignedAdmin = user.AssginedAdmin;
                log.Objection = _repository.ObjectionRepository.GetObjection(objectionId);
                await _repository.ParasaleRepository.Add(log);
                //{
                //    AppUser = await _userManager.FindByNameAsync(User.Identity.Name),
                //    IsCompleted = isCompleted,
                //    ObjectionTime = DateTime.Now,
                //    AssignedAdmin = user.AssginedAdmin,
                //    Objection = _repository.ObjectionRepository.GetObjection(objectionId)
                //});
                arrayTobeSaved.appUser = user;
                arrayTobeSaved.objection = _repository.ObjectionRepository.GetObjection(objectionId);
                arrayTobeSaved.TimeStamp = DateTime.Now;
                arrayTobeSaved.WPMTarget = 140;
                await _repository.ParasaleRepository.Add(arrayTobeSaved);
                try
                {
                    await _repository.ParasaleRepository.Save();
                }
                catch (Exception ex)
                {

                }
                SessionObjection sessionObjection = new SessionObjection();
                sessionObjection.session = _repository.SessionRepository.GetSessionByID(sessionId);
                sessionObjection.cCS = _repository.CCSRepository.getCCsById(arrayTobeSaved.Id);
                sessionObjection.objection = _repository.ObjectionRepository.GetObjection(objectionId);
                sessionObjection.objectionLog = _repository.ObjectionLogRepository.getObjectionLogById(log.Id);
                await _repository.ParasaleRepository.Add(sessionObjection);
                if (isCompleted)
                {
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = user.UserName,
                        Type = "Collection UnAssigned",
                        ManagerId = user.AssignedManager,
                        AdminId = user.AssginedAdmin,
                        UserId = user.Id,
                        Field = "User : " + user.UserName + " , Completed an Objection : " + _repository.ObjectionRepository.GetObjection(objectionId).InitialObjection,
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now

                    });
                }
                else
                {
                    await _repository.ParasaleRepository.Add(new AuditLog()
                    {
                        PreviousData = user.UserName,
                        Type = "Collection UnAssigned",
                        ManagerId = user.AssignedManager,
                        UserId = user.Id,
                        AdminId = user.AssginedAdmin,
                        Field = "User : " + user.UserName + " , Missed an Objection : " + _repository.ObjectionRepository.GetObjection(objectionId),
                        ModifiedBy = User.Identity.Name,
                        ModifiedDate = DateTime.Now
                    });
                }

                if (await _repository.ParasaleRepository.Save())
                {
                    return Json(true);
                }
            }
            //}
            return Json(false);
        }
        [HttpPost]
        public async Task<JsonResult> SetCollectionAndDialect(int? CollectionId, int? DialectDataId, string DialectDataName, string Dialect)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _repository.ParasaleRepository.Add(new UserHistory()
            {
                CollectionId = CollectionId ?? 0,
                LanguageDialect = Dialect,
                UserId = user.Id,
                DialectDataName = DialectDataName,
                DialectDataId = DialectDataId,
            });
            if (await _repository.ParasaleRepository.Save())
            {
                _flashMessage.Confirmation("Objection Saved Successfully");
            }
            else
            {
                _flashMessage.Danger("Error Occurred while saving objection");
            }
            return Json(new { status = "success" });
        }
        [HttpPost]
        public async Task<IActionResult> SaveSession(string sessionStart)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var lastId = _repository.SessionRepository.GetLastSessionID(user.Id);
            lastId = lastId + 1;
            UserSession userSession = new UserSession();
            userSession.appUser = user;
            userSession.SessionID = lastId;
            userSession.SessionName = "session_" + user.Name + "_" + lastId;
            userSession.SessionStart = Convert.ToDateTime(sessionStart);
            await _repository.ParasaleRepository.Add(userSession);
            await _repository.ParasaleRepository.Save();
            SessionViewModel model = new SessionViewModel()
            {
                SessionID = userSession.SessionID,
                SessionEnd = userSession.SessionEnd,
                SessionName = userSession.SessionName,
                SessionStart = userSession.SessionStart,
                Id = userSession.Id,

            };

            return Json(new { success = true, model = model });
        }

        public async Task<IActionResult> UpdateSession(int sessionID, string sessionEnd)
        {
            var session = _repository.SessionRepository.GetSessionByID(sessionID);
            if (session != null)
            {
                session.SessionEnd = Convert.ToDateTime(sessionEnd);
                _repository.ParasaleRepository.Update(session);
                await _repository.ParasaleRepository.Save();
            }
            return Json(true);
        }


        [HttpGet]
        public async Task<IActionResult> GetObjectionViews(int colId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var objections = new List<Objection>();
            objections = _repository.ObjectionRepository.getObjectionsbyCollection(colId);

            if (objections != null && objections.Count > 0)

                return PartialView(objections);

            return Json(false);
        }
        #region Save Later And SaveStart PopUp
        public async Task<IActionResult> SaveLater(string Urlpath, string step, int? StepLevel, string StepTitle)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                user.currentStep = step;
                user.path = Urlpath;
                user.StepLevel = StepLevel;
                user.StepTitle = StepTitle;
                _repository.ParasaleRepository.Update(user);
                await _repository.ParasaleRepository.Save();
            }
            return Json(true);
        }
        //public async Task<IActionResult> SaveStartPopUp()
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    try
        //    {
        //        await _repository.ParasaleRepository.Add(new VoiceOnBoarding()
        //        {
        //            IsStartupPopUp = false,
        //            UserId = user.Id
        //        });
        //        try
        //        {
        //            await _repository.ParasaleRepository.Save();
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(true);
        //}
        #endregion
        #region FinishTour
        public async Task<IActionResult> FinishTour()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.voiceOnboarding = true;
            _repository.ParasaleRepository.Update(user);
            await _repository.ParasaleRepository.Save();

            #region Delete DummyData
            var objectionLog = _repository.ObjectionLogRepository.GetObjectionLogsbyUser(user.Id);
            if (objectionLog != null)
            {
                _repository.ParasaleRepository.Delete(objectionLog);
            }
            var Ccs = _repository.CCSRepository.GetScorebyObjectionbyUser(user.Id);
            if (Ccs != null)
            {
                _repository.ParasaleRepository.Delete(Ccs);
            }
            var qCollection = _repository.QuickCollectionRepository.getCollection(user.Id);
            var qObjection = _repository.QuickObjectionRepository.GetAllObjectionbyCollectionId(qCollection.Id);
            if (qObjection != null)
            {
                _repository.ParasaleRepository.Delete(qObjection);
            }
            if (qCollection != null)
            {
                _repository.ParasaleRepository.Delete(qCollection);
            }
            var objection = _repository.ObjectionRepository.GetObjection(user.Id);
            if (objection != null)
            {
                _repository.ParasaleRepository.Delete(objection);
            }
            var collection = _repository.CollectionRepository.GetCollection(user.Id);
            if (collection != null)
            {
                _repository.ParasaleRepository.Delete(collection);
            }
            await _repository.ParasaleRepository.Save();
            #endregion
            return Json(true);
        }

        #endregion
        public async Task<IActionResult> GetUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return Json(user);
        }
        public async Task<IActionResult> OnFirstLogin()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                user.IsFirstLogin = false;
                _repository.ParasaleRepository.Update(user);
                await _repository.ParasaleRepository.Save();
            }
            return Json(true);
        }
        public async Task<IActionResult> GetVoiceOnboardings()
        {
            IEnumerable<VoiceOnBoarding> VoiceOnBoardings = _repository.VoiceOnboarding.GetVoiceOnBoardings();
            return Json(VoiceOnBoardings);
        }
        //public async Task<IActionResult> GetStepsByVOBID(int? VOBId)
        //{
        //    IEnumerable<VoiceOnBoardingSubSteps> voiceOnBoardingSubSteps = _repository.VoiceOnboardingSubSteps.GetAllSubStepsByVOBId(VOBId);
        //    return Json(voiceOnBoardingSubSteps);
        //}
    }
}